using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Settings;

public class ListSettingsCommand : Command<SettingsSettings>
{
    private readonly IConfiguration _config;

    public ListSettingsCommand(IConfiguration config)
    {
        _config = config;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] SettingsSettings settings)
    {
        var table = new Table();
        // Add some columns
        table.AddColumn("Name");
        table.AddColumn("Value");

        var configAsDictionary = _config.AsEnumerable().ToDictionary(c => c.Key, c => c.Value);

        foreach (var config in configAsDictionary)
        {
            table.AddRow(new Text(config.Key), new Text(config.Value));
        }

        AnsiConsole.Write(table);

        return 0;
    }
}