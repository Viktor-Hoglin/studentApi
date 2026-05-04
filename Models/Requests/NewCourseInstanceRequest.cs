namespace StudentApi.Models.Requests
{
    public struct NewCourseInstanceRequest {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseId { get; set; }
        public List<string> StudentIds { get; set; }
    }
}