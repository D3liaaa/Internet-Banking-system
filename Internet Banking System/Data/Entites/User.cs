using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Internet_Banking_System.Entites
{
    public class User:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public List<Account> Accounts { get; set; } = new List<Account>();
    }

}
