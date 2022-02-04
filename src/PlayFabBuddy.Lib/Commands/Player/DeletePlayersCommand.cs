using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Lib.Commands.Player;

public class DeletePlayersCommand : ICommand<bool>
{
    private readonly List<MasterPlayerAccountEntity> _accountList;
    private readonly IRepository<MasterPlayerAccountEntity> _repository;
    private readonly IPlayerAccountAdapter<MasterPlayerAccountEntity> _playFabAdpadter;

    public DeletePlayersCommand(IPlayerAccountAdapter<MasterPlayerAccountEntity> playFabAdapter, IRepository<MasterPlayerAccountEntity> repo)
    {
        _repository = repo;
        _accountList = _repository.Get();
        _playFabAdpadter = playFabAdapter;
    }

    public DeletePlayersCommand(IPlayerAccountAdapter<MasterPlayerAccountEntity> playFabAdapter, IRepository<MasterPlayerAccountEntity> repo, List<MasterPlayerAccountEntity> accounts)
    {
        _repository = repo;
        _accountList = accounts;
        _playFabAdpadter = playFabAdapter;
    }

    public async Task<bool> ExecuteAsync()
    {
        var deleteList = new List<Task>();
        foreach (var account in _accountList)
        {
            deleteList.Add(_playFabAdpadter.Delete(account));
        }

        await Task.WhenAll(deleteList);

        return true;
    }
}