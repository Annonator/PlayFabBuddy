using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Aggregate;

public class PlayerAccountAggregate
{
    /// <summary>
    /// Field to store the reference to the MasterPlayerAccount
    /// </summary>
    public MasterPlayerAccountEntity MasterPlayerAccount { get; private set; }

    /// <summary>
    /// Creates a new Aggregate based on a pre defined MasterPlayerAccountEntity
    /// </summary>
    /// <param name="masterPlayerAccount"></param>
    public PlayerAccountAggregate(MasterPlayerAccountEntity masterPlayerAccount)
    {
        MasterPlayerAccount = masterPlayerAccount;

        if (MasterPlayerAccount.PlayerAccounts == null)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }
    }

    /// <summary>
    /// Creates a new MasterPlayerAccountEntity and TitlePlayerAccountEntity based on their ID
    /// </summary>
    /// <param name="masterAccountId"></param>
    /// <param name="playerAccountId"></param>
    public PlayerAccountAggregate(string masterAccountId, string playerAccountId)
    {
        var titleAccount = new TitlePlayerAccountEntity
        {
            Id = playerAccountId
        };
        MasterPlayerAccount = new MasterPlayerAccountEntity
        {
            Id = masterAccountId,
            PlayerAccounts = new List<TitlePlayerAccountEntity>() { titleAccount }
        };
    }

    /// <summary>
    /// Removes a given TitlePlayerAccountEntity from the MasterAccountEntity
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public bool RemoveTitlePlayerAccount(TitlePlayerAccountEntity account)
    {
        if (MasterPlayerAccount.PlayerAccounts == null)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }

        MasterPlayerAccount.PlayerAccounts[MasterPlayerAccount.PlayerAccounts.IndexOf(account)].MasterAccount = null;

        return MasterPlayerAccount.PlayerAccounts.Remove(account);
    }

    /// <summary>
    /// Adds a TitlePlayerAccountEntity to the MasterPlayerAccountEnity handled by this Aggregate by removing existing references to previouse connected master accounts and add it to this.
    /// </summary>
    /// <param name="account"></param>
    public void AddTitlePlayerAccount(TitlePlayerAccountEntity account)
    {
        if (MasterPlayerAccount.PlayerAccounts == null)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }

        if (account.MasterAccount != null && account.MasterAccount != MasterPlayerAccount)
        {
            var oldMasterAccount = account.MasterAccount;
            var aggregate = new PlayerAccountAggregate(oldMasterAccount);
            aggregate.RemoveTitlePlayerAccount(account);
        }

        MasterPlayerAccount.PlayerAccounts.Add(account);
    }

    /// <summary>
    /// Remove all references of TitlePlayerAccounts from this Aggregate and MasterPlayerAccountEntity
    /// </summary>
    public void RemoveAllTitlePlayerAccounts()
    {
        MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
    }
}
