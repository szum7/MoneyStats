using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.ExampleData
{
    public static class TableDependencyOrder
    {
        public static readonly List<string> List = new List<string>()
        {
            nameof(RuleAction),
            nameof(TransactionTagConn),
            nameof(Tag),
            nameof(TransactionCreatedWithRule),
            nameof(Condition),
            nameof(AndConditionGroup),
            nameof(RulesetRuleConn),
            nameof(Ruleset),
            nameof(Rule),
            nameof(Transaction),
            nameof(BankRow)
        };
    }
}
