using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class PlayerAccountAdapter : IPlayerAccountAdapter
{
    public async Task Delete(MasterPlayerAccountEntity account)
    {
        var playedTitleList = await GetPlayedTitleList(account);
        if (playedTitleList.MoreThanOne())
        {
            throw new Exception($"Master PlayerAccount ID \"{account.Id}\" has more than one Title");
        }

        var request = new DeleteMasterPlayerAccountRequest
        {
            PlayFabId = account.Id
        };

        await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
    }

    public async Task<PlayedTitlesListEntity> GetPlayedTitleList(MasterPlayerAccountEntity account)
    {
        var request = new GetPlayedTitleListRequest { PlayFabId = account.Id };
        var response = await PlayFabAdminAPI.GetPlayedTitleListAsync(request);

        var titleIds = response.Result is not null ? response.Result.TitleIds.ToArray() : Array.Empty<string>();

        return new PlayedTitlesListEntity(titleIds);
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

        var aggregate = new PlayerAccountAggregate(loginResult.Result.AuthenticationContext.PlayFabId, loginResult.Result.AuthenticationContext.EntityId);

        return aggregate.MasterPlayerAccount;
    }
}
