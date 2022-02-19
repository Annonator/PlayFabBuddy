using PlayFabBuddy.Lib.Entities.Matchmaking;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Matchmaking;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Commands.Matchmaking;

public class CreateQueueCommand : AsyncCommand<CreateQueueCommandSettings>
{
    private readonly IMatchmakingAdapter _matchmakingAdapter;


    public CreateQueueCommand(IMatchmakingAdapter matchmakingAdapter)
    {
        _matchmakingAdapter = matchmakingAdapter;
    }

    public async override Task<int> ExecuteAsync(CommandContext context, CreateQueueCommandSettings settings)
    {
        var queueConfig = new QueueConfigEntity();
        // If no config is provided start create wizard, to gather the nessesary information
        if (settings.ConfigPath == "")
        {
            queueConfig.Name = AnsiConsole.Ask<string>("The Name of the Queue:");
            queueConfig.MinMatchSize = AnsiConsole.Ask<uint>("Min Match Size:");
            queueConfig.MaxMatchSize = AnsiConsole.Ask<uint>("Max Match Size:");

            var useCase = new CreateQueueUseCase(queueConfig, _matchmakingAdapter);
            await useCase.ExecuteAsync();
        }


        return 0;
    }
}
