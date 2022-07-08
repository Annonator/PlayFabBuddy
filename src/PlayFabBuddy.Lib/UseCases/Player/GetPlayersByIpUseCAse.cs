using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class GetPlayersByIpUseCAse : UseCase<List<MasterPlayerAccountAggregate>>
{
    private readonly IDataExplorerAdapter _dataExplorerAdapter;
    private readonly IPAddress _ip;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public GetPlayersByIpUseCAse(IDataExplorerAdapter dataExplorerAdapter, IPlayerAccountAdapter playerAccountAdapter,
        IPAddress ip)
    {
        _dataExplorerAdapter = dataExplorerAdapter;
        _playerAccountAdapter = playerAccountAdapter;
        _ip = ip;
    }

    public async override Task<List<MasterPlayerAccountAggregate>> ExecuteAsync(IProgress<double>? progress = null)
    {
        var entityList = await _dataExplorerAdapter.GetPlayersByIp(_ip);

        var aggregateList = new List<MasterPlayerAccountAggregate>();

        foreach (var masterPlayerEntity in entityList)
        {
            var masterAggregate = new MasterPlayerAccountAggregate(masterPlayerEntity);
            aggregateList.Add(await _playerAccountAdapter.GetTitleAccountsAndCustomId(masterAggregate));
        }

        return aggregateList;
    }
}