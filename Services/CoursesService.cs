using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Repositories;

namespace StudentApi.Services;

public interface ICoursesService
{
	public List<Course> GetCourses();
	public Course? GetCourseById(string id);
	public Course CreateCourse(NewCourseRequest req);
	public Course? DeleteCourse(string id);
	public Course? UpdateCourse(string id, NewCourseRequest req);
}

public class CoursesService(ICourseRepository repo) : ICoursesService
{
  	private ICourseRepository repository = repo;

		public List<Course> GetCourses()
	{
		return repository.GetCourses();
	} 

	public Course? GetCourseById(string id)
	{
		return repository.GetCourseById(id);
	} 

	public Course CreateCourse(NewCourseRequest req)
	{
		Course newCourse = new(req.Title, req.Description);

        bool success = repository.CreateCourse(newCourse);

        if (success)
        {
            return newCourse;
        }
        
        throw new Exception("Could not add course to the database");
	} 
	
	public Course? UpdateCourse(string id, NewCourseRequest req)
	{
		return repository.UpdateCourse(id, req);
	}

	public Course? DeleteCourse(string id)
	{
		return repository.DeleteCourse(id);
	}
}