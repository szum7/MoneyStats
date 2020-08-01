using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Common;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>, ITransactionRepository
    {
        public List<string> GetTransactionRulableProperties()
        {
            return (from property in typeof(Transaction).GetProperties()
                    where property.CustomAttributes.Any(customAttr => customAttr.AttributeType == typeof(Rulable))
                    select property.Name).ToList();
        }
    }
}
