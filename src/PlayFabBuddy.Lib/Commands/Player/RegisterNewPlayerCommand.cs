using PlayFab;
using PlayFabBuddy.Lib.Adapter.Accounts;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Commands.Player
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
            var mainAccount = new MasterPlayerAccountAdapter(loginResult.Result.AuthenticationContext.PlayFabId);
            var titleAccount = new TitlePlayerAccountAdapter(loginResult.Result.AuthenticationContext.EntityId, mainAccount.MainAccount);

            return mainAccount.MainAccount;
        }

    }
}
