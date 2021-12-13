using PlayFabBuddy.PlayFabHelpers.Admin;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class LoginPlayerCommand : ICommand<MasterPlayerAccountEntity>
    {
        public TitlePlayerAccountEntity player { private get; init; }
        public PlayFabConfig config {private  get; init; }

        public LoginPlayerCommand(TitlePlayerAccountEntity player, PlayFabConfig config)
        {
            this.player = player;
            this.config = config;
        }

        public void ExecuteAsync()
        {

            var newPlayerModel = new PlayFab.ClientModels.LoginWithCustomIDRequest
            {
                CreateAccount = true,
                CustomId = new Guid().ToString()
            };

            var request = PlayFab.PlayFabClientAPI.LoginWithCustomIDAsync(newPlayerModel);



        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        Task<MasterPlayerAccountEntity> ICommand<MasterPlayerAccountEntity>.ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
