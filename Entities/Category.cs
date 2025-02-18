using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public String CategoryName {  get; set; }

        
        public ICollection<Expense>? Expences { get; set; }
    }
}
