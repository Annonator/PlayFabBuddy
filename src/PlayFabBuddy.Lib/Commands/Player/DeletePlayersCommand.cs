using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.Repository;

namespace PlayFabBuddy.Lib.Commands.Player;

public class DeletePlayersCommand : ICommand<bool>
{
    private readonly List<MasterPlayerAccountEntity> _accountList;
    private readonly IRepository<MasterPlayerAccountEntity> _repository;

    public DeletePlayersCommand(IRepository<MasterPlayerAccountEntity> repo)
    {
        _repository = repo;
        _accountList = _repository.Get();
    }

    public DeletePlayersCommand(IRepository<MasterPlayerAccountEntity> repo, List<MasterPlayerAccountEntity> accounts)
    {
        _repository = repo;
        _accountList = accounts;
    }

    public async Task<bool> ExecuteAsync()
    {
        foreach (var account in _accountList)
        {
            var request = new DeleteMasterPlayerAccountRequest
            {
                PlayFabId = account.Id
            };

            await PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
        }

        return true;
    }
}