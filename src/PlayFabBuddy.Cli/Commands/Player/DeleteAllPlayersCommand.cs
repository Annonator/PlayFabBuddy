using PlayFabBuddy.Lib.Commands.Player;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.IoC;
using PlayFabBuddy.Lib.Util.Repository;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player
{
    public class DeleteAllPlayersCommand : AsyncCommand<DeleteAllPlayersCommandSettings>
    {
        private readonly IRepository<MasterPlayerAccountEntity> _repository;

        public DeleteAllPlayersCommand()
        {
            _repository = DependencyInjection.Instance.Resolve<IRepository<MasterPlayerAccountEntity>>();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, DeleteAllPlayersCommandSettings settings)
        {
            var countItemsDeleted = _repository.Get().Count;

            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var task = ctx.AddTask("[yellow]Deleting Users[/]", false);
                task.StartTask();

                var command = new DeletePlayersCommand(_repository);
                await command.ExecuteAsync();

                task.StopTask();
            });
            
            await _repository.Save(new List<MasterPlayerAccountEntity>());

            AnsiConsole.MarkupLine("[bold green]All Users Deleted! Count: " + countItemsDeleted + "[/]");

            return 0;
        }
    }
}
