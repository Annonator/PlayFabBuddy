using PlayFab;
using System;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class RegisterNewPlayerCommand : ICommand
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAsync()
        {

            var customId = Guid.NewGuid().ToString();

            var request = new PlayFab.ClientModels.LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true
            };

            var loginTask = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            Console.WriteLine("TEst");
        }

    }
}
