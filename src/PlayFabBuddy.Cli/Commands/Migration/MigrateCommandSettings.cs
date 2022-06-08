using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Migration;

public class MigrateCommandSettings : CommandSettings
{
    [Description("The Title Id you want to copy from")]
    [CommandArgument(0, "<SourceTitleId>")]
    public string SourceTitleId { get; set; } = "";

    [Description("The Title Id you want to copy to")]
    [CommandArgument(1, "<DestinationTitleId>")]
    public string DestinationTitleId { get; set; } = "";

    public override ValidationResult Validate()
    {
        if (SourceTitleId.Length > 0 && DestinationTitleId.Length > 0)
        {
            return ValidationResult.Success();
        }

        return ValidationResult.Error("And unexpected error occured in the command settings");
    }
}