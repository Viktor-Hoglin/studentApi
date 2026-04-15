namespace StudentApi.Models
{
    public class Grade(string value, CourseInstance courseInstance, Student student)
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Value { get; set; } = value;
        public CourseInstance CourseInstance { get; set; } = courseInstance;
        public Student Student { get; set; } = student;
    }
}