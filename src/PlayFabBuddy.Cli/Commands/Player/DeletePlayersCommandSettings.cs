using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersCommandSettings : PlayerSettings
{
    [Description("LoadUsers to log in from local JSON File")]
    [CommandOption("-l|--local")]
    [CommandArgument(0, "[Local]")]
    public string FromLocal { get; set; } = "";

    [Description("LoadUsers to log in from PlayFab Segment")]
    [CommandOption("-r|--remote")]
    [CommandArgument(1, "[Remote]")]
    public string FromRemote { get; set; } = "";

    [Description("Prints additional information, like the Master Player Account IDs of the deleted accounts")]
    [CommandOption("-V|--verbose")]
    [DefaultValue(false)]
    public bool Verbose { get; set; }

    public override ValidationResult Validate()
    {
        if ((FromLocal.Length > 0 && FromRemote.Length == 0) || (FromRemote.Length > 0 && FromLocal.Length == 0))
        {
            return ValidationResult.Success();
        }
        else if (FromLocal.Length > 0 && FromRemote.Length > 0)
        {
            return ValidationResult.Error("Only one option can be set, either FromLocal or FromRemote");
        }
        else
        {
            return ValidationResult.Error("You need to provide a Path for at least one option!");
        }
    }
}