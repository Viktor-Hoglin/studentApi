namespace StudentApi.Models;

public class Student(string name, string email)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
}
