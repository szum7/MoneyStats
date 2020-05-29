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

    public class RuleRepository : EntityBaseRepository<Rule>, IRuleRepository
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
        /// <param name="ruleGroups">Needs to have connected entities in-depth loaded! (RuleActions, AndRuleGroups, AndRuleGroups.Rules, etc.)</param>
        /// <param name="bankRows"></param>
        [Obsolete("Spaghetti code for easier debugging. Do not use it!", false)]
        public RuleEvaluationOutput CreateTransactionUsingRulesFlattened(List<RuleGroup> ruleGroups, List<BankRow> bankRows)
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
                    var oneOrRulesValidate = false;
                    while (i < ruleGroup.AndRuleGroups.Count && !oneOrRulesValidate)
                    {                        
                        var andRule = ruleGroup.AndRuleGroups[i];  // = (a & b & c)
                        var allAndRulesValidate = true;
                        var j = 0;
                        while (j < andRule.Rules.Count && allAndRulesValidate)
                        {
                            var rule = andRule.Rules[j]; // = a
                            var rowValue = typeof(BankRow).GetProperty(rule.Property).GetValue(br);

                            if (rule.RuleTypeId == (int)RuleTypeEnum.TrueRule)
                            {
                                allAndRulesValidate = true;
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsEqualTo)
                            {
                                allAndRulesValidate = (rowValue?.ToString() == rule.Value);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsGreaterThan)
                            {
                                var convertedValue = (IComparable)Convert.ChangeType(rule.Value, rowValue.GetType());
                                allAndRulesValidate = RuleRepository.Compare(convertedValue, "<", (IComparable)rowValue);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsLesserThan)
                            {
                                var convertedValue = (IComparable)Convert.ChangeType(rule.Value, rowValue.GetType());
                                allAndRulesValidate = RuleRepository.Compare(convertedValue, ">", (IComparable)rowValue);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsPropertyNull)
                            {
                                allAndRulesValidate = (rowValue == null);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsPropertyNotNull)
                            {
                                allAndRulesValidate = (rowValue != null);
                            }
                            else if (rule.RuleTypeId == (int)RuleTypeEnum.ContainsValueOfProperty)
                            {
                                allAndRulesValidate = rowValue.ToString().Contains(rule.Value);
                            }
                            else
                            {
                                Console.WriteLine($"Unexpected RuleTypeId: {rule.RuleTypeId}");
                            }

                            j++;
                        }

                        if (allAndRulesValidate)
                        {
                            oneOrRulesValidate = true;
                        }

                        i++;
                    }

                    // Apply RuleAction if RuleGroup is valid
                    if (oneOrRulesValidate)
                    {
                        Transaction ruleTr = tr;

                        if (ruleGroup.RuleActions.Any(rule => rule.RuleActionTypeId == (int)RuleActionTypeEnum.Omit))
                        {
                            transactions.Remove(ruleTr);
                            break;
                        }
                        if (ruleGroup.RuleActions.Count(rule => rule.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction) > 1)
                        {
                            throw new Exception("You can't have more than one aggregating action applied to a Transaction!");
                        }

                        // We need to evaluate the AggregateToMonthlyTransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to.
                        var aggregatingAction = ruleGroup.RuleActions
                            .Where(x => x.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction)
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
                                    Sum = 0,
                                    CreateDate = DateTime.Now,
                                    State = 1
                                };

                                aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                transactions.Add(monthlyTr);

                                // Save the ruleGroup as reference
                                transactionCreatedWithRules.Add(new TransactionCreatedWithRule()
                                {
                                    RuleGroupId = ruleGroup.Id,
                                    Transaction = monthlyTr, // no transaction.Id yet.
                                    CreateDate = DateTime.Now,
                                    State = 1
                                });
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
                            // Save the ruleGroup as reference
                            transactionCreatedWithRules.Add(new TransactionCreatedWithRule()
                            {
                                RuleGroupId = ruleGroup.Id,
                                Transaction = ruleTr, // no transaction.Id yet.
                                CreateDate = DateTime.Now,
                                State = 1
                            });
                        }

                        foreach (RuleAction action in ruleGroup.RuleActions)
                        {
                            if (action.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction)
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
