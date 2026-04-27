using StudentApi.Models;
using StudentApi.Models.Requests;

namespace StudentApi.Services;

public interface IStudentsService
{
	public List<Student> GetStudents();
	public Student? GetStudentById(string id);
	public Student CreateStudent(NewStudentRequest req);
	public Student? DeleteStudent(string id);
	public Student? UpdateStudent(string id, NewStudentRequest req);
}

public class StudentsService : IStudentsService
{
  	public List<Student> students = [
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

	public Student CreateStudent(NewStudentRequest req)
	{
		Student newStudent = new(req.Name, req.Email);
        students.Add(newStudent);
        
		return newStudent;
	} 

	public Student? UpdateStudent(string id, NewStudentRequest req)
	{
		Student? studentToUpdate = students.FirstOrDefault(s => s.Id == id);
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
		Student? studentToDelete = students.FirstOrDefault(s => s.Id == id);
		if(studentToDelete != null)
		{
			students.Remove(studentToDelete);
		} 
		
		return studentToDelete;	
	}
}