using Microsoft.EntityFrameworkCore;
using MoneyStats.DAL.Model;

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
