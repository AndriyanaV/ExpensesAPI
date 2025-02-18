using ExpensesAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesAPI.Configuration
{

    public class ExpenceConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            var expence1 = new Expense
            {
                Id = 1,
                Title = "Expence 1",
                Amount = 2000,
                Description = "Expence 1",
                UserId = 1,
                CategoryId = 1,
                PaymentMethodId = 2

            };

            var expence2 = new Expense
            {
                Id = 2,
                Title = "Expence 2",
                Amount = 3000,
                Description = "Expence 2",
                UserId = 2,
                CategoryId = 2,
                PaymentMethodId = 1

            };

            var expence3 = new Expense
            {

                Id= 3,
                Title = "Expence 3",
                Amount = 3000,
                Description = "Expence 3",
                UserId = 2,
                CategoryId = 1,
                PaymentMethodId = 1

            };

            builder.HasData(expence1, expence2,expence3);

        }
    }
}
