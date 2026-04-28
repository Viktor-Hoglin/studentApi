using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Repositories;

namespace StudentApi.Services;

public interface IStudentsService
{
	public List<Student> GetStudents();
	public Student? GetStudentById(string id);
	public Student CreateStudent(NewStudentRequest req);
	public Student? DeleteStudent(string id);
	public Student? UpdateStudent(string id, NewStudentRequest req);
}

public class StudentsService(IStudentRepository repo) : IStudentsService
{
    private IStudentRepository repository = repo;

	public List<Student> GetStudents()
	{
		return repository.GetStudents();
	} 

	public Student? GetStudentById(string id)
	{
		return repository.GetStudentById(id);
	} 

	public Student CreateStudent(NewStudentRequest req)
	{
		Student newStudent = new(req.Name, req.Email);

        bool success = repository.CreateStudent(newStudent);

        if (success)
        {
            return newStudent;
        }
        
        throw new Exception("Could not add student to the database");
	} 

	public Student? UpdateStudent(string id, NewStudentRequest req)
	{
		return repository.UpdateStudent(id, req);
	}

	public Student? DeleteStudent(string id)
	{
		return repository.DeleteStudent(id);
	}
}