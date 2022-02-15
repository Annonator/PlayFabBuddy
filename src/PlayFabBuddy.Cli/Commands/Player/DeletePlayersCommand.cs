using PlayFabBuddy.Infrastructure.Repositories;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class DeletePlayersCommand : AsyncCommand<DeletePlayersCommandSettings>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _localFileRepository;
    private readonly IRepository<MasterPlayerAccountAggregate> _remoteSegmentRepository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public DeletePlayersCommand(LocalMasterPlayerAccountRepository localFileRepository, SegmentMasterPlayerAccountRepository remoteSegmentRepository, IPlayerAccountAdapter playerAccountAdapter)
    {
        _localFileRepository = localFileRepository;
        _remoteSegmentRepository = remoteSegmentRepository;
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, DeletePlayersCommandSettings settings)
    {
        var countPlayersDeleted = 0;
        List<MasterPlayerAccountAggregate> deletedPlayers;

        if (settings.FromLocal.Length > 0)
        {
            _localFileRepository.UpdateSettings(new LocalMasterPlayerAccountRepositorySettings(settings.FromLocal));
            var useCase = new DeletePlayersUseCase(_playerAccountAdapter, _localFileRepository);
            (countPlayersDeleted, deletedPlayers) = await RunUseCase(useCase, context);

        }
        else //If FromLocal is not selected default to remote
        {
            _remoteSegmentRepository.UpdateSettings(new SegmentMasterPlayerAccountRepositorySetting { SegmentName = settings.FromRemote });
            var useCase = new DeletePlayersUseCase(_playerAccountAdapter, _remoteSegmentRepository);
            (countPlayersDeleted, deletedPlayers) = await RunUseCase(useCase, context);
        }

        AnsiConsole.MarkupLine("[bold green]All Users Deleted! Count: " + countPlayersDeleted + "[/]");

        HandleVerboseOutput(settings, deletedPlayers);

        return 0;
    }

    private async Task<(int countPlayersDeleted, List<MasterPlayerAccountAggregate> deletedPlayers)> RunUseCase(DeletePlayersUseCase useCase, CommandContext context)
    {
        var countPlayersDeleted = 0;
        var deletedPlayers = new List<MasterPlayerAccountAggregate>();

        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Deleting Players[/]", false);
            task.StartTask();
            var progress = new Progress<double>(d => task.Increment(d));

            (countPlayersDeleted, deletedPlayers) = await useCase.ExecuteAsync(progress);

            while (!ctx.IsFinished)
            {
                task.Increment(0.1);
            }
        });

        return (countPlayersDeleted, deletedPlayers);
    }

    private static void HandleVerboseOutput(DeletePlayersCommandSettings settings, List<MasterPlayerAccountAggregate> playersInSegment)
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

