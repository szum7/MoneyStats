using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.Tests
{
    [TestClass]
    public class TransactionRepositoryTest
    {
        static readonly Random RND = new Random();

        [TestMethod]
        public void InsertRange_Get_DestroyRangeTest()
        {
            // Arrange
            TransactionRepository repo = new TransactionRepository();
            List<BankRow> inserts = new List<BankRow>();
            int state = RND.Next(-2000, -1);
            int length = 10;

            for (int i = 0; i < length; i++)
                inserts.Add(new BankRow() { Message = $"UnitTest{i + 1}", State = state });

            // Act
            var originalGetResult = repo.ForceGet().ToList().Count;
            var insertRangeResult = repo.InsertRange(inserts);
            var actualGetResult = repo.ForceGet().ToList().Count;
            var deleteResult = repo.DestroyRange(insertRangeResult);

            // Assert
            Assert.AreEqual(10, insertRangeResult.ToList().Count);
            Assert.AreEqual(10, actualGetResult - originalGetResult);
            Assert.AreEqual(true, deleteResult);
        }

        [TestMethod]
        public void Insert_DestroyTest()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Insert_Update_DestroyTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
