using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MoneyStats.DAL.Common;

namespace MoneyStats.BL.Repositories
{
    /// <summary>
    /// This is not yet useful as operands are created on client side and stored as string.
    /// Using this enum would mean extra conversions.
    /// </summary>
    public enum RuleType
    {
        Compare, // Operator
        Contain // String.Contains
    }

    // Examples:
    // AccountingDate < 2014-01-01 00:00:00
    // TransactionId.ToLower().Contains("banki")


    // Special characters: 
    // ,  -> &44
    // &  -> &38
    // || -> &124&124

    // Rule examples:
    // contains;TransactionId;Hello&44 my friend!;caseSensitive&&contains;Message;15HUF;caseInsensitive
    // contains;TransactionId;Hello&44 my friend!;caseInsensitive

    public class Rule
    {
        public string Type { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public string Arguments { get; set; }
    }

    public class RuleRepository
    {
        public RuleRepository()
        {

        }

        void HandleExceptions()
        {

        }

        public static bool Compare(string op, IComparable left, IComparable right)
        {
            switch (op)
            {
                case "<": return left.CompareTo(right) < 0;
                case ">": return left.CompareTo(right) > 0;
                case "<=": return left.CompareTo(right) <= 0;
                case ">=": return left.CompareTo(right) >= 0;
                case "==": return left.Equals(right);
                case "!=": return !left.Equals(right);
                default: throw new ArgumentException("Invalid comparison operator: {0}", op);
            }
        }

        void Test(string rule, List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                var orRuleParts = rule.Split("||").ToList();
                var oneORRuleValidates = false;
                var i = 0;

                while (i < orRuleParts.Count && !oneORRuleValidates)
                {
                    var andRuleParts = orRuleParts[i].Split("&&").ToList();
                    var j = 0;
                    var allANDRuleValidates = true;

                    while (j < andRuleParts.Count && allANDRuleValidates)
                    {
                        // andRule = "contains;TransactionId;XYZ;caseInsensitive"
                        // andRule = "compare;AccountingDate;2010-10-10 00:00:00;<="

                        var andRuleArray = andRuleParts[j].Split(',').ToList();
                        var currentRule = new Rule()
                        {
                            Type = andRuleArray[0],
                            PropertyName = andRuleArray[1],
                            Value = andRuleArray[2],
                            Arguments = andRuleArray[3]
                        };


                        if (currentRule.Type == "Contains")
                        {
                            var currentValue = typeof(Transaction).GetProperty(currentRule.PropertyName).GetValue(transaction).ToString();
                            // TODO escape special characters
                            allANDRuleValidates = currentValue.Contains(currentRule.Value.ToString());
                        } 
                        else if (currentRule.Type == "Operand")
                        {
                            // compare;Property;value;<=;
                            // where Property <= value
                            // Misc.Compare(operand, value, Property.value) 
                            // => compare;property;50;<=
                            // => where 25 <= 50
                            // => Misc.Compare(<=, 50, 25) => true

                            var currentValue = (IComparable)typeof(Transaction).GetProperty(currentRule.PropertyName).GetValue(transaction);
                            allANDRuleValidates = !RuleRepository.Compare(currentRule.Arguments, currentRule.Value as IComparable, currentValue);
                        }

                        j++;
                    }
                    if (allANDRuleValidates)
                    {
                        // => one part of the OR rules validates
                        // => current transaction validates
                        oneORRuleValidates = true;
                    }
                }
            }


            var rulableProperties = (from property in typeof(Transaction).GetProperties()
                                     where property.CustomAttributes.Any(customAttribute => customAttribute.AttributeType == typeof(Rulable))
                                     select property).ToList();

            foreach (PropertyInfo property in rulableProperties)
            {
                var actualPropertyName = "AccountDate";
                var actualPropertyValue = DateTime.Now;
                var actualOperand = -1;

                var actualInstance = new Transaction();

                if (property.Name == "AccountingDate")
                {
                    if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                    {
                        if ((property.GetValue(actualInstance) as IComparable).CompareTo(actualPropertyValue) == actualOperand)
                        {
                            // => Megfelel a szabálynak
                        }
                    }
                }
            }
        }
    }
}
