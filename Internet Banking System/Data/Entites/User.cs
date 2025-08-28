using System.ComponentModel.DataAnnotations;

namespace Internet_Banking_System.Entites
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        public List<Account> Accounts { get; set; } = new List<Account>();
    }

}
