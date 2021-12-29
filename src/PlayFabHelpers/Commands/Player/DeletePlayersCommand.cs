using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using PlayFabBuddy.PlayFabHelpers.Proxy.Accounts;
using PlayFabBuddy.PlayFabHelpers.Util.IoC;
using PlayFabBuddy.PlayFabHelpers.Util.Repository;

namespace PlayFabBuddy.PlayFabHelpers.Commands.Player
{
    public class DeletePlayersCommand : ICommand<bool>
    {
        private IRepository<MasterPlayerAccountEntity> Repository;
        private List<MasterPlayerAccountEntity> AccountList;

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
}
