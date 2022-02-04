using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayerAccountAdapter<T>
{
    public Task Delete(MasterPlayerAccountEntity account);
    public Task<T> LoginWithCustomId(string customId);
}
