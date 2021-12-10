using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var account = new MasterPlayerAccountEntity(guid);

            Assert.AreEqual(guid, account.Id);
            Assert.AreEqual(emptyList.Count, account.PlayerAccounts.Count);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestMainConstructor()
        {
            string guid = Guid.NewGuid().ToString();

            var filledList = new List<TitlePlayerAccountEntity>();
            filledList.Add(new TitlePlayerAccountEntity(guid));

            var account2 = new MasterPlayerAccountEntity(guid, filledList);

            Assert.AreEqual(filledList, account2.PlayerAccounts);
        }

        [TestMethod()]
        public void MasterPlayerAccountEntityTestConstructor2()
        {
            string guid = Guid.NewGuid().ToString();

            var entity = new TitlePlayerAccountEntity(guid);

            var account = new MasterPlayerAccountEntity(guid, entity);

            Assert.IsTrue(account.PlayerAccounts.Contains(entity));
            Assert.IsTrue(account.PlayerAccounts.Count == 1);
        }

        [TestMethod()]
        public void RemoveTitlePlayerAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            TitlePlayerAccountEntity entity = new TitlePlayerAccountEntity(Guid.NewGuid().ToString());

            var filledList = new List<TitlePlayerAccountEntity>();
            filledList.Add(entity);

            var account = new MasterPlayerAccountEntity(guid);

            account.RemoveTitlePlayerAccount(entity);

            Assert.IsFalse(account.PlayerAccounts.Contains(entity));
        }

        [TestMethod()]
        public void AddTitlePlayerAccountTest()
        {
            string guid = Guid.NewGuid().ToString();

            var emptyList = new List<TitlePlayerAccountEntity>();

            var account = new MasterPlayerAccountEntity(guid);

            TitlePlayerAccountEntity entity = new TitlePlayerAccountEntity(Guid.NewGuid().ToString());

            account.AddTitlePlayerAccount(entity);

            Assert.IsTrue(account.PlayerAccounts.Contains(entity));
        }
    }
}