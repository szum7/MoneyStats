namespace MoneyStats.DAL.Models
{
    public enum RuleTypeEnum
    {
        TrueRule = 1,
        IsEqualTo = 2, // can be used with strings, numbers, dates, etc.
        IsGreaterThan = 3, // can be used with IComperable
        IsLesserThan = 4, // can be used with IComperable
        IsPropertyNull = 5,
        IsPropertyNotNull = 6,
        ContainsValueOfProperty = 7 // property's value contains a string
    }
}
