using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MoneyStats.ExampleData
{
    public static class Extensions
    {
        public static IQueryable Set2(this DbContext context, Type T)
        {
            // Get the generic type definition
            MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable;
        }

        public static IQueryable<T> Set2<T>(this DbContext context)
        {
            // Get the generic type definition 
            MethodInfo method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

            // Build a method with the specific type argument you're interested in 
            method = method.MakeGenericMethod(typeof(T));

            return method.Invoke(context, null) as IQueryable<T>;
        }
    }

    public class ModelValue
    {
        public bool IsActive { get; set; }
        public object Entities { get; set; }
        public EntityBase ModelClass { private get; set; }
        public string TableName => this.ModelClass?.GetType().Name;
        public Type TypeOfModelSet => this.ModelClass?.GetType();

        public ModelValue()
        {

        }
    }

    /// <summary>
    /// Order of the value lists matter! Item at index 0 will be 
    /// inserted first, and item at length - 1 will be inserted last.
    /// </summary>
    public static class DataControl
    {
        public static readonly List<string> TableDependencyOrder = new List<string>()
        {
            nameof(RuleGroup),
            nameof(AndRuleGroup),
            nameof(Rule),
            nameof(RuleType),
            nameof(RulesetRuleGroupConn),
            nameof(Ruleset),
            nameof(TransactionCreatedWithRule),
            nameof(TransactionTagConn),
            nameof(RuleActionTagConn),
            nameof(Tag),
            nameof(RuleAction),
            nameof(RuleActionType),
            nameof(Transaction),
            nameof(BankRow)
            // The deletion of Transaction and BankRow is unique! 
            // TODO If the migration did not create the foreign key constraint, then they're not unique.
        };

        public static readonly List<ModelValue> BasicValues = new List<ModelValue>()
        {
            new ModelValue()
            {
                ModelClass = new RuleType(),
                IsActive = true,
                Entities = new List<RuleType>()
                {
                    new RuleType() { Id = 1, Title = "", State = 1 },
                    new RuleType() { Id = 2, Title = "", State = 1 },
                    new RuleType() { Id = 3, Title = "", State = 1 }
                }
            },
            new ModelValue()
            {
                ModelClass = new Tag(),
                IsActive = true,
                Entities = new List<Tag>()
                {
                    new Tag() { Id = 1, Description = "", Title = "", State = 1 },
                    new Tag() { Id = 2, Description = "", Title = "", State = 1 },
                    new Tag() { Id = 3, Description = "", Title = "", State = 1 }
                }
            }
        };
    }

    public class Something
    {
        public DbSet<RuleType> RuleTypes { get => this.db.RuleTypes; }
        public MoneyStatsContext db { get; set; }
        public Something(MoneyStatsContext db)
        {
            this.db = db;
        }
    }

    public class Global
    {
        Dictionary<string, Func<DbContext, IQueryable>> myDictionary = new Dictionary<string, Func<DbContext, IQueryable>>()
        {
            { (new RuleType()).GetType().Name, (DbContext context) => context.Set<RuleType>() }
        };

        public Global()
        {

        }

        //DbSet<object> Akarmi(MoneyStatsContext db)
        //{

        //}

        public void Test()
        {
            using (var db = new MoneyStatsContext())
            {
                var s = new Something(db);
                foreach (var modelValue in DataControl.BasicValues)
                {
                    if (!modelValue.IsActive)
                        continue;

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        
                        var data = new RuleType { Id = 222, Title = "Test:CreatedWithIdentityInsertOn 2", State = 1 };
                        //db.Set<RuleType>().Add(data);
                        //db.Set<modelValue.TypeOfModelSet>().AddRange(modelValue.Entities);
                        //var type = modelValue.TypeOfModelSet;
                        //db.Set<typeof(RuleType)>().AddRange(modelValue.Entities);
                        //var dbSet = (DbSet<object>)myDictionary[modelValue.TableName].Invoke(db);
                        //var dbSet = (DbSet<object>)s.GetType().GetProperty("RuleTypes").GetValue(s);
                        var dbSet = (s.GetType().GetProperty("RuleTypes").GetValue(s) as IListSource).GetList();
                        dbSet.Add(data);
                        // TODO does not work, no dynamic dbcontext set in EF Core 3.0
                        //db.Set2(modelValue.TypeOfModelSet).Add(data);


                        db.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + modelValue.TableName + "] ON;");
                        db.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + modelValue.TableName + "] OFF;");
                        transaction.Commit();
                    }
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all records from every tables.
        /// Handles the order of which tables can 
        /// be cleaned (foreign key dependencies).
        /// </summary>
        public void CleanDatabase()
        {
            using (var db = new MoneyStatsContext())
            {
                foreach (var tableName in DataControl.TableDependencyOrder)
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [" + tableName + "]");
                }
            }
        }

        void NeutralizeBankRowTable()
        {

        }

        /// <summary>
        /// Inserts all examples.
        /// Handles the order of which records can 
        /// be inserted (foreign key dependencies).
        /// </summary>
        public void InsertAllExamples()
        {

        }
    }

    public abstract class BaseExample
    {
        protected EntityBase ModelClass { get; set; }
        protected string TableName => this.ModelClass?.GetType().Name;

        public BaseExample(EntityBase modelClass)
        {
            this.ModelClass = modelClass;
        }

        protected void SaveIdentityInsert(DbContext db, IDbContextTransaction transaction)
        {
            db.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + TableName + "] ON;");
            db.SaveChanges();
            db.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT [dbo].[" + TableName + "] OFF;");
            transaction.Commit();
        }
    }

    public class RuleTypeExample : BaseExample
    {

        public RuleTypeExample() 
            : base(new RuleType())
        {
        }

        public void Insert()
        {
            using (var db = new MoneyStatsContext())
            using (var transaction = db.Database.BeginTransaction())
            {
                var data = new RuleType { Id = 222, Title = "Test:CreatedWithIdentityInsertOn 2", State = 1 };
                db.RuleTypes.Add(data);

                this.SaveIdentityInsert(db, transaction);
            }
        }
    }

    #region version 1
    public interface IExample<TEntity> where TEntity : EntityBase
    {
        public IEntityBaseRepository<TEntity> Repo { get; set; }

        void InsertRange();

        /// <summary>
        /// Physical deletion. There's no need 
        /// for logical deletion in this project.
        /// </summary>
        void DeleteAll();
    }

    public class RuleGroupExample : IExample<RuleGroup>
    {
        public IEntityBaseRepository<RuleGroup> Repo { get; set; }

        public RuleGroupExample()
        {
            this.Repo = new RuleGroupRepository();
        }

        public void InsertRange()
        {
            var e = new List<RuleGroup>();
            // TODO add examples
            this.Repo.InsertRange(e);
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }

    public class BankRowExample : IExample<BankRow>
    {
        public IEntityBaseRepository<BankRow> Repo { get; set; }

        public BankRowExample()
        {
            this.Repo = new BankRowRepository();
        }

        public void InsertRange()
        {
            var e = new List<BankRow>();
            // TODO add examples
            this.Repo.InsertRange(e);
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global();
            global.Test();
            //(new RuleTypeExample()).Insert();

#if false
            global.InsertAllExamples();
#endif

#if false
            global.CleanDatabase();
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
