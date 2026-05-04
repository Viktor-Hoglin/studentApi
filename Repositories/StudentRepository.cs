using StudentApi.Models;
using StudentApi.Models.Requests;

namespace StudentApi.Repositories;
public interface IStudentRepository
{
  	public List<Student> GetStudents();
	public Student? GetStudentById(string id);
	public bool AddStudent(Student studentToAdd);
	public bool UpdateStudent(Student studentToUpdate, NewStudentRequest req);
	public bool DeleteStudent(Student studentToDelete);
}

public class InMemoryStudentRepository : IStudentRepository
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

	public bool AddStudent(Student studentToAdd)
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

	public bool UpdateStudent(Student studentToUpdate, NewStudentRequest req)
	{
		try
		{
			studentToUpdate.Name = req.Name;
        	studentToUpdate.Email = req.Email;
			
			return true;
		}
		catch
		{
			throw;
		}
	}

	public bool DeleteStudent(Student studentToDelete)
	{
		try
		{
			students.Remove(studentToDelete);

			return true;
		}
		catch
		{
			throw;
		}
	}
}