using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;

namespace MoneyStats.Tests
{
    [TestClass]
    public class Misc
    {
        [TestMethod]
        public void TestRepository()
        {
            ITransactionRepository repo = new TransactionRepository();
            var transactions = repo.Get();
        }
    }
}
