namespace MoneyStats.DAL.Models
{
    public enum RuleType
    {
        TrueRule,
        HasValueOfProperty,
        IsPropertyNull,
        IsPropertyNotNull,
        ContainsValueOfProperty // property's value contains a string
    }
}
