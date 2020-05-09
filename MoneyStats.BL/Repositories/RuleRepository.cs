using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MoneyStats.DAL.Common;
using MoneyStats.BL.Common;
using MoneyStats.BL.Interfaces;

namespace MoneyStats.BL.Repositories
{
    public class RuleRepository : EntityBaseRepository<Rule>, IRuleRepository
    {
        /// <summary>
        /// BankRow list goes in, Transaction and TransactionTagConn list goes out.
        /// 1. 
        /// ...
        /// x. save Transactions
        /// y. create TransactionTagConn models based on Transactions now with ids and Tag ids from the tr.s' Tags lists.
        /// z. save TransactionTagConns
        /// ...
        /// </summary>
        /// <param name="ruleGroups"></param>
        /// <param name="transactions"></param>
        [Obsolete("Spaghetti code for easier debugging. Do not use it!", false)]
        public void CreateTransactionUsingRulesFlattened(List<RuleGroup> ruleGroups, List<BankRow> bankRows)
        {
            var transactions = new List<Transaction>();
            var aggregatedTransactions = new Dictionary<DateTime, Transaction>();
            var transactionTagConns = new List<TransactionTagConn>(); // TODO lehet nem fog kelleni és jobb ha a tr.Tags-be pakolok
            var transactionCreatedWithRules = new List<TransactionCreatedWithRule>();

            #region Sort ruleGroups
            // Sort ruleGroups based on Omit actions. 
            // Rules with Omit actions need to run first (break from iteration)
            int start = 0;
            int end = ruleGroups.Count - 1;
            var sortedArray = new RuleGroup[ruleGroups.Count];
            foreach (var rule in ruleGroups)
            {
                if (rule.RuleActions.Any(action => action.RuleActionTypeId == (int)RuleActionTypeEnum.Omit))
                {
                    if (rule.RuleActions.Count > 1)
                    {
                        throw new Exception("Invalid rule! Rules with an Omit action must not have other actions to perform!");
                    }

                    sortedArray[start] = rule;
                    start++;
                } 
                else
                {
                    sortedArray[end] = rule;
                    end--;
                }
            }
            ruleGroups = sortedArray.ToList();
            #endregion

            foreach (var br in bankRows)
            {
                #region Add transaction
                // Create Transaction based on BankRow (TODO)
                var tr = new Transaction()
                {
                    BankRowId = br.Id,
                    Date = br.AccountingDate,
                    Sum = br.Sum,
                    CreateDate = DateTime.Now,
                    State = 1
                };
                transactions.Add(tr);
                #endregion

                foreach (RuleGroup ruleGroup in ruleGroups) // item = (a & b & c) || d || (e & f) => action
                {
                    // Check if RuleGroup validates
                    var i = 0;
                    var oneOrRuleValidates = false;
                    while (i < ruleGroup.AndRuleGroups.Count && !oneOrRuleValidates)
                    {                        
                        var andRule = ruleGroup.AndRuleGroups[i];  // = (a & b & c)
                        var allAndRuleValidates = true;
                        var j = 0;
                        while (j < andRule.Rules.Count && allAndRuleValidates)
                        {
                            var rule = andRule.Rules[j]; // = a
                            
                            if (rule.RuleTypeId == (int)RuleTypeEnum.TrueRule)
                            {
                                // Do nothing
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsGreaterThan)
                            {
                                var value = (IComparable)typeof(BankRow).GetProperty(rule.Property).GetValue(br);
                                allAndRuleValidates = !RuleRepositoryV1.Compare("<", rule.Value as IComparable, value);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsLesserThan)
                            {
                                var value = (IComparable)typeof(BankRow).GetProperty(rule.Property).GetValue(br);
                                allAndRuleValidates = !RuleRepositoryV1.Compare("<", rule.Value as IComparable, value);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsEqualTo)
                            {
                                var value = (IComparable)typeof(BankRow).GetProperty(rule.Property).GetValue(br);
                                allAndRuleValidates = (value == (IComparable)rule.Value);
                            }

                            j++;
                        }

                        if (allAndRuleValidates)
                        {
                            oneOrRuleValidates = true;
                        }

                        i++;
                    }

                    // Apply RuleAction if RuleGroup validated
                    if (oneOrRuleValidates)
                    {
                        Transaction ruleTr = tr;

                        if (ruleGroup.RuleActions.Any(rule => rule.RuleActionTypeId == (int)RuleActionTypeEnum.Omit))
                        {
                            transactions.Remove(ruleTr);
                            break;
                        }
                        if (ruleGroup.RuleActions.Count(rule => rule.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToATransaction) > 1)
                        {
                            throw new Exception("You can't have more than one aggregating action applied to a Transaction!");
                        }

                        // We need to evaluate the AggregateToATransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to.
                        foreach (RuleAction action in ruleGroup.RuleActions)
                        {
                            if (action.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToATransaction)
                            {
                                var month = new DateTime(br.AccountingDate.Value.Year, br.AccountingDate.Value.Month, 1);
                                Transaction monthlyTr = null;
                                if (!aggregatedTransactions.TryGetValue(month, out monthlyTr))
                                {
                                    monthlyTr = new Transaction()
                                    {
                                        Date = month.AddMonths(1).AddDays(-1),
                                        Sum = 0
                                    };

                                    aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                    transactions.Add(monthlyTr);
                                }
                                monthlyTr.Sum += tr.Sum.Value;

                                // Remove the BankRow created Transaction
                                transactions.Remove(tr);
                                // Bring out the aggregated Transaction
                                ruleTr = monthlyTr;

                                // TODO!!! trg.Id must be saved but there are no tr.Ids yet. May need to run this whole AggregateToATransaction type BEFORE other individual transaction saves. 
                                // It would be better though if this reference stayed the same and could modify the BankRows after the aggregated Transactions were saved.
                                br.GroupedTransaction = monthlyTr; // no transaction.Id yet
                            }
                        }

                        foreach (RuleAction action in ruleGroup.RuleActions)
                        {
                            if (action.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToATransaction)
                                continue;

                            switch (action.RuleActionTypeId)
                            {
                                case (int)RuleActionTypeEnum.SetValueOfProperty:

                                    ruleTr.SetPropertyValueFromString(action.Property, action.Value);

                                    break;
                                case (int)RuleActionTypeEnum.AddTag:

                                    ruleTr.Tags.Add(action.Tag);

                                    break;
                                default:
                                    throw new Exception($"RuleActionTypeId '{action.RuleActionTypeId}' is not recognized!");
                            }
                        }

                        // Save the ruleGroup as reference
                        transactionCreatedWithRules.Add(new TransactionCreatedWithRule()
                        {
                            RuleGroupId = ruleGroup.Id,
                            Transaction = ruleTr // no transaction.Id yet.
                        });
                    }
                }
            }

            // save transactions

            // save transactionCreatedWithRule

            // update bankrows (TransactionGroupId)

            // save transactionTagConns
        }
    }

    #region Version 1
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

    public class RuleV1
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

    public class RuleRepositoryV1
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
                        var currentRule = new RuleV1()
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
                            allANDRuleValidates = !RuleRepositoryV1.Compare(currentRule.Arguments, currentRule.Value as IComparable, currentValue);
                        }

                        j++;
                    }
                    if (allANDRuleValidates)
                    {
                        // => one part of the OR rules validates
                        // => current transaction validates

                        // => TODO apply ruleAction to transaction!

                        oneORRuleValidates = true; // we can stop iteration OR parts of the rule. One OR part passed, and that's enough for us.
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
    #endregion
}
