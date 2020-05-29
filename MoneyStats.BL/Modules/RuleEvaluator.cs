using MoneyStats.BL.Common;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Modules
{
    public class RuleEvaluator
    {
        public List<BankRow> BankRows { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<TransactionCreatedWithRule> TransactionCreatedWithRules { get; set; }

        public RuleEvaluator()
        {
            this.Transactions = new List<Transaction>();
            this.TransactionCreatedWithRules = new List<TransactionCreatedWithRule>();
        }

        public void Reset()
        {
            this.BankRows = new List<BankRow>();
            this.Transactions = new List<Transaction>();
            this.TransactionCreatedWithRules = new List<TransactionCreatedWithRule>();
        }

        List<RuleGroup> GetSortedRuleGroups(List<RuleGroup> ruleGroups)
        {
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
            return sortedArray.ToList();
        }

        Transaction GetNewTransaction(BankRow bankRow)
        {
            var item = new Transaction()
            {
                BankRowId = bankRow.Id,
                Date = bankRow.AccountingDate,
                Sum = bankRow.Sum
            };
            item.SetNew();
            return item;
        }

        bool IsRuleTrue(Rule rule, object rowValue)
        {
            if (rule.RuleTypeId == (int)RuleTypeEnum.TrueRule)
            {
                return true;
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsEqualTo)
            {
                return (rowValue?.ToString() == rule.Value);
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsGreaterThan)
            {
                var convertedValue = (IComparable)Convert.ChangeType(rule.Value, rowValue.GetType());
                return RuleRepository.Compare(convertedValue, "<", (IComparable)rowValue);
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsLesserThan)
            {
                var convertedValue = (IComparable)Convert.ChangeType(rule.Value, rowValue.GetType());
                return RuleRepository.Compare(convertedValue, ">", (IComparable)rowValue);
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsPropertyNull)
            {
                return (rowValue == null);
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.IsPropertyNotNull)
            {
                return (rowValue != null);
            }
            else if (rule.RuleTypeId == (int)RuleTypeEnum.ContainsValueOfProperty)
            {
                return rowValue.ToString().Contains(rule.Value);
            }
            throw new Exception($"Unexpected RuleTypeId: {rule.RuleTypeId}");
        }

        bool CheckIfBankRowValidatesToRuleGroup(RuleGroup ruleGroup, BankRow bankRow)
        {         
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
                    var rowValue = typeof(BankRow).GetProperty(rule.Property).GetValue(bankRow);

                    allAndRulesValidate = this.IsRuleTrue(rule, rowValue);

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

        TransactionCreatedWithRule GetNewTransactionCreatedWithRule(RuleGroup ruleGroup, Transaction transaction)
        {
            var item = new TransactionCreatedWithRule()
            {
                RuleGroupId = ruleGroup.Id,
                Transaction = transaction // no transaction.Id yet.
            };
            item.SetNew();
            return item;
        }

        public void Run(List<RuleGroup> ruleGroups, List<BankRow> bankRows)
        {
            this.BankRows = bankRows;
            var aggregatedTransactions = new Dictionary<DateTime, Transaction>();

            ruleGroups = this.GetSortedRuleGroups(ruleGroups);

            foreach (var bankRow in bankRows)
            {
                Transaction transaction = this.GetNewTransaction(bankRow);
                Transactions.Add(transaction);

                foreach (RuleGroup ruleGroup in ruleGroups) // item = (a & b & c) || d || (e & f) => action
                {
                    if (ruleGroup.RuleActions.Count == 0)
                        throw new Exception("RuleGroup is without RuleActions!");

                    if(ruleGroup.RuleActions.Count(x => x.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction) > 1)
                        throw new Exception("You can not aggregate a BankRow to multiple transactions, that would multiply it's sum!");

                    var oneOrRulesValidate = this.CheckIfBankRowValidatesToRuleGroup(ruleGroup, bankRow); // = (a & b & c) || d || (e & f)

                    // Apply RuleAction if RuleGroup is valid
                    if (oneOrRulesValidate)
                    {
                        Transaction validTransaction = transaction;

                        if (ruleGroup.RuleActions[0].RuleActionTypeId == (int)RuleActionTypeEnum.Omit)
                        {
                            Transactions.Remove(validTransaction);
                            break;
                        }

                        // We need to evaluate the AggregateToMonthlyTransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to.
                        if (ruleGroup.RuleActions.Any(x => x.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction))
                        {
                            var month = new DateTime(bankRow.AccountingDate.Value.Year, bankRow.AccountingDate.Value.Month, 1);
                            Transaction monthlyTr = null;
                            if (!aggregatedTransactions.TryGetValue(month, out monthlyTr)) // New monthly aggregated transaction
                            {
                                monthlyTr = new Transaction()
                                {
                                    Date = month.AddMonths(1).AddDays(-1),
                                    Sum = 0
                                };
                                monthlyTr.SetNew();

                                aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                Transactions.Add(monthlyTr);

                                // Save the ruleGroup as reference
                                TransactionCreatedWithRules.Add(this.GetNewTransactionCreatedWithRule(ruleGroup, monthlyTr));
                            }
                            monthlyTr.Sum += transaction.Sum.Value;

                            // Remove the BankRow created Transaction
                            Transactions.Remove(transaction);
                            // Bring out the aggregated Transaction
                            validTransaction = monthlyTr;

                            // TODO!!! trg.Id must be saved but there are no tr.Ids yet. May need to run this whole AggregateToMonthlyTransaction type BEFORE other individual transaction saves. 
                            // It would be better though if this reference stayed the same and could modify the BankRows after the aggregated Transactions were saved.
                            bankRow.GroupedTransaction = monthlyTr; // no transaction.Id yet

                            // DoNotAddTransactionCreatedWithRules();
                        }
                        else
                        {
                            // Save the ruleGroup as reference
                            TransactionCreatedWithRules.Add(this.GetNewTransactionCreatedWithRule(ruleGroup, validTransaction));
                        }

                        foreach (RuleAction action in ruleGroup.RuleActions)
                        {
                            if (action.RuleActionTypeId == (int)RuleActionTypeEnum.AggregateToMonthlyTransaction)
                                continue;

                            switch (action.RuleActionTypeId)
                            {
                                case (int)RuleActionTypeEnum.SetValueOfProperty:

                                    validTransaction.SetPropertyValueFromString(action.Property, action.Value);
                                    break;

                                case (int)RuleActionTypeEnum.AddTag:

                                    validTransaction.Tags.Add(action.Tag);
                                    break;

                                default:
                                    throw new Exception($"RuleActionTypeId '{action.RuleActionTypeId}' is not recognized!");
                            }
                        }
                    }
                }
            }
        }

        public void SaveResultsToDatabase()
        {
            if (this.BankRows == null || 
                this.BankRows.Count == 0 || 
                this.Transactions.Count == 0)
            {
                Console.WriteLine("There's nothing to save!");
                return;
            }

            // save transactions
            new TransactionRepository().InsertRange(Transactions);

            foreach (var item in TransactionCreatedWithRules)
            {
                item.TransactionId = item.Transaction.Id;
                item.Transaction = null; // TODO investigate why this is needed
            }

            // save transactionCreatedWithRule
            new TransactionCreatedWithRuleRepository().InsertRange(TransactionCreatedWithRules);

            // update bankrows (GroupedTransactionId)
            new BankRowRepository().UpdateGroupedTransactionIds(BankRows);

            // save transactionTagConns
            new TransactionTagConnRepository().SaveTransactionTagConns(Transactions);
        }
    }
}
