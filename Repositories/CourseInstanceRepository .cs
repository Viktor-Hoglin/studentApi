using StudentApi.Models;
using StudentApi.Models.Requests;


namespace StudentApi.Repositories;

public interface ICourseInstanceRepository
{
	public List<CourseInstance> GetInstances();
	public CourseInstance? GetInstanceById(string id);
	public bool AddInstance(CourseInstance instanceToAdd);
	public bool UpdateInstance(CourseInstance instanceToUpdate, Course reqCourse, List<Student> reqStudents, NewCourseInstanceRequest req);
	public bool DeleteInstance(CourseInstance instanceToDelete);
}

public class InMemoryCourseInstanceRepository(IStudentRepository studentRepo, ICourseRepository courseRepo) : ICourseInstanceRepository
{
	private readonly List<CourseInstance> courseInstances = [
		new(new DateTime(2026, 09, 01), new DateTime(2027, 05, 01), courseRepo.GetCourses()[0], [studentRepo.GetStudents()[0], studentRepo.GetStudents()[2]]),
		new(new DateTime(2026, 09, 01), new DateTime(2027, 04, 25), courseRepo.GetCourses()[1], [studentRepo.GetStudents()[1], studentRepo.GetStudents()[2]]),
		new(new DateTime(2026, 09, 01), new DateTime(2027, 05, 10), courseRepo.GetCourses()[2], [studentRepo.GetStudents()[0], studentRepo.GetStudents()[1]]),
	];

	public List<CourseInstance> GetInstances()
	{
		return courseInstances;
	}
	public CourseInstance? GetInstanceById(string id)
	{
		return courseInstances.FirstOrDefault(i => i.Id == id);
	}

	public bool AddInstance(CourseInstance instanceToAdd)
	{
		try {
        	courseInstances.Add(instanceToAdd); 

			return true;
		}
		catch {
			throw;
		}

	}

	public bool UpdateInstance(CourseInstance instanceToUpdate, Course reqCourse, List<Student> reqStudents, NewCourseInstanceRequest req)
	{
		try {
        	instanceToUpdate.StartDate = req.StartDate;
        	instanceToUpdate.EndDate = req.EndDate;
        	instanceToUpdate.Course = reqCourse;
        	instanceToUpdate.Students = reqStudents;

      		return true;
		}
		catch {
			throw;
		}
	}

  public bool DeleteInstance(CourseInstance instanceToDelete)
	{
		try {
			courseInstances.Remove(instanceToDelete);

			return true;
		}
		catch {
			throw;
		}
	}
}