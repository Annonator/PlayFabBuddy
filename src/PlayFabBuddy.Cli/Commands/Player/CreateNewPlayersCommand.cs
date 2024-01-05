using PlayFabBuddy.Infrastructure.Exceptions;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class CreateNewPlayersCommand(
    IPlayerAccountAdapter playerAccountAdapter)
    : AsyncCommand<CreateNewPlayersCommandSettings>
{
    public async override Task<int> ExecuteAsync(CommandContext context, CreateNewPlayersCommandSettings settings)
    {
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Creating Users[/]", false);
            task.StartTask();

            await CreateUsers(settings.NumberOfUsers, task);

            task.StopTask();
        });

        AnsiConsole.MarkupLine("[bold green]All Users Created![/]");

        return 0;
    }

    private async Task CreateUsers(int numberOfUsersToCreate, ProgressTask task)
    {
        task.MaxValue(numberOfUsersToCreate);
        for (var i = 0; i < numberOfUsersToCreate; i++)
        {
            task.Increment(1);
            try
            {
                await new RegisterNewPlayerUseCase(playerAccountAdapter).ExecuteAsync();
            }
            catch (AddPlayerRateLimitException e)
            {
                AnsiConsole.MarkupLine($"[red]Rate Limit Exceeded, waiting for {e.RetryInSeconds} seconds[/]");

                var retry = 0;
                if (e.RetryInSeconds is not null)
                {
                    retry = (int)e.RetryInSeconds;
                }

                Thread.Sleep(1000 * retry);

                // We need to decrement the counter because we didn't actually create a user and need to retry
                i--;
            }
        }
    }
}