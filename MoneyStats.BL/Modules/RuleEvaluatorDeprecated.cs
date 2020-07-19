using MoneyStats.BL.Common;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL.Modules
{
    public class RuleEvaluatorDeprecated
    {
        public List<BankRow> BankRows { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<TransactionCreatedWithRule> TransactionCreatedWithRules { get; set; }

        public RuleEvaluatorDeprecated()
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

        Transaction GetNewTransaction(BankRow bankRow)
        {
            var item = new Transaction()
            {
                BankRowId = bankRow.Id,
                Date = bankRow.AccountingDate,
                Sum = bankRow.Sum
            }.SetNew();
            return item;
        }

        bool IsConditionTrue(Condition condition, object rowValue)
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

        bool CheckIfBankRowValidatesToRule(Rule rule, BankRow bankRow)
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

                    allAndRulesValidate = this.IsConditionTrue(condition, rowValue);

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

        TransactionCreatedWithRule GetNewTransactionCreatedWithRule(Rule rule, Transaction transaction)
        {
            var item = new TransactionCreatedWithRule()
            {
                Rule = rule,
                RuleId = rule.Id,
                Transaction = transaction // no transaction.Id yet.
            }.SetNew();
            return item;
        }

        public void Run(List<Rule> rules, List<BankRow> bankRows)
        {
            this.BankRows = bankRows;
            var aggregatedTransactions = new Dictionary<DateTime, Transaction>();

            rules = this.GetSortedRules(rules);

            foreach (var bankRow in bankRows)
            {
                Transaction transaction = this.GetNewTransaction(bankRow);
                Transactions.Add(transaction);

                foreach (Rule rule in rules) // item = (a & b & c) || d || (e & f) => action
                {
                    if (rule.RuleActions.Count == 0)
                        throw new Exception("Rule is without RuleActions!");

                    if(rule.RuleActions.Count(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction) > 1)
                        throw new Exception("You can not aggregate a BankRow to multiple transactions, that would multiply it's sum!");

                    var oneOrRulesValidate = this.CheckIfBankRowValidatesToRule(rule, bankRow); // = (a & b & c) || d || (e & f)

                    // Apply RuleAction if Rule is valid
                    if (oneOrRulesValidate)
                    {
                        Transaction validTransaction = transaction;

                        if (rule.RuleActions[0].RuleActionType == RuleActionType.Omit)
                        {
                            Transactions.Remove(validTransaction);
                            break;
                        }

                        // We need to evaluate the AggregateToMonthlyTransaction 
                        // type FIRST and use the aggr.ed Transaction to 
                        // apply the rest of the actions to.
                        if (rule.RuleActions.Any(x => x.RuleActionType == RuleActionType.AggregateToMonthlyTransaction))
                        {
                            var month = new DateTime(bankRow.AccountingDate.Value.Year, bankRow.AccountingDate.Value.Month, 1);
                            Transaction monthlyTr = null;
                            if (!aggregatedTransactions.TryGetValue(month, out monthlyTr)) // New monthly aggregated transaction
                            {
                                monthlyTr = new Transaction()
                                {
                                    Date = month.AddMonths(1).AddDays(-1),
                                    Sum = 0
                                }.SetNew();

                                aggregatedTransactions.Add(month, monthlyTr); // have a separate list for better performance
                                Transactions.Add(monthlyTr);

                                // Save the Rule as reference
                                TransactionCreatedWithRules.Add(this.GetNewTransactionCreatedWithRule(rule, monthlyTr));
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
                            // Save the Rule as reference
                            TransactionCreatedWithRules.Add(this.GetNewTransactionCreatedWithRule(rule, validTransaction));
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
                item.Rule = null; // TODO investigate why this is needed
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
