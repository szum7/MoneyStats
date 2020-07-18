﻿using MoneyStats.BL.Common;
using MoneyStats.BL.Modules;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.BL.Interfaces
{
    public interface IGeneratedTransactionRepository
    {
        GenericResponse<bool> SaveAll(List<GeneratedTransaction> suggestedTransactions);

        List<GeneratedTransaction> Generate(List<Rule> rules, List<BankRow> bankRows);
    }
}