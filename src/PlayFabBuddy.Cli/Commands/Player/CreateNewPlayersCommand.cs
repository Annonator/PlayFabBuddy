using PlayFabBuddy.Lib.Commands.Player;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.Repository;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class CreateNewPlayersCommand : AsyncCommand<CreateNewPlayersCommandSettings>
{
    private readonly IRepository<MasterPlayerAccountEntity> _repository;

    public CreateNewPlayersCommand(IRepository<MasterPlayerAccountEntity> repo)
    {
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
        var commandList = new List<Task<MasterPlayerAccountEntity>>();
        for (var i = 0; i < concurrentUsers; i++)
        {
            task.Increment(i % 10);
            commandList.Add(new RegisterNewPlayerCommand().ExecuteAsync());
        }

        var results = await Task.WhenAll(commandList);

        await _repository.Append(results.ToList());
    }
}