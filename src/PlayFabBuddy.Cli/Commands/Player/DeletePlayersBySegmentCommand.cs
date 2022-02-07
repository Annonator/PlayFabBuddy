using PlayFabBuddy.Lib.Entities.Accounts;
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
        var masterPlayerAccounts = new List<MasterPlayerAccountEntity>();
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Deleting Players[/]", false);
            task.StartTask();

            var command = new Lib.Commands.Player.DeletePlayersBySegmentCommand(this.playStreamAdapter, this.playerAccountAdapter);
            var progress = new Progress<double>(d =>
            {
                task.Increment(d);
            });
            
            var (totalRemoved, accountEntities) = await command.ExecuteAsync(settings.SegmentName, progress);
            masterPlayerAccounts = accountEntities;

            while (!ctx.IsFinished)
            {
                task.Increment(0.1);
            }
            
            AnsiConsole.MarkupLine($"[bold green]Removed {totalRemoved} Master Player Accounts![/]");
        });

        HandleVerboseOutput(settings, masterPlayerAccounts);

        return 0;
    }

    private static void HandleVerboseOutput(DeletePlayersBySegmentCommandSettings settings, List<MasterPlayerAccountEntity> playersInSegment)
    {
        if (settings.Verbose && playersInSegment.Count > 0)
        {
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("MasterPlayer Account ID");

            foreach (var account in playersInSegment)
            {
                table.AddRow(account.Id ?? string.Empty);
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}