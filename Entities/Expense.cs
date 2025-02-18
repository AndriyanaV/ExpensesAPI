using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesAPI.Entities
{
    public class Expense
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public String Title { get; set; }

        [Required]
        public int Amount { get; set; }

        [MaxLength(500)]    
        public String Description { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(Category))]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(PaymentMethod))]
        [Required]
        public int PaymentMethodId { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

    }
}
