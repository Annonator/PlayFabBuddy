using System.ComponentModel;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class CreateNewPlayersCommandSettings : PlayerSettings
{
    [Description("The Number of users that you want to create")]
    [CommandArgument(0, "<NumberOfUsers>")]
    public int NumberOfUsers { get; set; }
}