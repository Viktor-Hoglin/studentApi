namespace StudentApi.Models;

public class Course(string title, string description)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
}
