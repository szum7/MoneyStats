using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL.Mocks
{
    class BankRowMockRepository : IBankRowRepository
    {
        public bool Delete(int id)
        {
            return true;
        }

        public bool DeleteRange(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public bool Destroy(int id)
        {
            return true;
        }

        public bool DestroyRange(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public void ExtraMethod()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankRow> ForceGet()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankRow> Get()
        {
            throw new NotImplementedException();
        }

        public List<string> GetBankRowProperties()
        {
            throw new NotImplementedException();
        }

        public BankRow Insert(BankRow transaction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankRow> InsertRange(IEnumerable<BankRow> entities)
        {
            throw new NotImplementedException();
        }

        public void InsertWithIdentity(DbContext context, IEnumerable<BankRow> entities)
        {
            throw new NotImplementedException();
        }

        public BankRow Select(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(BankRow transaction)
        {
            return true;
        }
    }
}
