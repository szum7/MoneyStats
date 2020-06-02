using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Models;

namespace MoneyStats.DAL
{
    public class MoneyStatsContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BankRow> BankRows { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RuleGroup> RuleGroups { get; set; }
        public DbSet<RuleAction> RuleActions { get; set; }
        public DbSet<AndRuleGroup> AndRuleGroups { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<TransactionCreatedWithRule> TransactionCreatedWithRules { get; set; }
        public DbSet<TransactionTagConn> TransactionTagConns { get; set; }
        public DbSet<Ruleset> Rulesets { get; set; }
        public DbSet<RulesetRuleGroupConn> RulesetRuleGroupConns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MoneyStats.Database.v2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}
