using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Models;

namespace MoneyStats.DAL
{
    public class MoneyStatsContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"");
            }
        }
    }
}
