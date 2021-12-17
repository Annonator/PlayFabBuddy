﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFabBuddy.PlayFabHelpers.Proxy.Accounts;
using System;
using System.Linq;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts.Tests
{
    [TestClass()]
    public class TitlePlayerAccountEntityTests
    {
        [TestMethod()]
        public void TitlePlayerAccountEntityTest()
        {
            string guid = Guid.NewGuid().ToString();

            var account = new TitlePlayerAccountProxy(guid);

            Assert.AreEqual(account.PlayerAccount.Id, guid);
        }

        [TestMethod()]
        public void TitlePlayerAccountEntityTest1()
        {
            string guid = Guid.NewGuid().ToString();

            var masterAccount = new MasterPlayerAccountProxy(guid);

            var titleAccount = new TitlePlayerAccountProxy(guid, masterAccount.MainAccount);

            Assert.IsNotNull(masterAccount.MainAccount.PlayerAccounts);
            Assert.IsTrue(masterAccount.MainAccount.PlayerAccounts.Count == 1);
            Assert.AreEqual(titleAccount, masterAccount.MainAccount.PlayerAccounts.First<TitlePlayerAccountEntity>());
        }

        [TestMethod()]
        public void AssignMasterAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            var masterAccount = new MasterPlayerAccountProxy(guid).MainAccount;
            var newMasterAccount = new MasterPlayerAccountProxy(Guid.NewGuid().ToString()).MainAccount;

            var titleAccount = new TitlePlayerAccountProxy(guid, masterAccount);
            titleAccount.AssignMasterAccount(newMasterAccount);

            Assert.IsTrue(newMasterAccount.PlayerAccounts.Count == 1);
            Assert.AreEqual(titleAccount, newMasterAccount.PlayerAccounts.First<TitlePlayerAccountEntity>());
        }
    }
}