using PlayFab;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using PlayFabBuddy.PlayFabHelpers.Util.Repository;

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
            var mainAccount = new MasterPlayerAccountEntity(loginResult.Result.AuthenticationContext.PlayFabId);
            var titleAccount = new TitlePlayerAccountEntity(loginResult.Result.AuthenticationContext.EntityId, mainAccount);

            return mainAccount;
        }

    }
}
