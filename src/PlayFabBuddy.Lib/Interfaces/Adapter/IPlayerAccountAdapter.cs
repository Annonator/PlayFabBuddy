using PlayFabBuddy.Lib.Aggregate;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayerAccountAdapter
{
    public Task Delete(MasterPlayerAccountAggregate account);
    public Task<MasterPlayerAccountAggregate> LoginWithCustomId(string customId);
}
