using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Adapter.Accounts
{
    public class MasterPlayerAccountAdapter
    {
        public MasterPlayerAccountEntity MainAccount { get; private set; }

        public MasterPlayerAccountAdapter(MasterPlayerAccountEntity account)
        {
            MainAccount = account;
        }

        public MasterPlayerAccountAdapter(string id)
        {
            MainAccount = new MasterPlayerAccountEntity
            {
                Id = id
            };
        }

        public MasterPlayerAccountAdapter(string id, TitlePlayerAccountEntity playerAccount)
        {
            MainAccount = new MasterPlayerAccountEntity
            {
                Id = id,
                PlayerAccounts = new List<TitlePlayerAccountEntity> { playerAccount }
            };
        }

        public MasterPlayerAccountAdapter(string id, List<TitlePlayerAccountEntity> playerAccountList)
        {
            MainAccount = new MasterPlayerAccountEntity
            {
                Id = id,
                PlayerAccounts = playerAccountList
            };
        }

        public bool RemoveTitlePlayerAccount(TitlePlayerAccountEntity account)
        {
            if (MainAccount.PlayerAccounts == null)
            {
                throw new ArgumentNullException("Can't remove Account from empty account list!");
            }

            return MainAccount.PlayerAccounts.Remove(account);
        }

        public void AddTitlePlayerAccount(TitlePlayerAccountEntity account)
        {
            if (MainAccount.PlayerAccounts == null)
            {
                MainAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
            }

            MainAccount.PlayerAccounts.Add(account);
        }

        public void RemoveAllTitlePlayerAccounts()
        {
            if (MainAccount.PlayerAccounts != null)
            {
                foreach (var account in MainAccount.PlayerAccounts)
                {
                    var proxy = new TitlePlayerAccountAdapter(account);

                    proxy.RemoveMasterAccount();
                }

                MainAccount.PlayerAccounts.Clear();
            }
        }
    }
}
