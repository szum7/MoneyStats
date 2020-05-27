using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>, ITransactionRepository
    {
        public void SaveTransactionTagConns(List<Transaction> transactions)
        {
            var inserts = new List<TransactionTagConn>();

            foreach (var transaction in transactions)
            {
                foreach (var tag in transaction.Tags)
                {
                    inserts.Add(new TransactionTagConn()
                    {
                        TagId = tag.Id,
                        TransactionId = transaction.Id,
                        CreateDate = DateTime.Now,
                        State = 1
                    });
                }
            }

            new TransactionTagConnRepository().InsertRange(inserts);
        }
    }
}
