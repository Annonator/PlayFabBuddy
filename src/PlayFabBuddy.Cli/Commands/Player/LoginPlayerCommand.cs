using PlayFabBuddy.Infrastructure.Repositories;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases.Player;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Player;

public class LoginPlayerCommand : AsyncCommand<LoginPlayerCommandSettings>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _localFileRepository;
    private readonly IRepository<MasterPlayerAccountAggregate> _remoteSegmentRepository;
    private readonly IPlayerAccountAdapter _accountAdapter;

    public LoginPlayerCommand(LocalMasterPlayerAccountRepository localFileRepository, SegmentMasterPlayerAccountRepository remoteSegmentRepository, IPlayerAccountAdapter playerAccountAdapter)
    {
        _localFileRepository = localFileRepository;
        _remoteSegmentRepository = remoteSegmentRepository;
        _accountAdapter = playerAccountAdapter;
    }
    public async override Task<int> ExecuteAsync(CommandContext context, LoginPlayerCommandSettings settings)
    {
        LoginPlayerUseCase useCase;
        //local first
        if (settings.FromLocal.Length > 0)
        {
            _localFileRepository.UpdateSettings(new LocalMasterPlayerAccountRepositorySettings(settings.FromLocal));
            useCase = new LoginPlayerUseCase(_localFileRepository, _accountAdapter);
            await RunUseCase(useCase, context);

        }
        else //If FromLocal is not selected default to remote
        {
            _remoteSegmentRepository.UpdateSettings(new SegmentMasterPlayerAccountRepositorySetting { SegmentName = settings.FromRemote });
            useCase = new LoginPlayerUseCase(_remoteSegmentRepository, _accountAdapter);
            await RunUseCase(useCase, context);
        }

        return 0;
    }

    private async Task RunUseCase(LoginPlayerUseCase useCase, CommandContext context)
    {
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[yellow]Login Players[/]", false);
            task.StartTask();
            var progress = new Progress<double>(d => task.Increment(d));

            await useCase.ExecuteAsync(progress);

            while (!ctx.IsFinished)
            {
                task.Increment(0.1);
            }

            AnsiConsole.MarkupLine("[bold green]All Players logged in[/]");
        });
    }
}
