using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Assignment3.Models;

namespace ORM_Assignment3.Configuration
{
    internal class InstructorConfiguration :IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.InstructorId);
            builder.Property(i => i.FullName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Bio).HasMaxLength(500);
            builder.Property(i => i.JoinDate).IsRequired();

            builder.HasData(
                new Instructor
                {
                    InstructorId = 1,
                    FullName = "Instructor1",
                    Email = "instructor1@gmail.com",
                    JoinDate = new DateTime(2023, 6, 1)
                },
                new Instructor
                {
                    InstructorId = 2,
                    FullName = "Instructor2",
                    Email = "instructor2@gmail.com",
                    JoinDate = new DateTime(2024, 9, 11)
                }
                );
        }
    }
}
