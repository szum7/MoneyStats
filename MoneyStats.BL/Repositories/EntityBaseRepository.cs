using MoneyStats.BL.Interfaces;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Repositories
{
    public abstract class EntityBaseRepository<TEntity> : IEntityBaseRepository<TEntity> where TEntity : EntityBase
    {
        public int Insert(TEntity entity)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
                return entity.Id;
            }
        }

        public IEnumerable<TEntity> Get()
        {
            using (var context = new MoneyStatsContext())
            {
                // TODO "context.Set<TEntity>().ToList().Where(x => x.IsActive);" works, but why do we need it?
                return context.Set<TEntity>().Where(x => x.IsActive);
            }
        }

        public bool Update(TEntity entity)
        {
            using (var context = new MoneyStatsContext())
            {
                var obj = context.Set<TEntity>().SingleOrDefault(x => x.Id == entity.Id);

                if (obj != null)
                {
                    context.Entry(obj).CurrentValues.SetValues(entity);
                    obj.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                // TODO log or throw
                return false;
            }
        }

        public TEntity Select(int id)
        {
            using (var context = new MoneyStatsContext())
            {
                return context.Set<TEntity>().SingleOrDefault(x => x.Id == id && x.IsActive);
            }
        }

        public bool Delete(int id)
        {
            using (var context = new MoneyStatsContext())
            {
                var obj = context.Set<TEntity>().SingleOrDefault(x => x.Id == id && x.IsActive);

                if (obj != null)
                {
                    obj.ModifiedDate = DateTime.Now;
                    obj.State = 0;
                    context.SaveChanges();
                    return true;
                }
                // TODO log or throw
                return false;
            }
        }

        public bool Destroy(int id)
        {
            using (var context = new MoneyStatsContext())
            {
                var obj = context.Set<TEntity>().SingleOrDefault(x => x.Id == id);

                if (obj != null)
                {
                    context.Set<TEntity>().Remove(obj);
                    context.SaveChanges();
                    return true;
                }
                // TODO log or throw
                return false;
            }
        }
    }
}
