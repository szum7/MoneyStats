using MoneyStats.BL.Common;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;

namespace MoneyStats.BL.Modules
{
    public class RuleEvaluator
    {
        static bool IsConditionTrue(Condition condition, object rowValue)
        {
            if (condition.ConditionType == ConditionType.TrueRule)
            {
                return true;
            }
            else if (condition.ConditionType == ConditionType.IsEqualTo)
            {
                return (rowValue?.ToString() == condition.Value);
            }
            else if (condition.ConditionType == ConditionType.IsGreaterThan)
            {
                var convertedValue = (IComparable)Convert.ChangeType(condition.Value, rowValue.GetType());
                return Utilities.Compare(convertedValue, "<", (IComparable)rowValue);
            }
            else if (condition.ConditionType == ConditionType.IsLesserThan)
            {
                var convertedValue = (IComparable)Convert.ChangeType(condition.Value, rowValue.GetType());
                return Utilities.Compare(convertedValue, ">", (IComparable)rowValue);
            }
            else if (condition.ConditionType == ConditionType.IsPropertyNull)
            {
                return (rowValue == null);
            }
            else if (condition.ConditionType == ConditionType.IsPropertyNotNull)
            {
                return (rowValue != null);
            }
            else if (condition.ConditionType == ConditionType.ContainsValueOfProperty)
            {
                return rowValue.ToString().Contains(condition.Value);
            }

            throw new Exception($"Unexpected RuleTypeId: {condition.ConditionType}");
        }

        public static bool CheckIfBankRowValidatesToRule(Rule rule, BankRow bankRow)
        {
            var i = 0;
            var oneOrRulesValidate = false;
            while (i < rule.AndConditionGroups.Count && !oneOrRulesValidate)
            {
                var andRule = rule.AndConditionGroups[i];  // = (a & b & c)
                var allAndRulesValidate = true;

                if (andRule.Conditions.Count > 0)
                {
                    var j = 0;
                    while (j < andRule.Conditions.Count && allAndRulesValidate)
                    {
                        var condition = andRule.Conditions[j]; // = a
                        var rowValue = typeof(BankRow).GetProperty(condition.Property).GetValue(bankRow);

                        allAndRulesValidate = RuleEvaluator.IsConditionTrue(condition, rowValue);

                        j++;
                    }

                    if (allAndRulesValidate)
                    {
                        oneOrRulesValidate = true;
                    }
                }
                else
                {
                    throw new Exception("AndCondtionGroups can NOT be without at least one condition!");
                }

                i++;
            }
            return oneOrRulesValidate;
        }
    }
}
