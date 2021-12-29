using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using System;
using System.Collections.Generic;

namespace PlayFabBuddy.PlayFabHelpers.Proxy.Accounts.Tests
{
    [TestClass()]
    public class MasterPlayerAccountProxyTests
    {
        [TestMethod()]
        public void MasterPlayerAccountEntityTestEmptyConstructor()
        {
            var guid = Guid.NewGuid().ToString();

            var account = new MasterPlayerAccountProxy(guid);

            Assert.AreEqual(guid, account.MainAccount.Id);
            Assert.IsNull(account.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestMainConstructor()
        {
            var guid = Guid.NewGuid().ToString();

            var filledList = new List<TitlePlayerAccountEntity> { new TitlePlayerAccountProxy(guid).PlayerAccount };

            var account2 = new MasterPlayerAccountProxy(guid, filledList);

            Assert.AreEqual(filledList, account2.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestConstructor2()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountProxy(guid).PlayerAccount;

            var account = new MasterPlayerAccountProxy(guid, entity).MainAccount;

            Assert.IsNotNull(account.PlayerAccounts);
            Assert.IsTrue(account.PlayerAccounts.Contains(entity));
            Assert.IsTrue(account.PlayerAccounts.Count == 1);
        }

        [TestMethod()]
        public void RemoveTitlePlayerAccountTest()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;

            var filledList = new List<TitlePlayerAccountEntity> { entity };

            var account = new MasterPlayerAccountProxy(guid, filledList);

            account.RemoveTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void AddTitlePlayerAccountTest()
        {
            var guid = Guid.NewGuid().ToString();

            var account = new MasterPlayerAccountProxy(guid);

            var entity = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;

            account.AddTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsTrue(account.MainAccount.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void RemoveAllTitlePlayerAccountsTest()
        {
            var guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;
            var entity2 = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;

            var filledList = new List<TitlePlayerAccountEntity> { entity, entity2 };

            var account = new MasterPlayerAccountProxy(guid, filledList);

            account.RemoveAllTitlePlayerAccounts();

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity));
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity2));
        }
    }
}