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
        }

        public void AssignMasterAccount(MasterPlayerAccountEntity account)
        {
            var proxy = new MasterPlayerAccountProxy(account);

            if (PlayerAccount.MasterAccount != null)
            {
                proxy.RemoveTitlePlayerAccount(PlayerAccount);
            }

            PlayerAccount.MasterAccount = account;
            proxy.AddTitlePlayerAccount(PlayerAccount);
        }
    }
}
