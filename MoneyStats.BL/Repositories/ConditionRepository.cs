using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MoneyStats.BL.Common;
using MoneyStats.BL.Interfaces;

namespace MoneyStats.BL.Repositories
{
    public class RuleEvaluationOutput
    {
        public List<Transaction> Transactions { get; set; }
        public List<TransactionCreatedWithRule> TransactionCreatedWithRules { get; set; }
        public List<TransactionTagConn> TransactionTagConns { get; set; }
    }

    public class ConditionRepository : EntityBaseRepository<Condition>, IConditionRepository
    {
        /// <summary>
        /// left op right => true/false
        /// 1 < 2 => true
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Compare(IComparable left, string op, IComparable right)
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

        /// <summary>
        /// BankRow list goes in, Transaction and TransactionTagConn list goes out.
        /// 1. 
        /// ...
        /// x. save Transactions
        /// y. create TransactionTagConn models based on Transactions now with ids and Tag ids from the tr.s' Tags lists.
        /// z. save TransactionTagConns
        /// ...
        /// </summary>
        /// <param name="rules">Needs to have connected entities in-depth loaded! (RuleActions, AndConditionGroups, AndConditionGroups.Rules, etc.)</param>
        /// <param name="bankRows"></param>
        [Obsolete("Spaghetti code for easier debugging. Do not use it!", false)]
        public RuleEvaluationOutput CreateTransactionUsingRulesFlattened(List<Rule> rules, List<BankRow> bankRows)
        {
            var transactions = new List<Transaction>();
            var aggregatedTransactions = new Dictionary<DateTime, Transaction>();
            var transactionTagConns = new List<TransactionTagConn>(); // TODO lehet nem fog kelleni és jobb ha a tr.Tags-be pakolok
            var transactionCreatedWithRules = new List<TransactionCreatedWithRule>();

            #region Sort Rules
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
            rules = sortedArray.ToList();
            #endregion

            foreach (var br in bankRows)
            {
                #region Add transaction
                // Create Transaction based on BankRow (TODO)
                var tr = new Transaction()
                {
                    BankRowId = br.Id,
                    Date = br.AccountingDate,
                    Sum = br.Sum
                }.SetNew();
                transactions.Add(tr);
                #endregion

                foreach (Rule rule in rules) // item = (a & b & c) || d || (e & f) => action
                {
                    // Check if Rule validates
                    var i = 0;
                    var oneOrRulesValidate = false;
                    while (i < rule.AndConditionGroups.Count && !oneOrRulesValidate)
                    {                        
                        var andCondition = rule.AndConditionGroups[i];  // = (a & b & c)
                        var allAndConditionsValidate = true;
                        var j = 0;
                        while (j < andCondition.Conditions.Count && allAndConditionsValidate)
                        {
                            var condition = andCondition.Conditions[j]; // = a
                            var rowValue = typeof(BankRow).GetProperty(condition.Property).GetValue(br);

                            if (condition.ConditionType == ConditionType.TrueRule)
                            {
                                allAndConditionsValidate = true;
                            }
                            else if (condition.ConditionType == ConditionType.IsEqualTo)
                            {
                                allAndConditionsValidate = (rowValue?.ToString() == condition.Value);
                            }
                            else if (condition.ConditionType == ConditionType.IsGreaterThan)
                            {
                                var convertedValue = (IComparable)Convert.ChangeType(condition.Value, rowValue.GetType());
                                allAndConditionsValidate = ConditionRepository.Compare(convertedValue, "<", (IComparable)rowValue);
                            }
                            else if (condition.ConditionType == ConditionType.IsLesserThan)
                            {
                                var convertedValue = (IComparable)Convert.ChangeType(condition.Value, rowValue.GetType());
                                allAndConditionsValidate = ConditionRepository.Compare(convertedValue, ">", (IComparable)rowValue);
                            }
                            else if (condition.ConditionType == ConditionType.IsPropertyNull)
                            {
                                allAndConditionsValidate = (rowValue == null);
                            }
                            else if (condition.ConditionType == ConditionType.IsPropertyNotNull)
                            {
                                allAndConditionsValidate = (rowValue != null);
                            }
                            else if (condition.ConditionType == ConditionType.ContainsValueOfProperty)
                            {
                                allAndConditionsValidate = rowValue.ToString().Contains(condition.Value);
                            }
                            else
                            {
                                Console.WriteLine($"Unexpected RuleTypeId: {condition.ConditionType}");
                            }

                            j++;
                        }

                        if (allAndConditionsValidate)
                        {
                            oneOrRulesValidate = true;
                        }

                        i++;
                    }

                    // Apply RuleAction if Rule is valid
                    if (oneOrRulesValidate)
                    {
                        Transaction ruleTr = tr;

                        if (rule.RuleActions.Any(x => x.RuleActionType == RuleActionType.Omit))
                        {
                            transactions.Remove(ruleTr);
                            break;
                        }
                        if (rule.RuleActions.Count(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction) > 1)
                        {
                            throw new Exception("You can't have more than one aggregating action applied to a Transaction!");
                        }

                        // We need to evaluate the AggregateToMonthlyTransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to.
                        var aggregatingAction = rule.RuleActions
                            .Where(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction)
                            .ToList();
                        if (aggregatingAction.Count > 1)
                        {
                            throw new Exception("You can not aggregate a BankRow to multiple transactions, that would multiply it's sum!");
                        } 
                        else if (aggregatingAction.Count == 1)
                        {
                            var month = new DateTime(br.AccountingDate.Value.Year, br.AccountingDate.Value.Month, 1);
                            Transaction monthlyTr = null;
                            if (!aggregatedTransactions.TryGetValue(month, out monthlyTr)) // New monthly aggregated transaction
                            {
                                monthlyTr = new Transaction()
                                {
                                    Date = month.AddMonths(1).AddDays(-1),
                                    Sum = 0
                                }.SetNew();

                                aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                transactions.Add(monthlyTr);

                                // Save the Rule as reference
                                transactionCreatedWithRules.Add(new TransactionCreatedWithRule()
                                {
                                    RuleId = rule.Id,
                                    Transaction = monthlyTr // no transaction.Id yet.
                                }.SetNew());
                            }
                            monthlyTr.Sum += tr.Sum.Value;

                            // Remove the BankRow created Transaction
                            transactions.Remove(tr);
                            // Bring out the aggregated Transaction
                            ruleTr = monthlyTr;

                            // TODO!!! trg.Id must be saved but there are no tr.Ids yet. May need to run this whole AggregateToMonthlyTransaction type BEFORE other individual transaction saves. 
                            // It would be better though if this reference stayed the same and could modify the BankRows after the aggregated Transactions were saved.
                            br.GroupedTransaction = monthlyTr; // no transaction.Id yet

                            // DoNotAddTransactionCreatedWithRules();
                        }
                        else
                        {
                            // Save the Rule as reference
                            transactionCreatedWithRules.Add(new TransactionCreatedWithRule()
                            {
                                RuleId = rule.Id,
                                Transaction = ruleTr // no transaction.Id yet.
                            }.SetNew());
                        }

                        foreach (RuleAction action in rule.RuleActions)
                        {
                            if (action.RuleActionType == RuleActionType.AggregateToMonthlyTransaction)
                                continue;

                            switch (action.RuleActionType)
                            {
                                case RuleActionType.SetValueOfProperty:

                                    ruleTr.SetPropertyValueFromString(action.Property, action.Value);

                                    break;
                                case RuleActionType.AddTag:

                                    ruleTr.Tags.Add(action.Tag);

                                    break;
                                default:
                                    throw new Exception($"RuleActionTypeId '{action.RuleActionType}' is not recognized!");
                            }
                        }
                    }
                }
            }

            // save transactions
            new TransactionRepository().InsertRange(transactions);

            foreach (var item in transactionCreatedWithRules)
            {
                item.TransactionId = item.Transaction.Id;
                item.Transaction = null; // TODO investigate why this is needed
            }

            // save transactionCreatedWithRule
            new TransactionCreatedWithRuleRepository().InsertRange(transactionCreatedWithRules);

            // update bankrows (GroupedTransactionId)
            new BankRowRepository().UpdateGroupedTransactionIds(bankRows);

            // save transactionTagConns
            new TransactionTagConnRepository().SaveTransactionTagConns(transactions);

            // TODO this is not the final version, saves could be inside the method or results passed in another way
            return new RuleEvaluationOutput()
            {
                Transactions = transactions,
                TransactionCreatedWithRules = transactionCreatedWithRules,
                TransactionTagConns = transactionTagConns
            };
        }
    }
}
