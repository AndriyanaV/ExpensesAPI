using ExpensesAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Configuration
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PaymentMethod> builder)
        {
            var payment1 = new PaymentMethod
            {
                Id = 1,
                PaymentName = "cash"
            };

            var payment2 = new PaymentMethod
            {
                Id = 2,
                PaymentName = "creditCard"
            };


            builder.HasData(payment1, payment2);
        }
    }
}
