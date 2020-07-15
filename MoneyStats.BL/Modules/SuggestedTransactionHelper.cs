﻿using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Modules
{
    public class SuggestedTransactionHelper
    {
        public void Save(List<SuggestedTransaction> suggestedTransactions)
        {
            if (suggestedTransactions == null ||
                suggestedTransactions.Count == 0)
            {
                Console.WriteLine("There's nothing to save!");
                return;
            }

            var transactions = new List<Transaction>();
            var transactionCreatedWithRules = new List<TransactionCreatedWithRule>();
            // TODO grouped bankRows and single bankRows are collected into one list for update.
            // grouped BankRows: GroupedTransactionId and IsTransactionCreated is changed
            // single BankRows: IsTransactionCreated is changed
            var bankRows = new List<BankRow>();
            var transactionTagConns = new List<TransactionTagConn>();

            foreach (var tr in suggestedTransactions)
            {
                var transaction = new Transaction()
                {
                    Title = tr.Title,
                    Description = tr.Description,
                    Date = tr.Date,
                    Sum = tr.Sum,
                    IsCustom = tr.IsCustom
                }.SetNew();

                if (!tr.IsCustom)
                {
                    bankRows.AddRange(this.GetBankRows(transaction, tr));
                }

                transactionTagConns.AddRange(this.GetTransactionTagConns(transaction, tr));

                transactionCreatedWithRules.AddRange(this.GetTransactionCreatedWithRules(transaction, tr));

                transactions.Add(transaction);
            }

            // save transactions (transactions will have ids)
            new TransactionRepository().InsertRange(transactions);

            // save transactionTagConns
            this.InsertTransactionTagConns(transactionTagConns);

            // save transactionCreatedWithRule
            this.InsertTransactionCreatedWithRules(transactionCreatedWithRules);

            // update bankrows (IsTransactionCreated [and GroupedTransactionId])
            this.UpdateBankRows(bankRows);
        }

        void UpdateBankRows(List<BankRow> bankRows)
        {
            bankRows.ForEach(x => {
                x.GroupedTransactionId = x.GroupedTransaction.Id;
                x.GroupedTransaction = null;
            });
            new BankRowRepository().UpdateMany(bankRows);
        }

        List<BankRow> GetBankRows(Transaction transaction, SuggestedTransaction tr)
        {
            var bankRows = new List<BankRow>();

            if (tr.AggregatedBankRowReferences.Count > 0)
            {
                foreach (var bankRow in tr.AggregatedBankRowReferences)
                {
                    bankRow.GroupedTransaction = transaction;
                    bankRow.IsTransactionCreated = true;
                    bankRows.Add(bankRow);
                }
            }
            else
            {
                transaction.BankRowId = tr.BankRowReference.Id;
                tr.BankRowReference.IsTransactionCreated = true;
                bankRows.Add(tr.BankRowReference);
            }

            return bankRows;
        }

        List<TransactionTagConn> GetTransactionTagConns(Transaction transaction, SuggestedTransaction tr)
        {
            var transactionTagConns = new List<TransactionTagConn>();

            foreach (var t in tr.Tags)
            {
                var transactionTagConn = new TransactionTagConn()
                {
                    TagId = t.Id,
                    Transaction = transaction
                }.SetNew();
                transactionTagConns.Add(transactionTagConn);
            }

            return transactionTagConns;
        }

        List<TransactionCreatedWithRule> GetTransactionCreatedWithRules(Transaction transaction, SuggestedTransaction tr)
        {
            var transactionCreatedWithRules = new List<TransactionCreatedWithRule>();

            foreach (var r in tr.AppliedRules)
            {
                var transactionCreatedWithRule = new TransactionCreatedWithRule()
                {
                    RuleId = r.Id,
                    Transaction = transaction
                }.SetNew();
                transactionCreatedWithRules.Add(transactionCreatedWithRule);
            }

            return transactionCreatedWithRules;
        }

        void InsertTransactionCreatedWithRules(List<TransactionCreatedWithRule> transactionCreatedWithRules)
        {
            transactionCreatedWithRules.ForEach(x => {
                x.TransactionId = x.Transaction.Id;
                x.Transaction = null;
            });
            new TransactionCreatedWithRuleRepository().InsertRange(transactionCreatedWithRules);
        }

        void InsertTransactionTagConns(List<TransactionTagConn> transactionTagConns)
        {
            transactionTagConns.ForEach(x => {
                x.TransactionId = x.Transaction.Id;
                x.Transaction = null;
            });
            new TransactionTagConnRepository().InsertRange(transactionTagConns);
        }
    }
}
