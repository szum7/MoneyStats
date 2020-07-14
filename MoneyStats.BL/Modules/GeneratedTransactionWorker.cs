using MoneyStats.BL.Common;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Modules
{
    public class SuggestedTransaction
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Sum { get; set; }

        public List<Tag> Tags { get; set; }
        public List<Rule> AppliedRules { get; set; }
        public BankRow BankRowReference { get; set; }
        public List<BankRow> AggregatedBankRowReferences { get; set; }

        public SuggestedTransaction()
        {
            this.Tags = new List<Tag>();
            this.AppliedRules = new List<Rule>();
            this.AggregatedBankRowReferences = new List<BankRow>();
        }
    }

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
                return ConditionRepository.Compare(convertedValue, "<", (IComparable)rowValue);
            }
            else if (condition.ConditionType == ConditionType.IsLesserThan)
            {
                var convertedValue = (IComparable)Convert.ChangeType(condition.Value, rowValue.GetType());
                return ConditionRepository.Compare(convertedValue, ">", (IComparable)rowValue);
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

                i++;
            }
            return oneOrRulesValidate;
        }
    }

    public class GeneratedTransactionWorker
    {
        public List<SuggestedTransaction> Get(List<Rule> rules, List<BankRow> bankRows)
        {
            var aggregatedTransactions = new Dictionary<DateTime, SuggestedTransaction>();
            var transactions = new List<SuggestedTransaction>();

            rules = this.GetSortedRules(rules);

            foreach (var bankRow in bankRows)
            {
                SuggestedTransaction transaction = this.GetSuggestedTransaction(bankRow);
                transactions.Add(transaction);

                foreach (Rule rule in rules) // item = (a & b & c) || d || (e & f) => action
                {
                    if (rule.RuleActions.Count == 0)
                        throw new Exception("Rule is without RuleActions!");

                    if (rule.RuleActions.Count(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction) > 1)
                        throw new Exception("You can not aggregate a BankRow to multiple transactions, that would multiply it's sum!");

                    var oneOrRulesValidate = RuleEvaluator.CheckIfBankRowValidatesToRule(rule, bankRow); // = (a & b & c) || d || (e & f)

                    // Apply RuleAction if Rule is valid
                    if (oneOrRulesValidate)
                    {
                        SuggestedTransaction validTransaction = transaction;

                        if (rule.RuleActions.Any(x => x.RuleActionType == RuleActionType.Omit))
                        {
                            transactions.Remove(validTransaction);
                            break;
                        }

                        // We need to evaluate the AggregateToMonthlyTransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to it.
                        if (rule.RuleActions.Any(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction))
                        {
                            var month = new DateTime(bankRow.AccountingDate.Value.Year, bankRow.AccountingDate.Value.Month, 1);
                            SuggestedTransaction monthlyTr = null;
                            if (!aggregatedTransactions.TryGetValue(month, out monthlyTr)) // New monthly aggregated transaction
                            {
                                monthlyTr = new SuggestedTransaction()
                                {
                                    Date = month.AddMonths(1).AddDays(-1),
                                    Sum = 0,
                                    BankRowReference = null,
                                    AppliedRules = new List<Rule>()
                                    {
                                        rule // A transaction can only be part of one aggregating rule
                                    }
                                };

                                aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                transactions.Add(monthlyTr);

                                // Save the Rule as reference
                                //TransactionCreatedWithRules.Add(this.GetNewTransactionCreatedWithRule(rule, monthlyTr));
                            }
                            monthlyTr.Sum += transaction.Sum.Value;

                            // Remove the BankRow created Transaction
                            transactions.Remove(transaction);
                            // Bring out the aggregated Transaction
                            validTransaction = monthlyTr;

                            monthlyTr.AggregatedBankRowReferences.Add(bankRow);
                        }
                        else
                        {
                            // Save the Rule as reference
                            validTransaction.AppliedRules.Add(rule);
                        }

                        foreach (RuleAction action in rule.RuleActions)
                        {
                            if (action.RuleActionType == RuleActionType.AggregateToMonthlyTransaction)
                                continue;

                            switch (action.RuleActionType)
                            {
                                case RuleActionType.SetValueOfProperty:

                                    validTransaction.SetPropertyValueFromString(action.Property, action.Value);
                                    break;

                                case RuleActionType.AddTag:

                                    validTransaction.Tags.Add(action.Tag);
                                    break;

                                default:
                                    throw new Exception($"RuleActionTypeId '{action.RuleActionType}' is not recognized!");
                            }
                        }
                    }
                }
            }
            return transactions;
        }

        List<Rule> GetSortedRules(List<Rule> rules)
        {
            // Sort Rules based on Omit actions. 
            // Rules with Omit actions need to run first (break from iteration)
            int start = 0;
            int end = rules.Count - 1;
            var sortedArray = new Rule[rules.Count];
            foreach (var rule in rules)
            {
                if (rule.RuleActions.Any(action => action.RuleActionType == RuleActionType.Omit))
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
            return sortedArray.ToList();
        }

        SuggestedTransaction GetSuggestedTransaction(BankRow bankRow)
        {
            var item = new SuggestedTransaction()
            {
                Date = bankRow.AccountingDate,
                Sum = bankRow.Sum,
                BankRowReference = bankRow
            };
            return item;
        }
    }
}
