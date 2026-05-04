using StudentApi.Models;
using StudentApi.Models.Requests;

namespace StudentApi.Repositories;

public interface ICourseRepository
{
	public List<Course> GetCourses();
	public Course? GetCourseById(string id);
	public bool AddCourse(Course courseToAdd);
	public bool UpdateCourse(Course courseToUpdate, NewCourseRequest req);
	public bool DeleteCourse(Course courseToDelete);
}

public class InMemoryCourseRepository : ICourseRepository
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

	public bool AddCourse(Course courseToAdd)
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

	public bool UpdateCourse(Course courseToUpdate, NewCourseRequest req)
	{
		try
		{
			courseToUpdate.Title = req.Title;
			courseToUpdate.Description = req.Description;

			return true;
		}
		catch
		{
			throw;
		}
	}

	public bool DeleteCourse(Course courseToDelete)
	{
		try
		{
			courses.Remove(courseToDelete);

			return true;
		}
		catch
		{
			throw;
		}
	}
}