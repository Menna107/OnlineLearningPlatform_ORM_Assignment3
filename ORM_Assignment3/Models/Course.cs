
namespace ORM_Assignment3.Models
{
    enum Level
    {
        Beginner,
        Intermediate,
        Advanced
    }
    internal class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } 
        public Level Level { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }

        public int InstructorId { get; set; } 
        public Instructor Instructor { get; set; } = null;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null;
    }
}
