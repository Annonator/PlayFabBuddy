using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class SegmentMasterPlayerAccountRepository : IRepository<MasterPlayerAccountAggregate>
{
    private readonly IPlayStreamAdapter _adapter;
    private string _segmentName;
    private readonly List<MasterPlayerAccountAggregate> _cache;

    public SegmentMasterPlayerAccountRepository(SegmentMasterPlayerAccountRepositorySetting settings, IPlayStreamAdapter playStreamAdapater)
    {
        _adapter = playStreamAdapater;
        _segmentName = settings.SegmentName;
        _cache = new List<MasterPlayerAccountAggregate>();
    }

    public Task Append(List<MasterPlayerAccountAggregate> toAppend)
    {
        throw new NotImplementedException();
    }

    public Task Clear()
    {
        // We don't need to clear here as PF should already up to daye
        return Task.CompletedTask;
    }

    public async Task<List<MasterPlayerAccountAggregate>> Get()
    {
        var segmentId = await _adapter.GetSegmentById(_segmentName);
        var playersInSegment = await _adapter.GetPlayersInSegment(segmentId);

        return playersInSegment;
    }

    public Task Save(List<MasterPlayerAccountAggregate> toSave)
    {
        throw new NotImplementedException();
    }

    public void UpdateSettings(IRepositorySettings settings)
    {
        if (settings.Equals(typeof(SegmentMasterPlayerAccountRepositorySetting)))
        {
            _segmentName = ((SegmentMasterPlayerAccountRepositorySetting)settings).SegmentName;
        }
    }
}
