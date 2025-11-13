namespace LearnHub.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public int Price { get; set; }
        public double? TotalRating { get; set; } = 0;
        public int TotalVotes { get; set; } = 0;
        public int NumberOfLearnears { get; set; } = 0;
        public bool IsApproved { get; set; } = false;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Lesson> Lessons { get; set; }
        public string ApplicationUserId { get; set; }  
        public virtual ApplicationUser ApplicationUser { get; set; } 
        public virtual List<Enrollment>? Enrollments { get; set; }

    }
}










