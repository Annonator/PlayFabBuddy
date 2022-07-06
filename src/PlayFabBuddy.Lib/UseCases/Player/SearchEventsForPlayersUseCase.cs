using System.Net;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class SearchEventsForPlayersUseCase : UseCase<List<MasterPlayerAccountEntity>>
{
    private IDataExplorerAdapter _dataExplorerAdapter;
    private IPAddress _ip;

    public SearchEventsForPlayersUseCase(IDataExplorerAdapter dataExplorerAdapter, IPAddress ip)
    {
        _dataExplorerAdapter = dataExplorerAdapter;
        _ip = ip;
    }

    public override Task<List<MasterPlayerAccountEntity>> ExecuteAsync(IProgress<double>? progress = null)
    {
        return _dataExplorerAdapter.GetPlayersByIp(_ip);
    }
}