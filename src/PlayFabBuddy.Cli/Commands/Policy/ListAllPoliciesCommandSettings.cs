using System.ComponentModel;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Policy;
public class ListAllPoliciesCommandSettings : PolicySettings
{
    [Description("Gets all policies from PlayFab")]
    [CommandOption("-r|--remote")]
    [CommandArgument(1, "[Remote]")]
    public bool IsRemote { get; set; } = false;
}
