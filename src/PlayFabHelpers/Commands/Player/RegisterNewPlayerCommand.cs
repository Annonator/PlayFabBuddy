using PlayFab;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using PlayFabBuddy.PlayFabHelpers.Proxy.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class RegisterNewPlayerCommand : ICommand<MasterPlayerAccountEntity>
    {
        public async Task<MasterPlayerAccountEntity> ExecuteAsync()
        {
            var customId = Guid.NewGuid().ToString();

            var request = new PlayFab.ClientModels.LoginWithCustomIDRequest
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

            //TODO: What happens if i ahve multipke tile accounts under the main account?
            var mainAccount = new MasterPlayerAccountProxy(loginResult.Result.AuthenticationContext.PlayFabId);
            var titleAccount = new TitlePlayerAccountProxy(loginResult.Result.AuthenticationContext.EntityId, mainAccount.MainAccount);

            return mainAccount.MainAccount;
        }

    }
}
