using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Proxy.Accounts
{
    public class MasterPlayerAccountProxy
    {
        public MasterPlayerAccountEntity MainAccount { get; private set; }

        public MasterPlayerAccountProxy(MasterPlayerAccountEntity account)
        {
            MainAccount = account;
        }

        public MasterPlayerAccountProxy(string id)
        {
            MainAccount = new MasterPlayerAccountEntity
            {
                Id = id
            };
        }

        public MasterPlayerAccountProxy(string id, TitlePlayerAccountEntity playerAccount)
        {
            MainAccount = new MasterPlayerAccountEntity
            {
                Id = id,
                PlayerAccounts = new List<TitlePlayerAccountEntity> { playerAccount }
            };
        }

        public MasterPlayerAccountProxy(string id, List<TitlePlayerAccountEntity> playerAccountList)
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
    }
}
