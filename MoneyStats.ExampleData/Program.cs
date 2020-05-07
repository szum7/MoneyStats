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
                nameof(Tag),
                new List<Tag>
                {
                    new Tag() { Id = 1, Description = "", Title = "K&H", State = 1 },
                    new Tag() { Id = 2, Description = "", Title = "Kamat", State = 1 },
                }
            },
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
            {
                nameof(RuleGroup),
                new List<RuleGroup>
                {
                    // Rule#1 : Típus == "Kamat" => addTags("K&H", "Kamat")
                    new RuleGroup() { Id = 1, Title = "K&H kamat", State = 1 },
                    // Rule#2
                }
            },
            {
                nameof(AndRuleGroup),
                new List<AndRuleGroup>
                {
                    // Rule#1
                    new AndRuleGroup() { Id = 1, RuleGroupId = 1, State = 1 },
                    // Rule#2
                }
            },
            {
                nameof(Rule),
                new List<Rule>
                {
                    // Rule#1
                    new Rule() { Id = 1, RuleTypeId = 2, AndRuleGroupId = 1, Property = "Type", Value = "Kamat", State = 1 },
                    // Rule#2
                }
            },
            {
                nameof(RuleAction),
                new List<RuleAction>
                {
                    // Rule#1
                    new RuleAction() { Id = 1, RuleActionTypeId = 2, RuleGroupId = 1, Property = null, Value = null, TagId = 1, State = 1 },
                    new RuleAction() { Id = 1, RuleActionTypeId = 2, RuleGroupId = 1, Property = null, Value = null, TagId = 2, State = 1 },
                    // Rule#2
                }
            },
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
                this.AttachInsert(new TagRepository(), db);
                this.AttachInsert(new RuleTypeRepository(), db);
                // ...
            }
        }

        void AttachInsert<TEntity>(EntityBaseRepository<TEntity> repository, DbContext db) where TEntity : EntityBase
        {
            repository.InsertRange(db, (List<TEntity>)DataControl.BasicValues[nameof(TEntity)]);
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
