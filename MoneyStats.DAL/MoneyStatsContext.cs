﻿using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Models;

namespace MoneyStats.DAL
{
    public class MoneyStatsContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BankRow> BankRows { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RuleAction> RuleActions { get; set; }
        public DbSet<AndConditionGroup> AndConditionGroups { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<TransactionTagConn> TransactionTagConns { get; set; }
        public DbSet<Ruleset> Rulesets { get; set; }
        public DbSet<RulesetRuleConn> RulesetRuleConns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=MoneyStats.Dev;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}
