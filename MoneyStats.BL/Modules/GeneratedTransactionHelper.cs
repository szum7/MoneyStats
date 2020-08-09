using MoneyStats.BL.Common;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Modules
{
    public class GeneratedTransactionHelper
    {
        public GenericResponse<bool> SaveAll(List<GeneratedTransaction> generatedTransactions)
        {
            if (generatedTransactions == null || generatedTransactions.Count == 0)
            {
                return new ErrorResponse("There's nothing to save!").Pulse();
            }

            var transactions = new List<Transaction>();
            // TODO grouped bankRows and single bankRows are collected into one list for update.
            // grouped BankRows: GroupedTransactionId and IsTransactionCreated is changed
            // single BankRows: IsTransactionCreated is changed
            // Test if its okay (?)
            var bankRows = new List<BankRow>();
            var transactionTagConns = new List<TransactionTagConn>();

            foreach (var tr in generatedTransactions)
            {
                var transaction = new Transaction()
                {
                    Title = tr.Title,
                    Description = tr.Description,
                    Date = tr.Date,
                    Sum = tr.Sum,
                    IsCustom = tr.IsCustom,
                    AppliedRules = $"{string.Join(";", tr.AppliedRules)};"
                }.SetNew();

                if (!tr.IsCustom)
                {
                    bankRows.AddRange(this.GetBankRows(transaction, tr));
                }

                transactionTagConns.AddRange(this.GetTransactionTagConns(transaction, tr));

                transactions.Add(transaction);
            }

            // save transactions (transactions will have ids)
            new TransactionRepository().InsertRange(transactions);

            // save transactionTagConns
            this.InsertTransactionTagConns(transactionTagConns);

            // update bankrows (IsTransactionCreated [and GroupedTransactionId])
            this.UpdateBankRows(bankRows);

            return new SuccessResponse();
        }

        void UpdateBankRows(List<BankRow> bankRows)
        {
            bankRows.ForEach(x => {
                x.GroupedTransactionId = x.GroupedTransaction.Id;
                x.GroupedTransaction = null;
            });
            new BankRowRepository().UpdateMany(bankRows);
        }

        List<BankRow> GetBankRows(Transaction transaction, GeneratedTransaction tr)
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

        List<TransactionTagConn> GetTransactionTagConns(Transaction transaction, GeneratedTransaction tr)
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
