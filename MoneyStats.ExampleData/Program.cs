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
                    new BankRow() { Id = 1, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular1", State = 1 },
                    new BankRow() { Id = 2, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular2", State = 1 },
                    // Grouped example
                    new BankRow() { Id = 3, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 4, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 5, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 6, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = 4, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group2", State = 1 },
                    // Rule#1 valid
                    new BankRow() { Id = 7, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Állandó átutalás elekt bankon k", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = "HU68120527050013209800100008", PartnerName = "Generali Biztosító Zrt.", Sum = -11880, Currency = "HUF", Message = "225534585, rendszeres díj", State = 1 },
                    new BankRow() { Id = 8, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "440602******4503", Type = "Készpénzfelvét K&H ATM-ből", Account = "104040657157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = "SZENTMIHALYI UT 131.", Sum = -20000, Currency = "HUF", Message = null, State = 1 },
                    new BankRow() { Id = 9, BankType = BankType.KH, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Állandó átut jutaléka-elektronikus", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = null, Sum = -90, Currency = "HUF", Message = null, State = 1 },
                }
            },
            // Transaction
            {
                nameof(Transaction),
                new List<Transaction>
                {
                    // Circular reference example
                    new Transaction() { Id = 1, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsGroup = false, IsCustom = false, BankRowId = 1, State = 1 },
                    new Transaction() { Id = 2, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsGroup = false, IsCustom = false, BankRowId = 2, State = 1 },
                    // Rule#1 valid
                    new Transaction() { Id = 3, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsGroup = true, IsCustom = false, BankRowId = null, State = 1 },
                    new Transaction() { Id = 4, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsGroup = true, IsCustom = false, BankRowId = null, State = 1 },
                }
            },
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
                this.AttachInsert(new RuleTypeRepository(), db);
                this.AttachInsert(new RuleActionTypeRepository(), db);
                this.AttachInsert(new RuleGroupRepository(), db);
                this.AttachInsert(new AndRuleGroupRepository(), db);
                this.AttachInsert(new RuleRepository(), db);
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
            using (var db = new MoneyStatsContext())
            {
                foreach (var tableName in DataControl.TableDependencyOrder)
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
            Console.WriteLine("Are you sure about DROPPING ALL TABLES?");
            var response = Console.ReadKey();
            if (response.Key != ConsoleKey.Y)
            {
                Console.WriteLine("\nAborted.");
                return;
            }

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
            repository.InsertRange(db, (List<TEntity>)DataControl.BasicValues[typeof(TEntity).Name]);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global();

#if true
            global.ReadRowCounts(); // READ
#endif

#if false
            global.DropAllTables(); // DROP
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT        
            global.ReadRowCounts(); // READ
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
