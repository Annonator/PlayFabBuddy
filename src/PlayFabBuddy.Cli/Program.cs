using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Cli.Commands.Matchmaking;
using PlayFabBuddy.Cli.Commands.Player;
using PlayFabBuddy.Cli.Commands.Policy;
using PlayFabBuddy.Cli.Commands.Settings;
using PlayFabBuddy.Cli.Infrastructure;
using PlayFabBuddy.Infrastructure;
using PlayFabBuddy.Lib;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("settings.json", false, true);
        builder.AddJsonFile("local.settings.json", true, true);

        var config = builder.Build();

        var registrations = new ServiceCollection();
        registrations.AddSingleton<IConfiguration>(config);
        registrations.AddLibrary(config);
        registrations.AddInfrastructure(config);

        var registrar = new TypeRegistrar(registrations);

        var app = new CommandApp(registrar);

        AnsiConsole.Write(new FigletText("PlayFabBuddy").Centered().Color(Color.Orange1));

        app.Configure(configurator =>
        {
            configurator.AddBranch<PlayerSettings>("players", players =>
            {
                players.AddCommand<CreateNewPlayersCommand>("create");
                players.AddCommand<DeletePlayersCommand>("delete");
                players.AddCommand<LoginPlayerCommand>("login");
            });
            configurator.AddBranch<MatchmakingSettings>("matchmaking", matchmaking =>
            {
                matchmaking.AddCommand<CreateQueueCommand>("create");
            });
            configurator.AddBranch<PolicySettings>("policy", policy =>
            {
                policy.AddCommand<AddPolicyCommand>("add");
                policy.AddCommand<ListAllPoliciesCommand>("list");
            });
            configurator.AddBranch<SettingsSettings>("settings", policy =>
            {
                policy.AddCommand<SetSettingsCommand>("set");
                policy.AddCommand<ListSettingsCommand>("list");
            });
        });

        return await app.RunAsync(args);
    }
}