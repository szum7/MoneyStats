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
            // RuleGroup
            {
                nameof(RuleGroup),
                new List<RuleGroup>
                {
                    // These rules are mostly fictional examples, do not apply them to real bank exported excel files!
                    // Rule#1 : Típus == "Kamat" => addTags("K&H", "Kamat")
                    new RuleGroup() { Id = 1, Title = "K&H kamat", State = 1 },
                    // Rule#2 : (PartnerName == "Elemér" && Sum < 2000) || Currency == "EUR" => setProperty(Title, "Elemér < 2000 | EUR")
                    new RuleGroup() { Id = 2, Title = "Elemér < 2000 | EUR", State = 1 },
                    // Rule#3 : Type == "food" => aggregate()
                    new RuleGroup() { Id = 3, Title = "Aggregate to transaction", State = 1 },
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
                    new AndRuleGroup() { Id = 2, RuleGroupId = 2, State = 1 }, // (PartnerName == "Elemér") && (Sum < 2000)
                    new AndRuleGroup() { Id = 3, RuleGroupId = 2, State = 1 }, // (Currency == "EUR")
                    // Rule#3
                    new AndRuleGroup() { Id = 4, RuleGroupId = 3, State = 1 }, // (Type == "food")
                }
            },
            // Rule
            {
                nameof(Rule),
                new List<Rule>
                {
                    // Rule#1
                    new Rule() { Id = 1, AndRuleGroupId = 1, State = 1, Property = "Type", Value = "Kamat", RuleType = RuleTypeEnum.IsEqualTo },
                    // Rule#2
                    new Rule() { Id = 2, AndRuleGroupId = 2, State = 1, Property = "PartnerName", Value = "Elemér", RuleType = RuleTypeEnum.IsEqualTo }, // PartnerName == "Elemér"
                    new Rule() { Id = 3, AndRuleGroupId = 2, State = 1, Property = "Sum", Value = "2000", RuleType = RuleTypeEnum.IsLesserThan }, // Sum < 2000
                    new Rule() { Id = 4, AndRuleGroupId = 3, State = 1, Property = "Currency", Value = "EUR", RuleType = RuleTypeEnum.IsEqualTo }, // Currency == "EUR"
                    // Rule#3
                    new Rule() { Id = 5, AndRuleGroupId = 4, State = 1, Property = "Type", Value = "food", RuleType = RuleTypeEnum.IsEqualTo }, // Type == "food"
                }
            },
            // RuleAction
            {
                nameof(RuleAction),
                new List<RuleAction>
                {
                    // Rule#1
                    new RuleAction() { Id = 1, RuleGroupId = 1, Property = null, Value = null, TagId = 1, State = 1, RuleActionType = RuleActionTypeEnum.AddTag },
                    new RuleAction() { Id = 2, RuleGroupId = 1, Property = null, Value = null, TagId = 2, State = 1, RuleActionType = RuleActionTypeEnum.AddTag },
                    // Rule#2
                    new RuleAction() { Id = 3, RuleGroupId = 2, Property = "Title", Value = "Elemér < 2000 | EUR", TagId = null, State = 1, RuleActionType = RuleActionTypeEnum.SetValueOfProperty },
                    // Rule#3
                    new RuleAction() { Id = 4, RuleGroupId = 3, Property = null, Value = null, TagId = null, State = 1, RuleActionType = RuleActionTypeEnum.AggregateToMonthlyTransaction },
                }
            },
            // BankRow
            {
                nameof(BankRow),
                new List<BankRow>
                {
                    // Circular reference example
                    new BankRow() { Id = 1, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular1", State = 1 },
                    new BankRow() { Id = 2, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Circular2", State = 1 },
                    // Grouped example
                    new BankRow() { Id = 3, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 4, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 5, IsTransactionCreated = true, GroupedTransactionId = 3, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group1", State = 1 },
                    new BankRow() { Id = 6, IsTransactionCreated = true, GroupedTransactionId = 4, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 0, Currency = null, Message = "Group2", State = 1 },
                    // Rule#1 valid
                    new BankRow() { Id = 7, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Kamat", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = "HU68120527050013209800100008", PartnerName = "Generali Biztosító Zrt.", Sum = -11880, Currency = "HUF", Message = "225534585, rendszeres díj", State = 1 },
                    new BankRow() { Id = 8, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "440602******4503", Type = "Kamat", Account = "104040657157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = "SZENTMIHALYI UT 131.", Sum = -20000, Currency = "HUF", Message = null, State = 1 },
                    new BankRow() { Id = 9, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(2016, 5, 27), BankTransactionId = "099960527H402381", Type = "Kamat", Account = "104019457157575649481012", AccountName = "SZŐCS ÁRON", PartnerAccount = null, PartnerName = null, Sum = -90, Currency = "HUF", Message = null, State = 1 },
                    // Rule#2 tests (1 validates, 0 does not)
                    /*1*/ new BankRow() { Id = 10, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 100, Currency = null, Message = null, State = 1 },
                    /*1*/ new BankRow() { Id = 11, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Zsolt", Sum = 250000, Currency = "EUR", Message = null, State = 1 },
                    /*0*/ new BankRow() { Id = 12, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Zsolt", Sum = 100, Currency = null, Message = null, State = 1 },
                    /*1*/ new BankRow() { Id = 13, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 200000, Currency = "EUR", Message = null, State = 1 },
                    /*0*/ new BankRow() { Id = 14, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = "Elemér", Sum = 200000, Currency = null, Message = null, State = 1 },
                    // Rule#3
                    new BankRow() { Id = 15, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 21), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 100, Currency = null, Message = null, State = 1 },
                    new BankRow() { Id = 16, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 3, 11), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 20, Currency = null, Message = null, State = 1 },
                    new BankRow() { Id = 17, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 16), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 20, Currency = null, Message = null, State = 1 },
                    new BankRow() { Id = 18, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 1, 1), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 3, Currency = null, Message = null, State = 1 },
                    new BankRow() { Id = 19, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 3, 11), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 2, Currency = null, Message = null, State = 1 },
                    new BankRow() { Id = 20, IsTransactionCreated = true, GroupedTransactionId = null, AccountingDate = new DateTime(1999, 9, 20), BankTransactionId = null, Type = "food", Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 111, Currency = null, Message = null, State = 1 },
                }
            },
            // Transaction
            {
                nameof(Transaction),
                new List<Transaction>
                {
                    // Circular reference example
                    new Transaction() { Id = 1, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = 1, State = 1 },
                    new Transaction() { Id = 2, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = 2, State = 1 },
                    // Rule#1 valid
                    new Transaction() { Id = 3, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = null, State = 1 },
                    new Transaction() { Id = 4, Title = "", Description = "", Date = new DateTime(1999, 1, 1), Sum = 0, IsCustom = false, BankRowId = null, State = 1 },
                }
            },
        };
    }
}
