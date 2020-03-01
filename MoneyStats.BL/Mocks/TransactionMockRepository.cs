using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL.Mocks
{
    class TransactionMockRepository : ITransactionRepository
    {
        public bool Delete(int id)
        {
            return true;
        }

        public bool Destroy(int id)
        {
            return true;
        }

        public void ExtraMethod()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> Get()
        {
            throw new NotImplementedException();
        }

        public int Insert(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Transaction Select(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Transaction transaction)
        {
            return true;
        }
    }
}
