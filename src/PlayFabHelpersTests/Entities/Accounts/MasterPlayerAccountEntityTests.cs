using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFabBuddy.PlayFabHelpers.Proxy.Accounts;
using System;
using System.Collections.Generic;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts.Tests
{
    [TestClass()]
    public class MasterPlayerAccountEntityTests
    {
        [TestMethod()]
        public void MasterPlayerAccountEntityTestEmptyConstructor()
        {
            string guid = Guid.NewGuid().ToString();

            var emptyList = new List<TitlePlayerAccountEntity>();

            var account = new MasterPlayerAccountProxy(guid);

            Assert.AreEqual(guid, account.MainAccount.Id);
            Assert.IsNull(account.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestMainConstructor()
        {
            string guid = Guid.NewGuid().ToString();

            var filledList = new List<TitlePlayerAccountEntity>();
            filledList.Add(new TitlePlayerAccountProxy(guid).PlayerAccount);

            var account2 = new MasterPlayerAccountProxy(guid, filledList);

            Assert.AreEqual(filledList, account2.MainAccount.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestConstructor2()
        {
            string guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountProxy(guid).PlayerAccount;

            var account = new MasterPlayerAccountProxy(guid, entity).MainAccount;

            Assert.IsNotNull(account.PlayerAccounts);
            Assert.IsTrue(account.PlayerAccounts.Contains(entity));
            Assert.IsTrue(account.PlayerAccounts.Count == 1);
        }

        [TestMethod()]
        public void RemoveTitlePlayerAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            TitlePlayerAccountEntity entity = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;

            var filledList = new List<TitlePlayerAccountEntity>();
            filledList.Add(entity);

            var account = new MasterPlayerAccountProxy(guid);

            account.RemoveTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsFalse(account.MainAccount.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void AddTitlePlayerAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            var emptyList = new List<TitlePlayerAccountEntity>();

            var account = new MasterPlayerAccountProxy(guid);

            TitlePlayerAccountEntity entity = new TitlePlayerAccountProxy(Guid.NewGuid().ToString()).PlayerAccount;

            account.AddTitlePlayerAccount(entity);

            Assert.IsNotNull(account.MainAccount.PlayerAccounts);
            Assert.IsTrue(account.MainAccount.PlayerAccounts.Contains(entity));
        }
    }
}