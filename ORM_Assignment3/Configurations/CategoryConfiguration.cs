using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Assignment3.Models;

namespace ORM_Assignment3.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Category1"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Category2",
                    ParentCategoryId = 1
                }
            );
        }
    }
}
