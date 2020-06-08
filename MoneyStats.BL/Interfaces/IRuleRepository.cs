using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IRuleRepository : IEntityBaseRepository<Rule>
    {
        public List<Rule> GetWithEntities();
    }
}
