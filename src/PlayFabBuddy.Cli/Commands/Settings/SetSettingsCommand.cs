using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Settings;

public class SetSettingsCommand : AsyncCommand<SetSettingsSettings>
{
    private readonly IConfiguration _config;

    public SetSettingsCommand(IConfiguration config)
    {
        _config = config;
    }

    public override Task<int> ExecuteAsync(CommandContext context, SetSettingsSettings settings)
    {
        if (!settings.WantToOverwrite)
        {
            if (!AnsiConsole.Confirm("This will overwrite any existing local settings, want to continue?"))
            {
                return Task.FromResult(0);
            }
        }

        if (settings.TitleId.Length == 0)
        {
            settings.TitleId = AnsiConsole.Ask<string>("Whats the Title Id you want PlayFabBuddy to use?");
        }

        if (settings.DevSecret.Length == 0)
        {
            settings.DevSecret = AnsiConsole.Prompt(
                new TextPrompt<string>("Whats the Dev Secret for this title?").Secret()
            );
        }

        _config["titleId"] = settings.TitleId;
        _config["devSecret"] = settings.DevSecret;
        _config["defaultSavePath"] = settings.MasterAccountDefaultSavePath;

        // TODO: Move this into a repo, as we are overlapping concerns here. 
        var configAsDictionary = _config.AsEnumerable().ToDictionary(c => c.Key, c => c.Value);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(configAsDictionary, options);
        File.WriteAllText("local.settings.json", json);

        return Task.FromResult(0);
    }
}