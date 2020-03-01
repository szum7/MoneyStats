using MoneyStats.DAL.Models;

namespace MoneyStats.BL.Interfaces
{
    public interface ITransactionRepository : IEntityRepository<Transaction>
    {
        void ExtraMethod();
    }
}
