using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.ExampleData
{
    public class Global
    {
        private Dictionary<string, object> _insertValues { get; set; }

        public Global(Dictionary<string, object> insertValues)
        {
            this._insertValues = insertValues;
        }

        public void ReadRowCounts()
        {
            this.PrintCount(new TagRepository());
            this.PrintCount(new RuleRepository());
            this.PrintCount(new AndConditionGroupRepository());
            this.PrintCount(new ConditionRepository());
            this.PrintCount(new RuleActionRepository());
            this.PrintCount(new BankRowRepository());
            this.PrintCount(new TransactionRepository());
            LineBreak();
        }

        /// <summary>
        /// Inserts all examples.
        /// Handles the order of which records can 
        /// be inserted (foreign key dependencies).
        /// </summary>
        public void InsertAllExamples()
        {
            using (var db = new MoneyStatsContext())
            {
                this.AttachInsert(new TagRepository(), db);
                this.AttachInsert(new RuleRepository(), db);
                this.AttachInsert(new AndConditionGroupRepository(), db);
                this.AttachInsert(new ConditionRepository(), db);
                this.AttachInsert(new RuleActionRepository(), db);
                this.AttachInsert(new BankRowRepository(), db);
                this.AttachInsert(new TransactionRepository(), db);
                // ...
            }
            LineBreak();
        }

        /// <summary>
        /// Deletes all records from every tables.
        /// Handles the order of which tables can 
        /// be cleaned (foreign key dependencies).
        /// </summary>
        public void DeleteAllFromDatabase()
        {
            this.AreYouSure("You are about to DELETE all records! Do you want to continue?");

            using (var db = new MoneyStatsContext())
            {
                foreach (var tableName in TableDependencyOrder.List)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
                    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT([" + tableName + "], RESEED, 0)");

                    Console.WriteLine($"[{tableName}] DELETE and RESEED finished.");
                }
            }
            LineBreak();
        }

        public void DropAllTables()
        {
            this.AreYouSure("You are about to DROP all tables! Do you want to continue?");

            using (var db = new MoneyStatsContext())
            {
                // Drop tables
                foreach (var tableName in TableDependencyOrder.List)
                {
                    db.Database.ExecuteSqlCommand("DROP TABLE [" + tableName + "];");
                    Console.WriteLine($"[{tableName}] was dropped.");
                }

                // Drop meta tables
                var migrationHistoryTableName = "__EFMigrationsHistory";
                db.Database.ExecuteSqlCommand("DROP TABLE [" + migrationHistoryTableName + "];");
                Console.WriteLine($"[{migrationHistoryTableName}] was dropped.");
            }
            LineBreak();
        }

        void LineBreak()
        {
            Console.WriteLine("");
        }

        void PrintCount<TEntity>(EntityBaseRepository<TEntity> repository) where TEntity : EntityBase
        {
            Console.WriteLine($"[{typeof(TEntity).Name}] has {repository.ForceGet().ToList().Count} rows.");
        }

        void AttachInsert<TEntity>(EntityBaseRepository<TEntity> repository, DbContext db) where TEntity : EntityBase
        {
            object entityList;
            if (!_insertValues.TryGetValue(typeof(TEntity).Name, out entityList))
                return;

            repository.InsertWithIdentity(db, (List<TEntity>)entityList);
        }

        void AreYouSure(string message)
        {
            Console.WriteLine($"{message} [y]");
            var response = Console.ReadLine();
            if (response.ToLower() != "y")
            {
                Console.WriteLine("\nAborted.");
                throw new NotSureWhatIWasDoingException();
            }
        }
    }
}
