using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.ExampleData
{
    /// <summary>
    /// Have as many dictionaries as you like. E.g. BasicValues dictionary for 
    /// general testing, BigData dictionary with long strings for max value testing, etc.
    /// 
    /// Dictionary item order doesn't matter.
    /// </summary>
    public static class CustomData
    {
        public static readonly Dictionary<string, object> UpdateWorkflowTest = new Dictionary<string, object>()
        {            
            // BankRow
            {
                nameof(BankRow),
                new List<BankRow>
                {
                    // null|undefined test
                    new BankRow() { Id = 1, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 2000, Currency = null, Message = "" }.SetNew(),
                    // other tests
                    new BankRow() { Id = 2, AccountingDate = new DateTime(2010, 10, 10), BankTransactionId = "bankTransactionId", Type = "type", Account = "account", AccountName = "accountName", PartnerAccount = "partnerAccount", PartnerName = "partnerName", Sum = 1, Currency = "currency", Message = "message" }.SetNew(),
                }
            },
        };

        public static readonly Dictionary<string, object> BasicValues = new Dictionary<string, object>
        {
            // Tag
            {
                nameof(Tag),
                new List<Tag>
                {
                    new Tag() { Id = 1, Description = "", Title = "K&H" }.SetNew(),
                    new Tag() { Id = 2, Description = "", Title = "Kamat" }.SetNew(),
                }
            },
            // Rule
            {
                nameof(Rule),
                new List<Rule>
                {
                    // These rules are mostly fictional examples, do not apply them to real bank exported excel files!
                    // Rule#1 : Típus == "Kamat" => addTags("K&H", "Kamat")
                    new Rule() { Id = 1, Title = "K&H kamat" }.SetNew(),
                    // Rule#2 : (PartnerName == "Elemér" && Sum < 2000) || Currency == "EUR" => setProperty(Title, "Elemér < 2000 | EUR")
                    new Rule() { Id = 2, Title = "Elemér < 2000 | EUR" }.SetNew(),
                    // Rule#3 : Type == "food" => aggregate()
                    new Rule() { Id = 3, Title = "Aggregate to transaction" }.SetNew(),
                }
            },
            // AndConditionGroup
            {
                nameof(AndConditionGroup),
                new List<AndConditionGroup>
                {
                    // Rule#1
                    new AndConditionGroup() { Id = 1, RuleId = 1 }.SetNew(),
                    // Rule#2
                    new AndConditionGroup() { Id = 2, RuleId = 2 }.SetNew(), // (PartnerName == "Elemér") && (Sum < 2000)
                    new AndConditionGroup() { Id = 3, RuleId = 2 }.SetNew(), // (Currency == "EUR")
                    // Rule#3
                    new AndConditionGroup() { Id = 4, RuleId = 3 }.SetNew(), // (Type == "food")
                }
            },
            // Condition
            {
                nameof(Condition),
                new List<Condition>
                {
                    // Rule#1
                    new Condition() { Id = 1, AndConditionGroupId = 1, Property = "Type", Value = "Kamat", ConditionType = ConditionType.IsEqualTo }.SetNew(),
                    // Rule#2
                    new Condition() { Id = 2, AndConditionGroupId = 2, Property = "PartnerName", Value = "Elemér", ConditionType = ConditionType.IsEqualTo }.SetNew(), // PartnerName == "Elemér"
                    new Condition() { Id = 3, AndConditionGroupId = 2, Property = "Sum", Value = "2000", ConditionType = ConditionType.IsLesserThan }.SetNew(), // Sum < 2000
                    new Condition() { Id = 4, AndConditionGroupId = 3, Property = "Currency", Value = "EUR", ConditionType = ConditionType.IsEqualTo }.SetNew(), // Currency == "EUR"
                    // Rule#3
                    new Condition() { Id = 5, AndConditionGroupId = 4, Property = "Type", Value = "food", ConditionType = ConditionType.IsEqualTo }.SetNew(), // Type == "food"
                }
            },
            // RuleAction
            {
                nameof(RuleAction),
                new List<RuleAction>
                {
                    // Rule#1
                    new RuleAction() { Id = 1, RuleId = 1, Property = null, Value = null, TagId = 1, RuleActionType = RuleActionType.AddTag }.SetNew(),
                    new RuleAction() { Id = 2, RuleId = 1, Property = null, Value = null, TagId = 2, RuleActionType = RuleActionType.AddTag }.SetNew(),
                    // Rule#2
                    new RuleAction() { Id = 3, RuleId = 2, Property = "Title", Value = "Elemér < 2000 | EUR", TagId = null, RuleActionType = RuleActionType.SetValueOfProperty }.SetNew(),
                    // Rule#3
                    new RuleAction() { Id = 4, RuleId = 3, Property = null, Value = null, TagId = null, RuleActionType = RuleActionType.AggregateToMonthlyTransaction }.SetNew(),
                }
            },
            // BankRow
            {
                nameof(BankRow),
                new List<BankRow>
                {
                    // Circular reference example
                    new BankRow() { Id = 1, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular1" }.SetNew(),
                    new BankRow() { Id = 2, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular2" }.SetNew(),
                    // Grouped example
                    new BankRow() { Id = 3, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1" }.SetNew(),
                    new BankRow() { Id = 4, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1" }.SetNew(),
                    new BankRow() { Id = 5, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1" }.SetNew(),
                    new BankRow() { Id = 6, IsTransactionCreated = true, GroupedTransactionId = 4, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group2" }.SetNew(),
                    // Rule#1 valid
                    new BankRow() { Id = 7, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Kamat", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = "HU68120527050013209800100008", PartnerName = "Generali Biztosító Zrt.", Sum = -11880, Currency = "HUF", Message = "225534585, rendszeres díj" }.SetNew(),
                    new BankRow() { Id = 8, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "440602******4503", Type = "Kamat", Account = "104040657157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = "SZENTMIHALYI UT 131.", Sum = -20000, Currency = "HUF", Message = null }.SetNew(),
                    new BankRow() { Id = 9, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Kamat", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = null, Sum = -90, Currency = "HUF", Message = null }.SetNew(),
                    // Rule#2 tests (1 validates, 0 does not)
                    /*1*/ new BankRow() { Id = 10, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 100, Currency = null, Message = null }.SetNew(),
                    /*1*/ new BankRow() { Id = 11, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Zsolt", Sum = 250000, Currency = "EUR", Message = null }.SetNew(),
                    /*0*/ new BankRow() { Id = 12, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Zsolt", Sum = 100, Currency = null, Message = null }.SetNew(),
                    /*1*/ new BankRow() { Id = 13, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 200000, Currency = "EUR", Message = null }.SetNew(),
                    /*0*/ new BankRow() { Id = 14, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 200000, Currency = null, Message = null }.SetNew(),
                    // Rule#3
                    new BankRow() { Id = 15, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 21), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 100, Currency = null, Message = null }.SetNew(),
                    new BankRow() { Id = 16, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 3, 11), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 20, Currency = null, Message = null }.SetNew(),
                    new BankRow() { Id = 17, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 16), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 20, Currency = null, Message = null }.SetNew(),
                    new BankRow() { Id = 18, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 3, Currency = null, Message = null }.SetNew(),
                    new BankRow() { Id = 19, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 3, 11), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 2, Currency = null, Message = null }.SetNew(),
                    new BankRow() { Id = 20, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 9, 20), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 111, Currency = null, Message = null }.SetNew(),
                }
            },
            // Transaction
            {
                nameof(Transaction),
                new List<Transaction>
                {
                    // Circular reference example
                    new Transaction() { Id = 1, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = 1 }.SetNew(),
                    new Transaction() { Id = 2, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = 2 }.SetNew(),
                    // Rule#1 valid
                    new Transaction() { Id = 3, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = null }.SetNew(),
                    new Transaction() { Id = 4, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = null }.SetNew(),
                }
            },
        };
    }
}
