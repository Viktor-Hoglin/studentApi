namespace StudentApi.Models
{
    public class Grade(int id, string value, CourseInstance courseInstance, Student student)
    {
        public int Id { get; set; } = id;
        public string Value { get; set; } = value;
        public CourseInstance CourseInstance { get; set; } = courseInstance;
        public Student Student { get; set; } = student;
    }
}