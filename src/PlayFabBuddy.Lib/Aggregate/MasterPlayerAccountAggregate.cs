using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Aggregate;

public class MasterPlayerAccountAggregate
{
    /// <summary>
    /// Field to store the reference to the MasterPlayerAccount
    /// </summary>
    public MasterPlayerAccountEntity MasterPlayerAccount { get; }

    /// <summary>
    /// Creates a new Aggregate based on a pre defined MasterPlayerAccountEntity
    /// </summary>
    /// <param name="masterPlayerAccount"></param>
    public MasterPlayerAccountAggregate(MasterPlayerAccountEntity masterPlayerAccount)
    {
        MasterPlayerAccount = masterPlayerAccount;

        if (MasterPlayerAccount.PlayerAccounts == null)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }
    }

    /// <summary>
    /// Creates a new Aggregate based on MasterPlayerAccount ID
    /// </summary>
    /// <param name="masterPlayerAccountId"></param>
    public MasterPlayerAccountAggregate(string masterPlayerAccountId)
    {
        MasterPlayerAccount = new MasterPlayerAccountEntity
        {
            Id = masterPlayerAccountId,
            PlayerAccounts = new List<TitlePlayerAccountEntity>()
        };
    }

    /// <summary>
    /// Creates a new MasterPlayerAccountEntity and TitlePlayerAccountEntity based on their ID
    /// </summary>
    /// <param name="masterPlayerAccountId"></param>
    /// <param name="titlePlayerAccountId"></param>
    public MasterPlayerAccountAggregate(string masterPlayerAccountId, string titlePlayerAccountId)
    {
        var titleAccount = new TitlePlayerAccountEntity
        {
            Id = titlePlayerAccountId
        };
        MasterPlayerAccount = new MasterPlayerAccountEntity
        {
            Id = masterPlayerAccountId,
            PlayerAccounts = new List<TitlePlayerAccountEntity> { titleAccount }
        };
    }

    /// <summary>
    /// Removes a given TitlePlayerAccountEntity from the MasterAccountEntity
    /// </summary>
    /// <param name="account"></param>
    /// <returns>Returns false if MasterPlayerAccount was not found, does not belong to this MasterPlayerAccount or couldn't be deleted</returns>
    public bool RemoveTitlePlayerAccount(TitlePlayerAccountEntity account)
    {
        if (MasterPlayerAccount.PlayerAccounts == null || MasterPlayerAccount.PlayerAccounts.Count == 0)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
            return false;
        }

        MasterPlayerAccount.PlayerAccounts[MasterPlayerAccount.PlayerAccounts.IndexOf(account)].MasterAccount = null;

        return MasterPlayerAccount.PlayerAccounts.Remove(account);
    }

    /// <summary>
    /// Adds a TitlePlayerAccountEntity to the MasterPlayerAccountEntity handled by this Aggregate by removing existing references to previously connected master accounts and add it to this.
    /// </summary>
    /// <param name="account"></param>
    public void AddTitlePlayerAccount(TitlePlayerAccountEntity account)
    {
        if (MasterPlayerAccount.PlayerAccounts == null || MasterPlayerAccount.PlayerAccounts.Count == 0)
        {
            MasterPlayerAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }

        if (account.MasterAccount != null && account.MasterAccount != MasterPlayerAccount)
        {
            var oldMasterAccount = account.MasterAccount;
            var aggregate = new MasterPlayerAccountAggregate(oldMasterAccount);
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
