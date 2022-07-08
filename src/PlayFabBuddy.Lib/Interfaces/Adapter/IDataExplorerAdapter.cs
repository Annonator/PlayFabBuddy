using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IDataExplorerAdapter
{
    public Task<List<MasterPlayerAccountEntity>> GetPlayersByIp(IPAddress ip);
}