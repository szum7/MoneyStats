using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.Tests
{
    [TestClass]
    public class Misc
    {
        [TestMethod]
        public void TestRepository()
        {
            ITransactionRepository repo = new TransactionRepository();
            var transactions = repo.Get().ToList<Transaction>();
            //repo.ExtraMethod();

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Test2()
        {
            TransactionRepository repo = new TransactionRepository();
            var tr1 = repo.LazyLoading();
            var tr2 = repo.EagerLoading();

            Assert.AreEqual(1, 1);
        }
    }
}
