using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IEntityBaseRepository<TEntity> where TEntity : EntityBase
    {
        IEnumerable<TEntity> ForceGet();

        IEnumerable<TEntity> Get();

        TEntity Select(int id);

        int Insert(TEntity transaction);

        IEnumerable<int> InsertRange(IEnumerable<TEntity> entities);
        
        bool Update(TEntity transaction);

        // Logical deletion (a State SQL-UPDATE)
        bool Delete(int id);

        bool DeleteRange(IEnumerable<int> ids);

        // Physical deletion (SQL-DELETE)
        bool Destroy(int id);

        bool DestroyRange(IEnumerable<int> ids);

        void InsertWithIdentity(DbContext context, IEnumerable<TEntity> entities);
    }
}
