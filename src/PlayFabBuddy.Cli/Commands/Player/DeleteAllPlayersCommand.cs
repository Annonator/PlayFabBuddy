using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player
{
    public class DeleteAllPlayersCommand : AsyncCommand<DeleteAllPlayersCommandSettings>
    {
        private readonly IRepository<MasterPlayerAccountAggregate> _repository;
        private readonly IPlayerAccountAdapter _playerAccountAdapter;

        public DeleteAllPlayersCommand(IPlayerAccountAdapter playerAccounterAdapter, IRepository<MasterPlayerAccountAggregate> repo)
        {
            _repository = repo;
            _playerAccountAdapter = playerAccounterAdapter;
        }

        public async override Task<int> ExecuteAsync(CommandContext context, DeleteAllPlayersCommandSettings settings)
        {
            var countItemsDeletedTask = await _repository.Get();
            var countItemsDeleted = countItemsDeletedTask.Count;

            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var task = ctx.AddTask("[yellow]Deleting Users[/]", false);
                task.StartTask();

                var command = new DeletePlayersUseCase(_playerAccountAdapter, _repository);
                await command.ExecuteAsync();

                task.StopTask();
            });

            await _repository.Save(new List<MasterPlayerAccountAggregate>());

            AnsiConsole.MarkupLine("[bold green]All Users Deleted! Count: " + countItemsDeleted + "[/]");

            return 0;
        }
    }
}
