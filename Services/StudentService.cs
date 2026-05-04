using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Repositories;

namespace StudentApi.Services;

public interface IStudentService
{
	public List<Student> GetAll();
	public Student? GetById(string id);
	public Student Add(NewStudentRequest req);
	public Student? Update(string id, NewStudentRequest req);
	public Student? Delete(string id);
}

public class StudentService(IStudentRepository repo) : IStudentService
{
    private IStudentRepository repository = repo;

	public List<Student> GetAll()
	{
		return repository.GetStudents();
	} 

	public Student? GetById(string id)
	{
		return repository.GetStudentById(id);
	} 

	public Student Add(NewStudentRequest req)
	{
		Student newStudent = new(req.Name, req.Email);

        bool success = repository.AddStudent(newStudent);

        if (success)
        {
            return newStudent;
        }
        
        throw new Exception("Could not add student to the database");
	} 

	public Student? Update(string id, NewStudentRequest req)
	{
		Student? studentToUpdate = repository.GetStudentById(id);
		if(studentToUpdate == null)
		{
			return studentToUpdate; // Throwing error in controller
		}

		bool success = repository.UpdateStudent(studentToUpdate, req);

        if (success)
        {
            return studentToUpdate;
        }
        
        throw new Exception("Could not update student in the database");
	}

	public Student? Delete(string id)
	{
		Student? studentToDelete = repository.GetStudentById(id);
		if(studentToDelete == null)
		{
			return studentToDelete; // Throwing error in controller
		}

		bool success = repository.DeleteStudent(studentToDelete);

		if (success)
        {
            return studentToDelete;
        }
        
        throw new Exception("Could not delete student from the database");
	}
}