using System;
using System.Collections.Generic;
using PlayFabBuddy.Lib.Entities.Accounts;
using Xunit;

namespace PlayFabBuddy.Lib.Aggregate.Tests;
public class PlayerAccountAggregateTests
{
    [Fact()]
    public void PlayerAccountAggregateTest()
    {
        var mainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var aggregate = new PlayerAccountAggregate(mainAccount);

        Assert.Equal(mainAccount, aggregate.MasterPlayerAccount);

        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        mainAccount.PlayerAccounts = new List<TitlePlayerAccountEntity>() { titlePlayerAccount };

        var newAggregate = new PlayerAccountAggregate(mainAccount);

        Assert.NotNull(newAggregate.MasterPlayerAccount.PlayerAccounts);
        Assert.Equal(mainAccount, newAggregate.MasterPlayerAccount);
        Assert.Contains(titlePlayerAccount, newAggregate.MasterPlayerAccount.PlayerAccounts);
    }

    [Fact()]
    public void PlayerAccountAggregateTestByIds()
    {
        var mainAccountGuid = Guid.NewGuid().ToString();
        var titleAccountGuid = Guid.NewGuid().ToString();

        var aggregate = new PlayerAccountAggregate(mainAccountGuid, titleAccountGuid);

        Assert.NotNull(aggregate.MasterPlayerAccount);
        Assert.Equal(mainAccountGuid, aggregate.MasterPlayerAccount.Id);
    }

    [Fact()]
    public void RemoveTitlePlayerAccountTest()
    {

        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var mainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString(),
            PlayerAccounts = new List<TitlePlayerAccountEntity>() { titlePlayerAccount }
        };

        var aggregate = new PlayerAccountAggregate(mainAccount);

        aggregate.RemoveTitlePlayerAccount(titlePlayerAccount);

        Assert.Empty(aggregate.MasterPlayerAccount.PlayerAccounts);

    }

    [Fact()]
    public void RemoveTitlePlayerAccountFromEmptyTest()
    {

        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var mainAccountEmptyTitleAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var aggregate2 = new PlayerAccountAggregate(mainAccountEmptyTitleAccount);

        aggregate2.RemoveTitlePlayerAccount(titlePlayerAccount);

        Assert.False(aggregate2.RemoveTitlePlayerAccount(titlePlayerAccount));
    }

    [Fact()]
    public void RemoveAllTitlePlayerAccountsTest()
    {
        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var titlePlayerAccount2 = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var mainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString(),
            PlayerAccounts = new List<TitlePlayerAccountEntity>() { titlePlayerAccount, titlePlayerAccount2 }
        };

        var aggregate = new PlayerAccountAggregate(mainAccount);

        aggregate.RemoveAllTitlePlayerAccounts();

        Assert.Empty(aggregate.MasterPlayerAccount.PlayerAccounts);
    }

    [Fact()]
    public void AddTitlePlayerAccountTest()
    {
        var mainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var aggregate = new PlayerAccountAggregate(mainAccount);

        var titlePlayerAccount = new TitlePlayerAccountEntity { Id = Guid.NewGuid().ToString() };

        aggregate.AddTitlePlayerAccount(titlePlayerAccount);

        Assert.Contains(titlePlayerAccount, aggregate.MasterPlayerAccount.PlayerAccounts);
    }

    [Fact()]
    public void AddTitlePlayerAccountWithExistingMainAccountTest()
    {
        var mainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };
        var oldMainAccount = new MasterPlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString()
        };

        var aggregate = new PlayerAccountAggregate(mainAccount);

        var titlePlayerAccount = new TitlePlayerAccountEntity
        {
            Id = Guid.NewGuid().ToString(),
            MasterAccount = oldMainAccount

        };

        aggregate.AddTitlePlayerAccount(titlePlayerAccount);

        Assert.Contains(titlePlayerAccount, aggregate.MasterPlayerAccount.PlayerAccounts);


    }
}
