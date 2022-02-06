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
}