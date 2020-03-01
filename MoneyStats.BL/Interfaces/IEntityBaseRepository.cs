using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IEntityBaseRepository<TEntity> where TEntity : EntityBase
    {
        IEnumerable<TEntity> Get();

        TEntity Select(int id);

        int Insert(TEntity transaction);
        
        bool Update(TEntity transaction);

        bool Delete(int id);

        bool Destroy(int id);
    }
}
