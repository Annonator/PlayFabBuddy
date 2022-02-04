using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayerAccountAdapter
{
    public Task Delete(MasterPlayerAccountEntity account);
    public Task<MasterPlayerAccountEntity> LoginWithCustomId(string customId);
}
