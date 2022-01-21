using PlayFab;
using PlayFab.ClientModels;
using PlayFabBuddy.Lib.Admin;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Commands.Player;

public class LoginPlayerCommand : ICommand<MasterPlayerAccountEntity>
{
    public LoginPlayerCommand(TitlePlayerAccountEntity player, PlayFabConfig config)
    {
        this.player = player;
        this.config = config;
    }

    public TitlePlayerAccountEntity player { private get; init; }
    public PlayFabConfig config { private get; init; }

    Task<MasterPlayerAccountEntity> ICommand<MasterPlayerAccountEntity>.ExecuteAsync()
    {
        throw new NotImplementedException();
    }

    public void ExecuteAsync()
    {
        var newPlayerModel = new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = new Guid().ToString()
        };

        var request = PlayFabClientAPI.LoginWithCustomIDAsync(newPlayerModel);
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}