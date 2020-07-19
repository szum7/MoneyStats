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
        public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> entities)
        {
            using (var context = new MoneyStatsContext())
            {
                foreach (var item in entities)
                {
                    item.SetNew();
                }

                context.Set<TEntity>().AddRange(entities);
                context.SaveChanges();
                return entities;
            }
        }

        public TEntity Insert(TEntity entity)
        {
            using (var context = new MoneyStatsContext())
            {
                entity.SetNew();

                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
                return entity;
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

        public bool UpdateMany(List<TEntity> entities)
        {
            using (var context = new MoneyStatsContext())
            {
                // Set each
                var i = 0;
                foreach (var entity in entities)
                {
                    var obj = context.Set<TEntity>().SingleOrDefault(x => x.Id == entity.Id);
                    if (obj != null)
                    {
                        // Update values
                        context.Entry(obj).CurrentValues.SetValues(entity);

                        // Update modified date
                        obj.ModifiedDate = DateTime.Now;

                        i++;
                    }
                }

                // Save changes
                if (i > 0)
                {
                    context.SaveChanges();
                    return true;
                }

                return false;
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
        public void InsertWithIdentity(DbContext context, IEnumerable<TEntity> entities)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var entityName = typeof(TEntity).Name;
                context.Set<TEntity>().AddRange(entities);
                context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + entityName + "] ON;");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + entityName + "] OFF;");
                transaction.Commit();

                Console.WriteLine($"InsertRange for [{entityName}] finished.");
            }
        }
        #endregion
    }
}
