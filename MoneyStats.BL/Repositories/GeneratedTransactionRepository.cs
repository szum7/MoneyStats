using MoneyStats.BL.Common;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Modules.TransactionGeneration;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL.Repositories
{
    public class GeneratedTransactionRepository : IGeneratedTransactionRepository
    {
        public List<GeneratedTransaction> Generate(List<Rule> rules, List<BankRow> bankRows)
        {
            return new TransactionGenerator().Generate(rules, bankRows);
        }

        public GenericResponse<bool> SaveAll(List<GeneratedTransaction> suggestedTransactions)
        {
            return new GeneratedTransactionHelper().SaveAll(suggestedTransactions);
        }
    }
}
