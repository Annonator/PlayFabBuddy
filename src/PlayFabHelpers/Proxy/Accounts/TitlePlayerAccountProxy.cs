using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Proxy.Accounts
{
    public class TitlePlayerAccountProxy
    {
        public TitlePlayerAccountEntity PlayerAccount { get; private set; }

        public TitlePlayerAccountProxy(string id)
        {
            PlayerAccount = new TitlePlayerAccountEntity
            {
                Id = id
            };
        }

        public TitlePlayerAccountProxy(string id, MasterPlayerAccountEntity account)
        {
            PlayerAccount = new TitlePlayerAccountEntity
            {
                Id = id,
            };

            AssignMasterAccount(account);
        }

        public void AssignMasterAccount(MasterPlayerAccountEntity account)
        {
            MasterPlayerAccountProxy proxy;

            if (PlayerAccount.MasterAccount != null)
            {
                proxy = new MasterPlayerAccountProxy(PlayerAccount.MasterAccount);
                proxy.RemoveTitlePlayerAccount(PlayerAccount);
            }

            proxy = new MasterPlayerAccountProxy(account);
            PlayerAccount.MasterAccount = account;
            proxy.AddTitlePlayerAccount(PlayerAccount);
        }
    }
}
