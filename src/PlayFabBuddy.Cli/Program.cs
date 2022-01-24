using Microsoft.Extensions.Configuration;
using PlayFabBuddy.Cli.Commands.Player;
using PlayFabBuddy.Infrastructure.IoC;
using PlayFabBuddy.Lib.Admin;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.Config;
using PlayFabBuddy.Lib.Util.Repository;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("settings.json");
        builder.AddJsonFile("local.settings.json", true);

        var config = builder.Build();

        var pfConfig = new PlayFabConfig(config["titleId"], config["devSecret"]);

        pfConfig.InitAsync();

        DependencyInjection.Instance.Register<IConfig>(() => pfConfig, RegistrationType.Singleton);

        var defaultAccountOutputPath = "MasterAccountOutput.json";
        DependencyInjection.Instance.Register<IRepository<MasterPlayerAccountEntity>>(
            () => new LocalMasterPlayerAccountRepository(defaultAccountOutputPath), RegistrationType.Singleton);

        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddBranch<PlayerSettings>("players", players =>
            {
                players.AddCommand<CreateNewPlayersCommand>("create");
                players.AddCommand<DeleteAllPlayersCommand>("delete");
            });
        });

        return await app.RunAsync(args);
    }
}