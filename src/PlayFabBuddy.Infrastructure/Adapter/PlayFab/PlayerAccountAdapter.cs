using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFabBuddy.Lib.Adapter.Accounts;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class PlayerAccountAdapter : IPlayerAccountAdapter<MasterPlayerAccountEntity>
{
    public async Task Delete(MasterPlayerAccountEntity account)
    {
        var request = new DeleteMasterPlayerAccountRequest
        {
            PlayFabId = account.Id
        };

        await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
    }

    public async Task<MasterPlayerAccountEntity> LoginWithCustomId(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        /*
         * Result:
         *  AuthenticationContext: 
         *      EntityId = Title Player Account Id
         *      PlayFabId = Master Player account Id
         *  
         */
        var loginResult = await PlayFabClientAPI.LoginWithCustomIDAsync(request);
        var mainAccount = new MasterPlayerAccountAdapter(loginResult.Result.AuthenticationContext.PlayFabId);
        var titleAccount =
            new TitlePlayerAccountAdapter(loginResult.Result.AuthenticationContext.EntityId, mainAccount.MainAccount);

        return mainAccount.MainAccount;
    }
}
