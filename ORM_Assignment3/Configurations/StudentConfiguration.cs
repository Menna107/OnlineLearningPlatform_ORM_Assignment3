using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Assignment3.Models;

namespace ORM_Assignment3.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.DOB).IsRequired();
            builder.Property(s => s.Email).IsRequired().HasMaxLength(100);

            builder.HasData(
                new Student
                {
                    StudentId = 1,
                    FullName = "Student1",
                    DOB = new DateTime(2000, 1, 1),
                    Email = "student1@gmail.com"
                },
                new Student
                {
                    StudentId = 2,
                    FullName = "Student2",
                    DOB = new DateTime(2001, 2, 2),
                    Email = "student2@gmail.com"
                }
                );
        }
    }
}
