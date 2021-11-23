using PlayFab;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class RegisterNewPlayerCommand : ICommand
    {
        public async Task ExecuteAsync()
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
        }

    }
}
