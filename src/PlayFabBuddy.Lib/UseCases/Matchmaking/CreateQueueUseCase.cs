using PlayFabBuddy.Lib.Entities.Matchmaking;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Matchmaking;

public class CreateQueueUseCase : UseCase<bool>
{
    private readonly QueueConfigEntity _queueConfig;
    private readonly IMatchmakingAdapter _matchmakingAdapter;

    public CreateQueueUseCase(QueueConfigEntity queueConfig, IMatchmakingAdapter matchmakingAdapter)
    {
        _queueConfig = queueConfig;
        _matchmakingAdapter = matchmakingAdapter;
    }

    public async override Task<bool> ExecuteAsync(IProgress<double>? progress = null)
    {
        await _matchmakingAdapter.CreateQueueAsync(_queueConfig);

        return true;
    }
}
