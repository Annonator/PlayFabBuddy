using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class BanPlayersCommand : AsyncCommand<BanPlayersCommandSettings>
{
    private readonly IDataExplorerAdapter _dataExplorerAdapter;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public BanPlayersCommand(IPlayerAccountAdapter playerAccountAdapter, IDataExplorerAdapter dataExplorerAdapter)
    {
        _playerAccountAdapter = playerAccountAdapter;
        _dataExplorerAdapter = dataExplorerAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, BanPlayersCommandSettings settings)
    {
        var getPlayersUseCase = new GetPlayersByIpUseCAse(_dataExplorerAdapter, _playerAccountAdapter,
            IPAddress.Parse(settings.IpAddress));

        var playerList = new List<MasterPlayerAccountAggregate>();


        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .StartAsync("Searching for Players...", async ctx =>
            {
                playerList = await getPlayersUseCase.ExecuteAsync();
            });

        var banUseCase = new BanTitlePlayerAccountsByIpUseCase(_playerAccountAdapter, playerList,
            IPAddress.Parse(settings.IpAddress), " Test ");

        var hasBanned = false;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .StartAsync("Banning Players...", async ctx =>
            {
                hasBanned = await banUseCase.ExecuteAsync();
            });

        if (hasBanned)
        {
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
        }

        return 0;
    }
}