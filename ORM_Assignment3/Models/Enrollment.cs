
namespace ORM_Assignment3.Models
{
    enum EnrollmentStatus
    {
        Active,
        Completed,
        Dropped
    }
    internal class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal Progress { get; set; }
        public decimal? FinalGrade { get; set; }
        public EnrollmentStatus Status { get; set; }

        public Student Student { get; set; } = null;
        public Course Course { get; set; } = null;

    }
}
