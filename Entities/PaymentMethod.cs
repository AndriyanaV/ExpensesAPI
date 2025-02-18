using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string PaymentName { get; set; }

        public ICollection<Expense>? Expenses { get; set; }
    }
}
