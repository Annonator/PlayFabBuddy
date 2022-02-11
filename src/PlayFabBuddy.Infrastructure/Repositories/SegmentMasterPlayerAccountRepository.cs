using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class SegmentMasterPlayerAccountRepository : IRepository<MasterPlayerAccountAggregate>
{
    private readonly IPlayStreamAdapter _adapter;
    private readonly string _segmentName;
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
}
