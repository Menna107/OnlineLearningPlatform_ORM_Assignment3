using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Assignment3.Models;

namespace ORM_Assignment3.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.Price).HasPrecision(10, 2);
            builder.Property(c => c.Level).IsRequired();
            builder.Property(c => c.IsPublished).IsRequired();

            builder.HasOne(c => c.Instructor)
                .WithMany()
                .HasForeignKey(c => c.InstructorId);

            builder.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId);

            builder.HasData(
                new Course
                {
                    CourseId = 1,
                    Title = "Course1",
                    Description = "Description for Course1",
                    Price = 300,
                    Level = Level.Beginner,
                    IsPublished = true,
                    InstructorId = 1,
                    CategoryId = 2
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Course2",
                    Description = "Description for Course2",
                    Price = 800,
                    Level = Level.Intermediate,
                    IsPublished = false,
                    InstructorId = 2,
                    CategoryId = 1
                }
            );
        }
    }
}
