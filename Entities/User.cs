using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Entities
{
    public class User : IdentityUser<int>

    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }


        public ICollection<Expense>? Expences { get; set; }
    }

}