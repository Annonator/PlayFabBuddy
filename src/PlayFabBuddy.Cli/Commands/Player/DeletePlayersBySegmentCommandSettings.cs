using System.ComponentModel;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersBySegmentCommandSettings : PlayerSettings
{
    [Description(
        $"The name of the segment Accounts should be deleted from. Use \"{IPlayStreamAdapter.AllPlayersSegmentName}\" to delete all Players.")]
    [CommandArgument(0, "<SegmentName>")]
    public string SegmentName { get; init; } = "";
    
    [Description("Prints additional information, like the Master Player Account IDs of the deleted accounts")]
    [CommandOption("-V|--verbose")]
    [DefaultValue(false)]
    public bool Verbose { get; set; }
}