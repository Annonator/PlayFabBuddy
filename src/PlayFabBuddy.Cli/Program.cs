using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Cli.Commands.Matchmaking;
using PlayFabBuddy.Cli.Commands.Player;
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
        builder.AddJsonFile("settings.json");
        builder.AddJsonFile("local.settings.json", true);

        var config = builder.Build();

        var registrations = new ServiceCollection();
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
        });

        return await app.RunAsync(args);
    }
}