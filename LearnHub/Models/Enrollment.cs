namespace LearnHub.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public DateTime EnrolledAt { get; set; }
        public int? Rating { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}
