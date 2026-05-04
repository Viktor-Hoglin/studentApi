using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Repositories;

namespace StudentApi.Services;

public interface ICourseService
{
	public List<Course> GetAll();
	public Course? GetById(string id);
	public Course Add(NewCourseRequest req);
	public Course? Update(string id, NewCourseRequest req);
	public Course? Delete(string id);
}

public class CourseService(ICourseRepository repo) : ICourseService
{
  	private ICourseRepository repository = repo;

		public List<Course> GetAll()
	{
		return repository.GetCourses();
	} 

	public Course? GetById(string id)
	{
		return repository.GetCourseById(id);
	} 

	public Course Add(NewCourseRequest req)
	{
		Course newCourse = new(req.Title, req.Description);

        bool success = repository.AddCourse(newCourse);

        if (success)
        {
            return newCourse;
        }
        
        throw new Exception("Could not add course to the database");
	} 
	
	public Course? Update(string id, NewCourseRequest req)
	{
		Course? courseToUpdate = repository.GetCourseById(id);
		if(courseToUpdate == null)
		{
			return courseToUpdate;
		}

		bool success = repository.UpdateCourse(courseToUpdate, req);

		if (success)
        {
            return courseToUpdate;
        }
        
        throw new Exception("Could not update course in the database");
	}

	public Course? Delete(string id)
	{
		Course? courseToDelete = repository.GetCourseById(id);
		if(courseToDelete == null){
			return courseToDelete; // Throwing error in controller
		}

		bool success = repository.DeleteCourse(courseToDelete);

		if (success)
        {
            return courseToDelete;
        }
        
        throw new Exception("Could not update course in the database");
	}
}