using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class PlayerAccountAdapter : IPlayerAccountAdapter
{
    private readonly PlayFabAdminInstanceAPI _playFabAdminInstanceApi;
    private readonly PlayFabApiSettings _playFabApiSettings;

    public PlayerAccountAdapter(PlayFabAdminInstanceAPI adminInstanceApi, PlayFabApiSettings playFabApiSettings)
    {
        _playFabAdminInstanceApi = adminInstanceApi;
        _playFabApiSettings = playFabApiSettings;
    }

    public async Task Delete(MasterPlayerAccountAggregate account)
    {
        if (account.HasMoreThanOneTitlePlayerAccount())
        {
            throw new Exception($"Master PlayerAccount ID \"{account.MasterPlayerAccount.Id}\" has more than one Title");
        }

        var request = new DeleteMasterPlayerAccountRequest
        {
            PlayFabId = account.MasterPlayerAccount.Id
        };

        await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
    }

    public async Task<MasterPlayerAccountAggregate> LoginWithCustomId(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetTitleData = true,
                GetUserData = true,
                GetUserAccountInfo = true,
            }
        };

        /*
         * Result:
         *  AuthenticationContext: 
         *      EntityId = Title Player Account Id
         *      PlayFabId = Master Player account Id
         *      EntityToken = limited auth token
         *  
         */
        var loginResult = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        var masterPlayerAccount = new MasterPlayerAccountEntity
        {
            Id = loginResult.Result.AuthenticationContext.PlayFabId,
            CustomId = customId
        };
        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = loginResult.Result.AuthenticationContext.EntityId,
            TitleId = _playFabApiSettings.TitleId
        };
        var aggregate = new MasterPlayerAccountAggregate(masterPlayerAccount);
        aggregate.AddTitlePlayerAccount(titlePlayerAccount);

        return aggregate;
    }

    public async Task<MasterPlayerAccountAggregate> GetTitleAccountsAndCustomId(MasterPlayerAccountAggregate account)
    {
        var request = new LookupUserAccountInfoRequest
        {
            PlayFabId = account.MasterPlayerAccount.Id
        };
        // Result:
        //      Userinfo:
        //          CustomIdInfo:
        //              CustomId
        //          TitleInfo:
        //              TitlePlayerAccount:
        //                  ID
        var response = await _playFabAdminInstanceApi.GetUserAccountInfoAsync(request);

        account.MasterPlayerAccount.CustomId = response.Result.UserInfo.CustomIdInfo.CustomId;

        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = response.Result.UserInfo.TitleInfo.TitlePlayerAccount.Id,
            TitleId = _playFabApiSettings.TitleId
        };

        account.AddTitlePlayerAccount(titlePlayerAccount);

        return account;
    }

    /// <summary>
    /// This gets the Entity Token for a given customId, if there is no user Account with this customId a new one will be created.
    /// </summary>
    /// <param name="customId"></param>
    /// <returns>The Entity Token</returns>
    public async Task<string> GetEntityToken(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetTitleData = true,
                GetUserData = true,
                GetUserAccountInfo = true,
            }
        };

        var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        return response.Result.AuthenticationContext.EntityToken;
    }
}
