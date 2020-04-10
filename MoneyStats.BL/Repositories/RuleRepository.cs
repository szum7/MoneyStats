﻿using MoneyStats.DAL.Models;
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

    public enum RuleActionType
    {
        AddTags,
        AggregateToMonth
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

    public class TagToBeAttached
    {
        public Tag Tag { get; set; } // Only the Id and Name are not null
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string Self => $"[Tag with id:{Tag?.Id} and title '{Tag?.Title}']";
    }

    public class RuleRepository
    {
        private List<Tag> _tags { get; set; }
        private List<Tag> _Tags
        {
            get
            {
                if (_tags == null)
                {
                    var repo = new TagRepository();
                    _tags = repo.Get().ToList();
                }
                return _tags;
            }
        }

        public RuleRepository()
        {

        }

        public List<TagToBeAttached> ConvertToModel(string rule)
        {
            // TODO...
            return null;
        }

        public void AddTagsToTransaction(BankRow transaction, List<TagToBeAttached> tagToBeAttacheds)
        {
            // Check if tag ids exist
            var repo = new TagRepository();
            foreach (var tagToBeAttached in tagToBeAttacheds)
            {
                var ruleTag = tagToBeAttached.Tag;
                var tag = _Tags.SingleOrDefault(x => x.Id == ruleTag.Id || x.Title == ruleTag.Title);
                tagToBeAttached.IsValid = tag != null && tag.Id == ruleTag.Id && tag.Title == ruleTag.Title;

                if (!tagToBeAttached.IsValid)
                {
                    if (tag.Id != ruleTag.Id)
                    {
                        tagToBeAttached.Message = $"{tagToBeAttached.Self} found with a different id: {tag.Id}. Tag-transaction assossiation skipped.";
                    }
                    else if (tag.Title != ruleTag.Title)
                    {
                        tagToBeAttached.Message = $"{tagToBeAttached.Self} has a different title now: '{tag.Title}'. Tag-transaction assossiation skipped.";
                    }
                } 
                else
                {
                    tagToBeAttached.Message = $"{tagToBeAttached.Self} doesn't exists. Tag-transaction assossiation skipped.";
                }
            }

            // Create transaction-tag connection
            foreach (var tagToBeAttached in tagToBeAttacheds)
            {
                if (!tagToBeAttached.IsValid)
                    continue;

                // TODO...
            }
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

        // TODO this is for one rule but there are more than one rules!

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule">"compare;AccountingDate;2010-10-10 00:00:00;<="</param>
        /// <param name="transactions"></param>
        void Test(string rule, string ruleAction, List<BankRow> transactions)
        {
            foreach (BankRow transaction in transactions)
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
                            var currentValue = typeof(BankRow).GetProperty(currentRule.PropertyName).GetValue(transaction).ToString();
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

                            var currentValue = (IComparable)typeof(BankRow).GetProperty(currentRule.PropertyName).GetValue(transaction);
                            allANDRuleValidates = !RuleRepository.Compare(currentRule.Arguments, currentRule.Value as IComparable, currentValue);
                        }

                        j++;
                    }
                    if (allANDRuleValidates)
                    {
                        // => one part of the OR rules validates
                        // => current transaction validates
                        // => TODO evaluate ruleAction
                        transaction.EvaluatedRule = rule;
                        oneORRuleValidates = true;
                    }
                }
            }


            //var rulableProperties = (from property in typeof(Transaction).GetProperties()
            //                         where property.CustomAttributes.Any(customAttribute => customAttribute.AttributeType == typeof(Rulable))
            //                         select property).ToList();

            //foreach (PropertyInfo property in rulableProperties)
            //{
            //    var actualPropertyName = "AccountDate";
            //    var actualPropertyValue = DateTime.Now;
            //    var actualOperand = -1;

            //    var actualInstance = new Transaction();

            //    if (property.Name == "AccountingDate")
            //    {
            //        if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
            //        {
            //            if ((property.GetValue(actualInstance) as IComparable).CompareTo(actualPropertyValue) == actualOperand)
            //            {
            //                // => Megfelel a szabálynak
            //            }
            //        }
            //    }
            //}
        }
    }
}
