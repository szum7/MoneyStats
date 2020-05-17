using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.ExampleData
{
    public static class TableDependencyOrder
    {
        public static readonly List<string> List = new List<string>()
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
    }
}
