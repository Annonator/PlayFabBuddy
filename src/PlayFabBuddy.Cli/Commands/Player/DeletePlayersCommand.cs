using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersCommand : AsyncCommand<DeletePlayersCommandSettings>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public DeletePlayersCommand(IPlayerAccountAdapter playerAccounterAdapter, IRepository<MasterPlayerAccountAggregate> repo)
    {
        _repository = repo;
        _playerAccountAdapter = playerAccounterAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, DeletePlayersCommandSettings settings)
    {
        var countItemsDeletedTask = await _repository.Get();
        var countItemsDeleted = countItemsDeletedTask.Count;

        if (settings.FromLocal.Length > 0)
        {
            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var task = ctx.AddTask("[yellow]Deleting Users[/]", false);
                task.StartTask();

                var command = new DeletePlayersUseCase(_playerAccountAdapter, _repository);
                await command.ExecuteAsync();

                task.StopTask();
            });

            await _repository.Save(new List<MasterPlayerAccountAggregate>());
        }
        else //If FromLocal is not selected default to remote
        {
        }

        AnsiConsole.MarkupLine("[bold green]All Users Deleted! Count: " + countItemsDeleted + "[/]");

        return 0;
    }
    private static void HandleVerboseOutput(DeletePlayersBySegmentCommandSettings settings, List<MasterPlayerAccountAggregate> playersInSegment)
    {
        if (settings.Verbose && playersInSegment.Count > 0)
        {
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("MasterPlayer Account ID");

            foreach (var account in playersInSegment)
            {
                table.AddRow(account.MasterPlayerAccount.Id ?? string.Empty);
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}

