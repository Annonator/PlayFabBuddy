using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class RegisterNewPlayerUseCase : IUseCase<MasterPlayerAccountAggregate>
{
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public RegisterNewPlayerUseCase(IPlayerAccountAdapter playerAccountAdapter)
    {
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<MasterPlayerAccountAggregate> ExecuteAsync()
    {
        var customId = Guid.NewGuid().ToString();
        var loginResult = await _playerAccountAdapter.LoginWithCustomId(customId);

        return loginResult;
    }
}