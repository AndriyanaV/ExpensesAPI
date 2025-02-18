using ExpensesAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesAPI.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var category1 = new Category
            {
                Id = 1,
                CategoryName = "Food",
                


            };

            var category2 = new Category
            {
                Id = 2,
                CategoryName = "Clothes",



            };

           builder.HasData(category1, category2);
            
        }
    }
}
