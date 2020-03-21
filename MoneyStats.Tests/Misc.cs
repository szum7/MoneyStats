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

        [TestMethod]
        public void TestReflection()
        {
            if (typeof(IComparable).IsAssignableFrom(typeof(int)))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

            // d1.CompareTo(d2) < 0 -> d1 kisebb, régebbi dátum mint d2
            // d1.CompareTo(d2) > 0 -> d1 nagyobb, késõbbi dátum mint d2
            // d1.CompareTo(d2) = 0 -> d1 = d2
        }
    }
}
