using System.ComponentModel;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersBySegmentCommandSettings : PlayerSettings
{
    [Description(
        $"The Segment Name the player to delete are in. Use {IPlayStreamAdapter.AllPlayersSegmentName} to telete all Players.")]
    [CommandArgument(0, "<SegmentName>")]
    public string SegmentName { get; init; } = "";
}