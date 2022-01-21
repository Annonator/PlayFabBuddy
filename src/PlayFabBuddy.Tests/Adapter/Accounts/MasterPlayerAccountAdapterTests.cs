using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFabBuddy.Lib.Adapter.Accounts;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Tests.Adapter.Accounts
{
    [TestClass()]
    public class MasterPlayerAccountAdapterTests
    {
        [TestMethod()]
        public void MasterPlayerAccountEntityTestEmptyConstructor()
        {
            var guid = Guid.NewGuid().ToString();

            var account = new MasterPlayerAccountAdapter(guid);

            Assert.AreEqual(guid, account.MainAccount.Id);
            Assert.IsNull(account.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestMainConstructor()
        {
            var guid = Guid.NewGuid().ToString();

            var filledList = new List<TitlePlayerAccountEntity> { new TitlePlayerAccountAdapter(guid).PlayerAccount };

            var account2 = new MasterPlayerAccountAdapter(guid, filledList);

            Assert.AreEqual(filledList, account2.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestConstructor2()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountAdapter(guid).PlayerAccount;

            var account = new MasterPlayerAccountAdapter(guid, entity).MainAccount;

            Assert.IsNotNull(account.PlayerAccounts);
            Assert.IsTrue(account.PlayerAccounts.Contains(entity));
            Assert.IsTrue(account.PlayerAccounts.Count == 1);
        }

        [TestMethod()]
        public void RemoveTitlePlayerAccountTest()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountAdapter(Guid.NewGuid().ToString()).PlayerAccount;

            var filledList = new List<TitlePlayerAccountEntity> { entity };

            var account = new MasterPlayerAccountAdapter(guid, filledList);

            account.RemoveTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void AddTitlePlayerAccountTest()
        {
            var guid = Guid.NewGuid().ToString();

            var account = new MasterPlayerAccountAdapter(guid);

            var entity = new TitlePlayerAccountAdapter(Guid.NewGuid().ToString()).PlayerAccount;

            account.AddTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsTrue(account.MainAccount.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void RemoveAllTitlePlayerAccountsTest()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountAdapter(Guid.NewGuid().ToString()).PlayerAccount;
            var entity2 = new TitlePlayerAccountAdapter(Guid.NewGuid().ToString()).PlayerAccount;

            var filledList = new List<TitlePlayerAccountEntity> { entity, entity2 };

            var account = new MasterPlayerAccountAdapter(guid, filledList);

            account.RemoveAllTitlePlayerAccounts();

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity));
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity2));
        }
    }
}