using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;

namespace PlayFabBuddy.PlayFabHelpers.Adapter.Accounts
{
    public class TitlePlayerAccountAdapter
    {
        public TitlePlayerAccountEntity PlayerAccount { get; private set; }

        public TitlePlayerAccountAdapter(string id)
        {
            PlayerAccount = new TitlePlayerAccountEntity
            {
                Id = id
            };
        }
        public TitlePlayerAccountAdapter(TitlePlayerAccountEntity account)
        {
            PlayerAccount = account;
        }

        public TitlePlayerAccountAdapter(string id, MasterPlayerAccountEntity account)
        {
            PlayerAccount = new TitlePlayerAccountEntity
            {
                Id = id,
            };

            AssignMasterAccount(account);
        }

        public void AssignMasterAccount(MasterPlayerAccountEntity account)
        {
            MasterPlayerAccountAdapter proxy;

            if (PlayerAccount.MasterAccount != null)
            {
                proxy = new MasterPlayerAccountAdapter(PlayerAccount.MasterAccount);
                proxy.RemoveTitlePlayerAccount(PlayerAccount);
            }

            proxy = new MasterPlayerAccountAdapter(account);
            PlayerAccount.MasterAccount = account;
            proxy.AddTitlePlayerAccount(PlayerAccount);
        }

        public void RemoveMasterAccount()
        {
            PlayerAccount.MasterAccount = null;
        }
    }
}
