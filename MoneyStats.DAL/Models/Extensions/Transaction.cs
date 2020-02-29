using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public partial class Transaction
    {
        [NotMapped]
        public string ContentId => $"{AccountingDate.ToString()}{TransactionId}{Type}{Account}{AccountName}{PartnerAccount}{PartnerName}{Sum?.ToString()}{Currency}{Message}";
    }
}
