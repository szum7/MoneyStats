namespace MoneyStats.DAL.Models
{
    public enum RuleActionType
    {
        Omit = 1,
        AddTag = 2,
        SetValueOfProperty = 3,
        AggregateToMonthlyTransaction = 4
    }
}