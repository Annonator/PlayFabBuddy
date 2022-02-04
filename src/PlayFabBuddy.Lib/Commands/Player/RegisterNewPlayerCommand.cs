using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.Commands.Player;

public class RegisterNewPlayerCommand : ICommand<MasterPlayerAccountEntity>
{
    private readonly IPlayerAccountAdapter<MasterPlayerAccountEntity> _playFabAdapter;

    public RegisterNewPlayerCommand(IPlayerAccountAdapter<MasterPlayerAccountEntity> playFabAdapter)
    {
        _playFabAdapter = playFabAdapter;
    }

    public async Task<MasterPlayerAccountEntity> ExecuteAsync()
    {
        var customId = Guid.NewGuid().ToString();
        var loginResult = await _playFabAdapter.LoginWithCustomId(customId);

        return loginResult;
    }
}