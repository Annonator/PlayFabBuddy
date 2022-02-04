using System.ComponentModel;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersBySegmentCommandSettings : PlayerSettings
{
    [Description(
        $"The Segment Name the player to delete are in. Use {Lib.Commands.Player.DeletePlayersBySegmentCommand.AllPlayersSegmentName} to telete all Players.")]
    [CommandArgument(0, "<SegmentName>")]
    public string SegmentName { get; init; } = "";
}