
namespace ORM_Assignment3.Models
{
    internal class Instructor
    {
        public int InstructorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
