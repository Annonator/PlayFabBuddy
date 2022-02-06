using PlayFabBuddy.Lib.Adapter.Accounts;
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
            
            var (totalRemoved, playersInSegment) = await command.ExecuteAsync(settings.SegmentName, progress);
            
            HandleVerboseOutput(settings, playersInSegment);

            while (!ctx.IsFinished)
            {
                task.Increment(0.1);
            }
            AnsiConsole.MarkupLine($"[bold green]Removed {totalRemoved} Master Player Accounts![/]");
        });

        return 0;
    }

    private static void HandleVerboseOutput(DeletePlayersBySegmentCommandSettings settings, List<MasterPlayerAccountAdapter> playersInSegment)
    {
        if (settings.Verbose && playersInSegment.Count > 0)
        {
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("MasterPlayer Account ID");

            foreach (var account in playersInSegment)
            {
                table.AddRow(account.MainAccount.Id ?? string.Empty);
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}