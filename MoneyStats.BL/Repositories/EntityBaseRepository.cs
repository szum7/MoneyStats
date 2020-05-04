using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<int> InsertRange(IEnumerable<TEntity> entities)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Set<TEntity>().AddRange(entities);
                context.SaveChanges();
                return entities.Select(x => x.Id);
            }
        }

        public int Insert(TEntity entity)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
                return entity.Id;
            }
        }

        public IEnumerable<TEntity> ForceGet()
        {
            using (var context = new MoneyStatsContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        public IEnumerable<TEntity> Get()
        {
            using (var context = new MoneyStatsContext())
            {
                // TODO "context.Set<TEntity>().ToList().Where(x => x.IsActive);" works, but why do we need it?
                return context.Set<TEntity>().ToList().Where(x => x.IsActive);
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

        public bool DeleteRange(IEnumerable<int> ids)
        {
            using (var context = new MoneyStatsContext())
            {
                var objs = this.Get();
                var toDeleteList = objs.Where(x => ids.Contains(x.Id));

                if (toDeleteList.ToList().Count == 0)
                {
                    // TODO log or throw
                    return false;
                }

                foreach (var toDeleteItem in toDeleteList)
                {
                    toDeleteItem.ModifiedDate = DateTime.Now;
                    toDeleteItem.State = 0;
                }
                context.SaveChanges();
                return true;
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

        public bool DestroyRange(IEnumerable<int> ids)
        {
            using (var context = new MoneyStatsContext())
            {
                var objs = (from e in context.Set<TEntity>()
                            where ids.Contains(e.Id)
                            select e).ToList();

                if (objs.Count == 0)
                {
                    // TODO log or throw
                    return false;
                }

                context.Set<TEntity>().RemoveRange(objs);
                context.SaveChanges();
                return true;
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

        #region For example inserts
        public void InsertRange(DbContext context, IEnumerable<TEntity> entities)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                context.Set<TEntity>().AddRange(entities);
                context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + typeof(TEntity).Name + "] ON;");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + typeof(TEntity).Name + "] OFF;");
                transaction.Commit();
            }
        }
        #endregion
    }
}
