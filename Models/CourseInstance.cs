namespace StudentApi.Models
{
    public class CourseInstance(DateTime startDate, DateTime endDate, Course course, List<Student> students  )
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime StartDate { get; set; } = startDate;
        public DateTime EndDate { get; set; } = endDate;
        public Course Course { get; set; } = course;
        public List<Student> Students { get; set; } = students;
    }
}