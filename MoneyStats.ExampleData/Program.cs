using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.ExampleData
{
    public static class DataControl
    {
        public static readonly List<string> TableDependencyOrder = new List<string>()
        {
            nameof(RuleAction),
            nameof(RuleActionType),
            nameof(TransactionTagConn),
            nameof(Tag),
            nameof(TransactionCreatedWithRule),
            nameof(Rule),
            nameof(RuleType),
            nameof(AndRuleGroup),
            nameof(RulesetRuleGroupConn),
            nameof(Ruleset),
            nameof(RuleGroup),
            nameof(Transaction),
            nameof(BankRow)
            // The deletion of Transaction and BankRow is unique! 
            // TODO If the migration did not create the foreign key constraint, then they're not unique.
        };

        /// <summary>
        /// Order doesn't matter.
        /// </summary>
        public static readonly Dictionary<string, object> BasicValues = new Dictionary<string, object>
        {
            // Tag
            {
                nameof(Tag),
                new List<Tag>
                {
                    new Tag() { Id = 1, Description = "", Title = "K&H", State = 1 },
                    new Tag() { Id = 2, Description = "", Title = "Kamat", State = 1 },
                }
            },
            // RuleType
            {
                nameof(RuleType),
                new List<RuleType>
                {
                    new RuleType() { Id = 1, Title = "TrueRule", State = 1 },
                    new RuleType() { Id = 2, Title = "IsEqualTo", State = 1 },
                    new RuleType() { Id = 3, Title = "IsGreaterThan", State = 1 },
                    new RuleType() { Id = 4, Title = "IsLesserThan", State = 1 },
                    new RuleType() { Id = 5, Title = "IsPropertyNull", State = 1 },
                    new RuleType() { Id = 6, Title = "IsPropertyNotNull", State = 1 },
                    new RuleType() { Id = 7, Title = "ContainsValueOfProperty", State = 1 },
                }
            },
            // RuleActionType
            {
                nameof(RuleActionType),
                new List<RuleActionType>
                {
                    new RuleActionType() { Id = 1, Title = "Omit", State = 1 },
                    new RuleActionType() { Id = 2, Title = "AddTag", State = 1 },
                    new RuleActionType() { Id = 3, Title = "SetValueOfProperty", State = 1 },
                    new RuleActionType() { Id = 4, Title = "AggregateToATransaction", State = 1 },
                }
            },
            // RuleGroup
            {
                nameof(RuleGroup),
                new List<RuleGroup>
                {
                    // Rule#1 : Típus == "Kamat" => addTags("K&H", "Kamat")
                    new RuleGroup() { Id = 1, Title = "K&H kamat", State = 1 },
                    // Rule#2
                }
            },
            // AndRuleGroup
            {
                nameof(AndRuleGroup),
                new List<AndRuleGroup>
                {
                    // Rule#1
                    new AndRuleGroup() { Id = 1, RuleGroupId = 1, State = 1 },
                    // Rule#2
                }
            },
            // Rule
            {
                nameof(Rule),
                new List<Rule>
                {
                    // Rule#1
                    new Rule() { Id = 1, RuleTypeId = 2, AndRuleGroupId = 1, Property = "Type", Value = "Kamat", State = 1 },
                    // Rule#2
                }
            },
            // RuleAction
            {
                nameof(RuleAction),
                new List<RuleAction>
                {
                    // Rule#1
                    new RuleAction() { Id = 1, RuleActionTypeId = 2, RuleGroupId = 1, Property = null, Value = null, TagId = 1, State = 1 },
                    new RuleAction() { Id = 2, RuleActionTypeId = 2, RuleGroupId = 1, Property = null, Value = null, TagId = 2, State = 1 },
                    // Rule#2
                }
            },
            // BankRow
            {
                nameof(BankRow),
                new List<BankRow>
                {
                    // Circular reference example
                    // Rule#1 valid
                    new BankRow() { Id = 1, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Állandó átutalás elekt bankon k", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = "HU68120527050013209800100008", PartnerName = "Generali Biztosító Zrt.", Sum = -11880, Currency = "HUF", Message = "225534585, rendszeres díj", State = 1 },
                    new BankRow() { Id = 1, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "440602******4503", Type = "Készpénzfelvét K&H ATM-ből", Account = "104040657157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = "SZENTMIHALYI UT 131.", Sum = -20000, Currency = "HUF", Message = null, State = 1 },
                    new BankRow() { Id = 1, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Állandó átut jutaléka-elektronikus", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = null, Sum = -90, Currency = "HUF", Message = null, State = 1 },
                }
            }
        };
    }

    public class Global
    {
        public void ReadRowCounts()
        {
            this.PrintCount(new TagRepository());
            this.PrintCount(new RuleTypeRepository());
            this.PrintCount(new RuleActionTypeRepository());
            this.PrintCount(new RuleGroupRepository());
            this.PrintCount(new AndRuleGroupRepository());
            this.PrintCount(new RuleRepository());
            this.PrintCount(new RuleActionRepository());
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
                this.AttachInsert(new RuleTypeRepository(), db);
                this.AttachInsert(new RuleActionTypeRepository(), db);
                this.AttachInsert(new RuleGroupRepository(), db);
                this.AttachInsert(new AndRuleGroupRepository(), db);
                this.AttachInsert(new RuleRepository(), db);
                this.AttachInsert(new RuleActionRepository(), db);
                // ...
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
                    // TODO doesn't work. Even though tables are empty, foreign contraint still throws an exception
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [" + tableName + "]");
                    db.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
                    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT([" + tableName + "], RESEED, 0)");

                    Console.WriteLine($"[{tableName}] DELETE and RESEED finished.");
                }
            }
        }

        public void DropAllTables()
        {
            using (var db = new MoneyStatsContext())
            {
                // Drop tables
                foreach (var tableName in DataControl.TableDependencyOrder)
                {
                    db.Database.ExecuteSqlCommand("DROP TABLE [" + tableName + "];");

                    Console.WriteLine($"[{tableName}] was dropped.");
                }

                // Drop meta tables
                var migrationHistoryTableName = "__EFMigrationsHistory";
                db.Database.ExecuteSqlCommand("DROP TABLE [" + migrationHistoryTableName + "];");
                Console.WriteLine($"[{migrationHistoryTableName}] was dropped.");
            }
        }

        void PrintCount<TEntity>(EntityBaseRepository<TEntity> repository) where TEntity : EntityBase
        {
            Console.WriteLine($"[{typeof(TEntity).Name}] has {repository.ForceGet().ToList().Count} rows.");
        }

        void AttachInsert<TEntity>(EntityBaseRepository<TEntity> repository, DbContext db) where TEntity : EntityBase
        {
            repository.InsertRange(db, (List<TEntity>)DataControl.BasicValues[typeof(TEntity).Name]);
        }

        void NeutralizeBankRowTable()
        {

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global();

#if true
            global.DropAllTables();
#endif

#if false
            global.InsertAllExamples();            
            global.ReadRowCounts();
            global.CleanDatabase();
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
