using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using Newtonsoft.Json;

namespace MoneyStats.Tests
{
    public class EvaluateRulesTestInput
    {
        public List<BankRow> BankRows { get; set; }
        public List<Rule> Rules { get; set; }
    }

    public class EvaluateRulesTestOutput
    {
        public List<Transaction> Transactions { get; set; }
        public List<TransactionCreatedWithRule> TransactionCreatedWithRules { get; set; }
        public List<TransactionTagConn> TransactionTagConns { get; set; }
    }

    [TestClass]
    public class EvaluateRulesTest
    {
        [TestMethod]
        public void TestTransactionGroup()
        {
            // Arrange
            var input = new EvaluateRulesTestInput()
            {
                BankRows = new List<BankRow>()
                {

                },
                Rules = new List<Rule>()
                {

                }
            };

            var output = new EvaluateRulesTestOutput()
            {
                Transactions = new List<Transaction>()
                {

                },
                TransactionCreatedWithRules = new List<TransactionCreatedWithRule>()
                {

                },
                TransactionTagConns = new List<TransactionTagConn>()
                {

                }
            };

            // Act & Assert
            this.ActAndAssert(input, output);
        }

        public void ActAndAssert(EvaluateRulesTestInput input, EvaluateRulesTestOutput output)
        {
            // Act
            var result = new ConditionRepository().CreateTransactionUsingRulesFlattened(input.Rules, input.BankRows);

            // Assert
            Assert.AreEqual(result.Transactions.Count, output.Transactions.Count);
            Assert.AreEqual(result.TransactionTagConns.Count, output.TransactionTagConns.Count);
            Assert.AreEqual(result.TransactionCreatedWithRules.Count, output.TransactionCreatedWithRules.Count);

            for (int i = 0; i < result.Transactions.Count; i++)
            {
                var a = JsonConvert.SerializeObject(result.Transactions[i]);
                var b = JsonConvert.SerializeObject(output.Transactions[i]);
                Assert.AreEqual(a, b);
            }
            for (int i = 0; i < result.TransactionTagConns.Count; i++)
            {
                var a = JsonConvert.SerializeObject(result.TransactionTagConns[i]);
                var b = JsonConvert.SerializeObject(output.TransactionTagConns[i]);
                Assert.AreEqual(a, b);
            }
            for (int i = 0; i < result.TransactionCreatedWithRules.Count; i++)
            {
                var a = JsonConvert.SerializeObject(result.TransactionCreatedWithRules[i]);
                var b = JsonConvert.SerializeObject(output.TransactionCreatedWithRules[i]);
                Assert.AreEqual(a, b);
            }
        }
    }
}
