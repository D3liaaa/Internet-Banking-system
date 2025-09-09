using System.ComponentModel.DataAnnotations;

namespace Internet_Banking_System.Entites
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public TransactionType Type { get; set; }

        // Foreign key
        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int? TargetAccountId { get; set; }
        public Account? TargetAccount { get; set; }
        public string? Description { get; set; }
    }

}
