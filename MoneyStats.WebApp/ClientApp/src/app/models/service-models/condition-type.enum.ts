export enum ConditionType {
    Unset = 0,
    TrueRule = 1,
    IsEqualTo = 2,
    IsGreaterThan = 3,
    IsLesserThan = 4,
    IsPropertyNull = 5,
    IsPropertyNotNull = 6,
    ContainsValueOfProperty = 7 // property's value contains a string
}
