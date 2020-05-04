using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.ExampleData
{
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

        /// <summary>
        /// Order doesn't matter.
        /// </summary>
        public static readonly Dictionary<string, object> BasicValues = new Dictionary<string, object>
        {
            {
                nameof(RuleType),
                new List<RuleType>
                {
                    new RuleType() { Id = 1, Title = "a", State = 1 },
                    new RuleType() { Id = 2, Title = "b", State = 1 },
                    new RuleType() { Id = 3, Title = "c", State = 1 }
                }
            },
            {
                nameof(Tag), 
                new List<Tag>
                {
                    new Tag() { Id = 1, Description = "", Title = "d", State = 1 },
                    new Tag() { Id = 2, Description = "", Title = "e", State = 1 },
                    new Tag() { Id = 3, Description = "", Title = "f", State = 1 }
                } 
            }
        };
    }

    public class Global
    {
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
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [" + tableName + "]");
                }
            }
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
                new TagRepository().InsertRange(db, (List<Tag>)DataControl.BasicValues[nameof(Tag)]);
                new RuleTypeRepository().InsertRange(db, (List<RuleType>)DataControl.BasicValues[nameof(RuleType)]);
                // ...
            }
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
            global.InsertAllExamples();
#endif

#if true
            global.CleanDatabase();
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
