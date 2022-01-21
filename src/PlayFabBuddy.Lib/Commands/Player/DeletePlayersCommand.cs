using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.IoC;
using PlayFabBuddy.Lib.Util.Repository;

namespace PlayFabBuddy.Lib.Commands.Player;

public class DeletePlayersCommand : ICommand<bool>
{
    private readonly List<MasterPlayerAccountEntity> AccountList;
    private readonly IRepository<MasterPlayerAccountEntity> Repository;

    /**
         * Default behavior when creating this command is to load players from repository.
         */
    public DeletePlayersCommand()
    {
        Repository = DependencyInjection.Instance.Resolve<IRepository<MasterPlayerAccountEntity>>();
        AccountList = Repository.Get();
    }

    public DeletePlayersCommand(List<MasterPlayerAccountEntity> accountList)
    {
        Repository = DependencyInjection.Instance.Resolve<IRepository<MasterPlayerAccountEntity>>();
        AccountList = accountList;
    }

    public DeletePlayersCommand(IRepository<MasterPlayerAccountEntity> repo)
    {
        Repository = repo;
        AccountList = Repository.Get();
    }

    public DeletePlayersCommand(IRepository<MasterPlayerAccountEntity> repo, List<MasterPlayerAccountEntity> accounts)
    {
        Repository = repo;
        AccountList = accounts;
    }

    public Task<bool> ExecuteAsync()
    {
        foreach (var account in AccountList)
        {
            var request = new DeleteMasterPlayerAccountRequest
            {
                PlayFabId = account.Id
            };

            PlayFabAdminAPI.DeleteMasterPlayerAccountAsync(request);
        }

        Repository.Save(new List<MasterPlayerAccountEntity>());

        return Task.FromResult(true);
    }
}