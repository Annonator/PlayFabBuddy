using PlayFabBuddy.Lib.Commands.Player;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player
{
    public class DeleteAllPlayersCommand : AsyncCommand<DeleteAllPlayersCommandSettings>
    {
        private readonly IRepository<MasterPlayerAccountEntity> _repository;
        private readonly IPlayerAccountAdapter _playerAccountAdapter;

        public DeleteAllPlayersCommand(IPlayerAccountAdapter playerAccounterAdapter, IRepository<MasterPlayerAccountEntity> repo)
        {
            _repository = repo;
            _playerAccountAdapter = playerAccounterAdapter;
        }

        public async override Task<int> ExecuteAsync(CommandContext context, DeleteAllPlayersCommandSettings settings)
        {
            var countItemsDeleted = _repository.Get().Count;

            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var task = ctx.AddTask("[yellow]Deleting Users[/]", false);
                task.StartTask();

                var command = new DeletePlayersCommand(_playerAccountAdapter, _repository);
                await command.ExecuteAsync();

                task.StopTask();
            });

            await _repository.Save(new List<MasterPlayerAccountEntity>());

            AnsiConsole.MarkupLine("[bold green]All Users Deleted! Count: " + countItemsDeleted + "[/]");

            return 0;
        }
    }
}
