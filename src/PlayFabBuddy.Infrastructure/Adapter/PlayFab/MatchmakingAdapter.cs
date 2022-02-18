using PlayFab;
using PlayFab.MultiplayerModels;
using PlayFabBuddy.Lib.Entities.Matchmaking;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class MatchmakingAdapter : IMatchmakingAdapter
{
    private readonly PlayFabMultiplayerInstanceAPI _api;

    public MatchmakingAdapter(PlayFabMultiplayerInstanceAPI api)
    {
        _api = api;
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task CreateQueueAsync(QueueConfigEntity queueConfig)
    {
        var createQueueRequest = new SetMatchmakingQueueRequest
        {
            MatchmakingQueue = new MatchmakingQueueConfig
            {
                Name = queueConfig.Name,
                MinMatchSize = queueConfig.MinMatchSize,
                MaxMatchSize = queueConfig.MaxMatchSize
            }
        };

        var result = await _api.SetMatchmakingQueueAsync(createQueueRequest);

        if (result.Error != null)
        {
            throw new Exception(result.Error.ToString());
        }
    }
}
