using System.ComponentModel;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Matchmaking;

public class CreateQueueCommandSettings : MatchmakingSettings
{
    [Description("Path for an existing Matchmaking configuration in JSON")]
    [CommandOption("-c|--config")]
    [CommandArgument(0, "[configPath]")]
    public string ConfigPath { get; set; } = "";
}
