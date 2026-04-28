using StudentApi.Models;
using StudentApi.Models.Requests;

namespace StudentApi.Repositories;

public interface ICourseRepository
{
	public List<Course> GetCourses();
	public Course? GetCourseById(string id);
	public bool CreateCourse(Course courseToAdd);
	public Course? DeleteCourse(string id);
	public Course? UpdateCourse(string id, NewCourseRequest req);
}

public class CourseRepository : ICourseRepository
{
	private List<Course> courses = [
		new("History 101", "Introduction to world history."),
		new("Chemistry 101", "Introduction to chemistry."),
		new("English 101", "Introduction to english literature and language."),
	];

	public List<Course> GetCourses()
	{
		return courses;
	}
	public Course? GetCourseById(string id)
	{
		return courses.FirstOrDefault(s => s.Id == id);
	}

	public bool CreateCourse(Course courseToAdd)
	{
		try
		{
			courses.Add(courseToAdd);
			
			return true;
		}
		catch
		{
			throw;
		}

	}

	public Course? UpdateCourse(string id, NewCourseRequest req)
	{
		Course? courseToUpdate = GetCourseById(id);
		if(courseToUpdate == null || req.Title == null || req.Description == null)
		{
			return courseToUpdate;
		} 

		courseToUpdate.Title = req.Title;
        courseToUpdate.Description = req.Description;

		return courseToUpdate;
	}

	public Course? DeleteCourse(string id)
	{
		Course? courseToDelete = GetCourseById(id);
		if(courseToDelete != null)
		{
			courses.Remove(courseToDelete);
		} 

		return courseToDelete;
	}
}