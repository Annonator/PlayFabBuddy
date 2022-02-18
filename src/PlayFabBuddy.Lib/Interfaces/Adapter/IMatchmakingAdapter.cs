using PlayFabBuddy.Lib.Entities.Matchmaking;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IMatchmakingAdapter
{
    public Task CreateQueueAsync(QueueConfigEntity queueName);
}
