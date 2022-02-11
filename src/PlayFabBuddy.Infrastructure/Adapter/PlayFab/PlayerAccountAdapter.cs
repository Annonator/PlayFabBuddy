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

    public PlayerAccountAdapter(PlayFabAdminInstanceAPI adminInstanceApi)
    {
        _playFabAdminInstanceApi = adminInstanceApi;
    }

    public async Task Delete(MasterPlayerAccountAggregate account)
    {
        var playedTitleList = await GetPlayedTitleList(account);
        if (playedTitleList.MoreThanOne())
        {
            throw new Exception($"Master PlayerAccount ID \"{account.MasterPlayerAccount.Id}\" has more than one Title");
        }

        var request = new DeleteMasterPlayerAccountRequest
        {
            PlayFabId = account.MasterPlayerAccount.Id
        };

        await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
    }

    public async Task<PlayedTitlesListEntity> GetPlayedTitleList(MasterPlayerAccountAggregate account)
    {
        var request = new GetPlayedTitleListRequest { PlayFabId = account.MasterPlayerAccount.Id };
        var response = await PlayFabAdminAPI.GetPlayedTitleListAsync(request);

        var titleIds = response.Result is not null ? response.Result.TitleIds.ToArray() : Array.Empty<string>();

        return new PlayedTitlesListEntity(titleIds);
    }

    public async Task<MasterPlayerAccountAggregate> LoginWithCustomId(string customId)
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

        var masterPlayerAccount = new MasterPlayerAccountEntity
        {
            Id = loginResult.Result.AuthenticationContext.PlayFabId,
            CustomId = customId
        };
        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = loginResult.Result.AuthenticationContext.EntityId
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
            Id = response.Result.UserInfo.TitleInfo.TitlePlayerAccount.Id
        };

        account.AddTitlePlayerAccount(titlePlayerAccount);

        return account;
    }
}
