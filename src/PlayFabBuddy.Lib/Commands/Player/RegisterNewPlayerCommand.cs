using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.Commands.Player;

public class RegisterNewPlayerCommand : ICommand<MasterPlayerAccountEntity>
{
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public RegisterNewPlayerCommand(IPlayerAccountAdapter playerAccountAdapter)
    {
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<MasterPlayerAccountEntity> ExecuteAsync()
    {
        var customId = Guid.NewGuid().ToString();
        var loginResult = await _playerAccountAdapter.LoginWithCustomId(customId);

        return loginResult;
    }
}