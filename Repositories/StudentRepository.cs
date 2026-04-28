using StudentApi.Models;
using StudentApi.Models.Requests;

namespace StudentApi.Repositories;
public interface IStudentRepository
{
  	public List<Student> GetStudents();
	public Student? GetStudentById(string id);
	public bool CreateStudent(Student studentToAdd);
	public Student? DeleteStudent(string id);
	public Student? UpdateStudent(string id, NewStudentRequest req);
}

public class StudentRepository : IStudentRepository
{
	private List<Student> students = [
		new("Jacob Hope", "jhope@gmail.com"),
		new("Amy Falls", "afalls@gmail.com"),
		new("Luke Pry", "lpry@gmail.com"),
    ];

	public List<Student> GetStudents()
	{
		return students;
	}
	public Student? GetStudentById(string id)
	{
		return students.FirstOrDefault(s => s.Id == id);
	}

	public bool CreateStudent(Student studentToAdd)
	{
		try
		{
			students.Add(studentToAdd);
			
			return true;
		}
		catch
		{
			throw;
		}

	}

	public Student? UpdateStudent(string id, NewStudentRequest req)
	{
		Student? studentToUpdate = GetStudentById(id);
		if(studentToUpdate == null || req.Name == null || req.Email == null)
		{
			return studentToUpdate;
		} 

		studentToUpdate.Name = req.Name;
        studentToUpdate.Email = req.Email;

		return studentToUpdate;
	}

	public Student? DeleteStudent(string id)
	{
		Student? studentToDelete = GetStudentById(id);
		if(studentToDelete != null)
		{
			students.Remove(studentToDelete);
		} 

		return studentToDelete;
	}
}