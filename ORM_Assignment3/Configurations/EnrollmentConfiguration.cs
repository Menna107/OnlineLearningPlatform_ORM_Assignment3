using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Assignment3.Models;

namespace ORM_Assignment3.Configurations
{
    internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new 
            { 
                e.StudentId, e.CourseId 
            });
            builder.Property(e => e.EnrollmentDate).IsRequired();
            builder.Property(e => e.Progress)
                .IsRequired()
                .HasPrecision(5,2);

            builder.Property(e => e.FinalGrade)
                .HasPrecision(5, 2);
            
            builder.Property(e => e.Status).IsRequired();
           
            builder.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId);

            builder.HasOne(e => e.Course)
               .WithMany()
               .HasForeignKey(e => e.CourseId);

            builder.HasData(
                new Enrollment
                {
                    StudentId = 1,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2024, 9, 1),
                    Progress = 50,
                    FinalGrade = null,
                    Status = EnrollmentStatus.Active
                },
                new Enrollment
                {
                    StudentId = 2,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2024, 9, 1),
                    Progress = 100,
                    FinalGrade = 95,
                    Status = EnrollmentStatus.Completed
                }
            );
        }
    }
}
