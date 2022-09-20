using System.ComponentModel;
using System.Net;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class BanPlayersCommandSettings : PlayerSettings
{
    [Description("The IP Address to Ban")]
    [CommandArgument(0, "[IPAddress]")]
    public string IpAddress { get; set; } = "";

    public override ValidationResult Validate()
    {
        if (IpAddress.Length == 0 && !IPAddress.TryParse(IpAddress, out _))
        {
            return ValidationResult.Error("Please provide a valid IPAddress");
        }

        return ValidationResult.Success();
    }
}