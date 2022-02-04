using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersBySegmentCommand : AsyncCommand<DeletePlayersBySegmentCommandSettings>
{
    public async override Task<int> ExecuteAsync(CommandContext context, DeletePlayersBySegmentCommandSettings settings)
    {
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Deleting Players[/]", false);
            task.StartTask();

            // TODO call the command
            
            task.StopTask();
        });

        AnsiConsole.MarkupLine("[bold green]All Players deleted![/]");

        return 0;
    }
}