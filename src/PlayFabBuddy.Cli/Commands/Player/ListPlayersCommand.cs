using System.Net;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class ListPlayersCommand : AsyncCommand<ListPlayersCommandSettings>
{
    private IDataExplorerAdapter adapter;

    public ListPlayersCommand(IDataExplorerAdapter adapter)
    {
        this.adapter = adapter;
    }

    public override Task<int> ExecuteAsync(CommandContext context, ListPlayersCommandSettings settings)
    {
        var nase = new SearchEventsForPlayersUseCase(adapter, IPAddress.Parse(settings.IpAddress));

        nase.ExecuteAsync();

        return Task.FromResult(0);
    }
}