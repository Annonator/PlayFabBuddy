using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;
using PlayFabBuddy.Lib.UseCases;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class DeletePlayersUseCase : IUseCase<bool>
{
    private readonly List<MasterPlayerAccountAggregate> _accountList;
    private readonly IRepository<MasterPlayerAccountAggregate> _repository;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;

    public DeletePlayersUseCase(IPlayerAccountAdapter playerAccountAdapter, IRepository<MasterPlayerAccountAggregate> repo)
    {
        _repository = repo;
        _accountList = new List<MasterPlayerAccountAggregate>();
        _playerAccountAdapter = playerAccountAdapter;
    }

    public DeletePlayersUseCase(IPlayerAccountAdapter playFabAdapter, IRepository<MasterPlayerAccountAggregate> repo, List<MasterPlayerAccountAggregate> accounts)
    {
        _repository = repo;
        _accountList = accounts;
        _playerAccountAdapter = playFabAdapter;
    }

    public async Task<bool> ExecuteAsync()
    {
        _accountList.Clear();
        var updatedAccountList = await _repository.Get();
        _accountList.AddRange(updatedAccountList);

        var deleteList = new List<Task>();
        foreach (var account in _accountList)
        {
            deleteList.Add(_playerAccountAdapter.Delete(account));
        }

        await Task.WhenAll(deleteList);

        return true;
    }
}