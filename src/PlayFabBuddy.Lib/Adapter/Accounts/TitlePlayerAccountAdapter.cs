using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Adapter.Accounts;

public class TitlePlayerAccountAdapter
{
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
            Id = id
        };

        AssignMasterAccount(account);
    }

    public TitlePlayerAccountEntity PlayerAccount { get; }

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