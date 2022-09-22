using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFabBuddy.Infrastructure.Exceptions;
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
            throw new Exception(
                $"Master PlayerAccount ID \"{account.MasterPlayerAccount.Id}\" has more than one Title");
        }

        var request = new DeleteMasterPlayerAccountRequest { PlayFabId = account.MasterPlayerAccount.Id };

        await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
    }

    public async Task<MasterPlayerAccountAggregate> LoginWithCustomId(string customId)
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = customId,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true, GetTitleData = true, GetUserData = true, GetUserAccountInfo = true
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

        if (loginResult.Error != null && loginResult.Error.HttpStatus == "Forbidden")
        {
            throw new AddPlayerForbiddenException(customId);
        }

        var masterPlayerAccount = new MasterPlayerAccountEntity {
            Id = loginResult.Result.AuthenticationContext.PlayFabId, CustomId = customId
        };
        var titlePlayerAccount = new TitlePlayerAccountEntity {
            Id = loginResult.Result.AuthenticationContext.EntityId, TitleId = _playFabApiSettings.TitleId
        };
        var aggregate = new MasterPlayerAccountAggregate(masterPlayerAccount);
        aggregate.AddTitlePlayerAccount(titlePlayerAccount);

        return aggregate;
    }

    public async Task<MasterPlayerAccountAggregate> GetTitleAccountsAndCustomId(MasterPlayerAccountAggregate account)
    {
        var request = new LookupUserAccountInfoRequest { PlayFabId = account.MasterPlayerAccount.Id };
        // Result:
        //      Userinfo:
        //          CustomIdInfo:
        //              CustomId
        //          TitleInfo:
        //              TitlePlayerAccount:
        //                  ID
        var response = await _playFabAdminInstanceApi.GetUserAccountInfoAsync(request);

        account.MasterPlayerAccount.CustomId = response.Result.UserInfo.CustomIdInfo.CustomId;

        var titlePlayerAccount = new TitlePlayerAccountEntity {
            Id = response.Result.UserInfo.TitleInfo.TitlePlayerAccount.Id, TitleId = _playFabApiSettings.TitleId, IsBanned = response.Result.UserInfo.TitleInfo.isBanned
        };

        account.AddTitlePlayerAccount(titlePlayerAccount);

        return account;
    }

    public async Task<bool> BanPlayerByTitlePlayerAccount(List<MasterPlayerAccountAggregate> entityList, string reason,
        uint? banDurationInHours = null, bool banByIp = false)
    {
        var banRequests = new List<BanRequest>();

        foreach (var entity in entityList)
        {
            banRequests.Add(new BanRequest {
                Reason = reason,
                DurationInHours = banDurationInHours,
                IPAddress = banByIp ? entity.MasterPlayerAccount.LastKnownIp : null,
                PlayFabId = entity.MasterPlayerAccount.Id
            });
        }

        var request = new BanUsersRequest { Bans = banRequests };

        var response = await _playFabAdminInstanceApi.BanUsersAsync(request);

        // TO make it simple for now, check if we have banned the same amount of players as we requested. Can be optimized in the future.
        if (response.Result.BanData.Count == entityList.Count)
        {
            return true;
        }

        return false;
    }
}