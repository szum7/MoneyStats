using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Common;
using MoneyStats.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public class BankRowRepository : EntityBaseRepository<BankRow>, IBankRowRepository
    {
        public List<BankRow> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var list = (from d in context.BankRows.ToList()
                            join a in context.Transactions on d.GroupedTransactionId equals a.Id
                            where d.IsActive && d.GroupedTransactionId != null
                            select new BankRow()
                            {
                                GroupedTransaction = a
                            }).ToList();

                return list;
            }
        }

        public List<BankRow> GetOnIds(List<int> ids)
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.BankRows
                        where ids.Any(x => x == d.Id)
                        select d).ToList();
            }
        }

        public void UpdateGroupedTransactionIds(List<BankRow> rows)
        {
            var list = rows.Where(x => x.GroupedTransaction != null).ToList();

            if (list.Count == 0)
                return;

            using (var context = new MoneyStatsContext())
            {
                // TODO this shouldn't work!
                list.ForEach(x => x.GroupedTransactionId = x.GroupedTransaction.Id);
                context.SaveChanges();
            }
        }

        public List<string> GetBankRowProperties()
        {
            return (from property in typeof(BankRow).GetProperties()
                    where property.CustomAttributes.Any(customAttr => customAttr.AttributeType == typeof(Rulable))
                    select property.Name).ToList();
        }
    }
}
