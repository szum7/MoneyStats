using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MoneyStats.DAL.Models;
using Newtonsoft.Json;
using MoneyStats.BL.Modules;

namespace MoneyStats.Tests
{
    public class EvaluateRulesTestInput
    {
        public List<BankRow> BankRows { get; set; }
        public List<Rule> Rules { get; set; }
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

            var output = new List<GeneratedTransaction>()
            {
            };

            // Act & Assert
            this.ActAndAssert(input, output);
        }

        public void ActAndAssert(EvaluateRulesTestInput input, List<GeneratedTransaction> output)
        {
            // Act
            var result = new TransactionGenerator().Generate(input.Rules, input.BankRows);

            // Assert
            Assert.AreEqual(result.Count, output.Count);
            Assert.AreEqual(result.Count, output.Count);
            Assert.AreEqual(result.Count, output.Count);

            for (int i = 0; i < result.Count; i++)
            {
                var a = JsonConvert.SerializeObject(result[i]);
                var b = JsonConvert.SerializeObject(output[i]);
                Assert.AreEqual(a, b);
            }
            // Do this for other objects as well
        }
    }
}
