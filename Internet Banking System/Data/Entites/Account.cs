using System.ComponentModel.DataAnnotations;
public enum AccountType
{
    Savings,
    Current,
    Business
}

namespace Internet_Banking_System.Entites
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Account number must be digits only")]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]  
        public decimal Balance { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public AccountType Type { get; set; }
        public bool IsActive { get; set; } = true;

        //foreign Key
        public string UserId {  get; set; }
        public User User { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();


    }

}
