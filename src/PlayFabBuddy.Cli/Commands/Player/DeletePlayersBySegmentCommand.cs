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
            var progress = new Progress<double>(d =>
            {
                task.Increment(d);
            });
            await command.ExecuteAsync(settings.SegmentName, progress);
            
            while (!ctx.IsFinished)
            {
                task.Increment(0.1);
            }
        });

        return 0;
    }
}