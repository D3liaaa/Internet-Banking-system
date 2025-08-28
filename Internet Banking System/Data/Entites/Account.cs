using System.ComponentModel.DataAnnotations;

namespace Internet_Banking_System.Entites
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]  
        public decimal Balance { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        //foreign Key
        public int UserId {  get; set; }
        public User User { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }

}
