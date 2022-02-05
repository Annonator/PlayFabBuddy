using PlayFabBuddy.Lib.Interfaces.Adapter;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersBySegmentCommand : AsyncCommand<DeletePlayersBySegmentCommandSettings>
{
    private readonly IPlayStreamAdapter playStreamAdapter;
    private readonly IPlayerAccountAdapter playerAccountAdapter;

    public DeletePlayersBySegmentCommand(IPlayStreamAdapter playStreamAdapter, IPlayerAccountAdapter playerAccountAdapter)
    {
        this.playStreamAdapter = playStreamAdapter;
        this.playerAccountAdapter = playerAccountAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, DeletePlayersBySegmentCommandSettings settings)
    {
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Deleting Players[/]", false);
            task.StartTask();

            var command = new Lib.Commands.Player.DeletePlayersBySegmentCommand(this.playStreamAdapter, this.playerAccountAdapter);
            await command.ExecuteAsync(settings.SegmentName);
            
            task.StopTask();
        });

        AnsiConsole.MarkupLine("[bold green]All Players deleted![/]");

        return 0;
    }
}