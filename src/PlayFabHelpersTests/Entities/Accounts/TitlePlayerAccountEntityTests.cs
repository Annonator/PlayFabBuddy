using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var account = new TitlePlayerAccountEntity(guid);

            Assert.AreEqual(account.Id, guid);
        }

        [TestMethod()]
        public void TitlePlayerAccountEntityTest1()
        {
            string guid = Guid.NewGuid().ToString();

            var masterAccount = new MasterPlayerAccountEntity(guid);

            var titleAccount = new TitlePlayerAccountEntity(guid, masterAccount);

            Assert.IsTrue(masterAccount.PlayerAccounts.Count == 1);
            Assert.AreEqual(titleAccount, masterAccount.PlayerAccounts.First<TitlePlayerAccountEntity>());
        }

        [TestMethod()]
        public void AssignMasterAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            var masterAccount = new MasterPlayerAccountEntity(guid);
            var newMasterAccount = new MasterPlayerAccountEntity(Guid.NewGuid().ToString());

            var titleAccount = new TitlePlayerAccountEntity(guid, masterAccount);
            titleAccount.AssignMasterAccount(newMasterAccount);

            Assert.IsTrue(newMasterAccount.PlayerAccounts.Count == 1);
            Assert.AreEqual(titleAccount, newMasterAccount.PlayerAccounts.First<TitlePlayerAccountEntity>());
        }
    }
}