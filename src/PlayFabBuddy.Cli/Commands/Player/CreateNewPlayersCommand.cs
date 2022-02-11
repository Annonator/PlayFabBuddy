using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class CreateNewPlayersCommand : AsyncCommand<CreateNewPlayersCommandSettings>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public CreateNewPlayersCommand(IPlayerAccountAdapter playerAccountAdapter, IRepository<MasterPlayerAccountAggregate> repo)
    {
        _playerAccountAdapter = playerAccountAdapter;
        _repository = repo;
    }

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

    private async Task CreateUsers(int concurrentUsers, ProgressTask task)
    {
        var commandList = new List<Task<MasterPlayerAccountAggregate>>();
        for (var i = 0; i < concurrentUsers; i++)
        {
            task.Increment(i % 10);
            commandList.Add(new RegisterNewPlayerUseCase(_playerAccountAdapter).ExecuteAsync());
        }

        var results = await Task.WhenAll(commandList);

        await _repository.Append(results.ToList());
    }
}