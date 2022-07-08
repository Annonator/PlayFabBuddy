using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class ListPlayersCommand : AsyncCommand<ListPlayersCommandSettings>
{
    private readonly IDataExplorerAdapter _dataAdapter;
    private readonly IPlayerAccountAdapter _playerAdapter;

    public ListPlayersCommand(IDataExplorerAdapter dataAdapter, IPlayerAccountAdapter playerAccountAdapter)
    {
        _dataAdapter = dataAdapter;
        _playerAdapter = playerAccountAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, ListPlayersCommandSettings settings)
    {
        var getPlayerUsecase =
            new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(settings.IpAddress));

        var playerList = new List<MasterPlayerAccountAggregate>();

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .Start("Searching for Players...", async ctx =>
            {
                playerList = await getPlayerUsecase.ExecuteAsync();
            });

        if (playerList.Count == 0)
        {
            AnsiConsole.MarkupLine("No Player for given IP have been found.");
            return 0;
        }

        var playerTable = new Table();
        // Add some columns
        playerTable.AddColumn("Master Id");
        playerTable.AddColumn("Last Known IP");
        playerTable.AddColumn("TitlePlayerData");

        foreach (var aggregate in playerList)
        {
            var titlePlayer = "";

            if (aggregate.MasterPlayerAccount.PlayerAccounts != null)
            {
                foreach (var playerAccount in aggregate.MasterPlayerAccount.PlayerAccounts)
                {
                    titlePlayer += "ID: " + playerAccount.Id + "\n ";
                }
            }

            playerTable.AddRow(new Text(aggregate.MasterPlayerAccount.Id!),
                new Text(aggregate.MasterPlayerAccount.LastKnownIp!),
                new Text(titlePlayer));
        }

        AnsiConsole.Write(playerTable);

        return 0;
    }
}