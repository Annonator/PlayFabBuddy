using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class LoginPlayerUseCase : IUseCase<bool>
{
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public LoginPlayerUseCase(IRepository<MasterPlayerAccountAggregate> repo, IPlayerAccountAdapter playerAccountAdapter)
    {
        _repository = repo;
        _playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<bool> ExecuteAsync()
    {
        var allMasterPlayerAccounts = await _repository.Get();

        foreach (var masterPlayerAccount in allMasterPlayerAccounts)
        {
            if (masterPlayerAccount.MasterPlayerAccount.CustomId != null)
            {
                await _playerAccountAdapter.LoginWithCustomId(masterPlayerAccount.MasterPlayerAccount.CustomId);
            }
        }

        return true;
    }
}
