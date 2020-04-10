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
        public static bool Compare(string op, IComparable left, IComparable right)
        {
            switch (op)
            {
                case "<": return left.CompareTo(right) < 0;
                case ">": return left.CompareTo(right) > 0;
                case "<=": return left.CompareTo(right) <= 0;
                case ">=": return left.CompareTo(right) >= 0;
                case "==": return left.Equals(right);
                case "!=": return !left.Equals(right);
                default: throw new ArgumentException("Invalid comparison operator: {0}", op);
            }
        }

        [DataTestMethod]
        [DataRow(60, "<=", 50, false)]
        [DataRow(50, "<=", 50, true)]
        [DataRow(40, "<=", 50, true)]
        [DataRow(-2, "<=", 50, true)]
        [DataRow(0, "<=", 50, true)]
        [DataRow(0, "==", 50, false)]
        [DataRow(0, "!=", 50, true)]
        [DataRow(50, ">", 50, false)]
        [DataRow(51, ">", 50, true)]
        public void TestCompare(int ruleValue, string operand, int transactionValue, bool result)
        {
            // operand;Property;50;<=;
            // where Property <= 50

            var resultToTest = Misc.Compare(operand, ruleValue, transactionValue);
            Assert.AreEqual(resultToTest, result);

            if (operand != "==" && operand != "!=" && ruleValue != transactionValue)
            {
                var resultToTestNegated = Misc.Compare(operand, transactionValue, ruleValue);
                Assert.AreEqual(resultToTestNegated, !result);
            }
        }

        [TestMethod]
        public void TestRepository()
        {
            ITransactionRepository repo = new TransactionRepository();
            var transactions = repo.Get().ToList<BankRow>();
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
