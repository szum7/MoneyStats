using MoneyStats.DAL.Models;

namespace MoneyStats.BL.Interfaces
{
    public interface ITransactionRepository : IEntityBaseRepository<Transaction>
    {
        void ExtraMethod();
    }
}
