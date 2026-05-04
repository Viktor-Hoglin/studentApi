using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Repositories;

namespace StudentApi.Services;

public interface ICourseInstanceService
{
	public List<CourseInstance> GetAll();
	public CourseInstance? GetById(string id);
	public CourseInstance Add(NewCourseInstanceRequest req);
	public CourseInstance? Update(string id, NewCourseInstanceRequest req);
	public CourseInstance? Delete(string id);
}

public class CourseInstanceService(ICourseInstanceRepository repo, IStudentRepository studentRepo, ICourseRepository courseRepo) : ICourseInstanceService
{
    private ICourseInstanceRepository courseInstanceRepository = repo;
	private IStudentRepository studentRepository = studentRepo;
	 private ICourseRepository courseRepository = courseRepo;

	public List<CourseInstance> GetAll()
	{
		return courseInstanceRepository.GetInstances();
	} 

	public CourseInstance? GetById(string id)
	{
		return courseInstanceRepository.GetInstanceById(id);
	} 

	public CourseInstance Add(NewCourseInstanceRequest req)
	{
        Course? reqCourse = courseRepository.GetCourseById(req.CourseId);
		if (reqCourse == null){
            throw new Exception($"Course with id:{req.CourseId} not found");
        }

		List<Student> reqStudents = new List<Student>();

		foreach (string reqStudentId in req.StudentIds) {
			Student? requestedStudent = studentRepository.GetStudentById(reqStudentId);
			if(requestedStudent != null) {
				reqStudents.Add(requestedStudent);
			} else {
				throw new Exception($"Student with id:{reqStudentId} not found");
			}
		}

		CourseInstance instanceToAdd = new(req.StartDate, req.EndDate, reqCourse, reqStudents);

        bool success = courseInstanceRepository.AddInstance(instanceToAdd);

        if (success)
        {
            return instanceToAdd;
        }
        
        throw new Exception("Could not add courceInstance to the database");
	} 

	public CourseInstance? Update(string id, NewCourseInstanceRequest req)
	{
		CourseInstance? instanceToUpdate = courseInstanceRepository.GetInstanceById(id);
        if(instanceToUpdate == null) {
            return instanceToUpdate; // Throwing error in controller
        }

        Course? reqCourse = courseRepository.GetCourseById(req.CourseId);
		if (reqCourse == null){
            throw new Exception($"Course with id:{req.CourseId} not found in database");
        }

		List<Student> reqStudents = [];

		foreach (string reqStudentId in req.StudentIds) {
			Student? requestedStudent = studentRepository.GetStudentById(reqStudentId);
			if(requestedStudent != null) {
				reqStudents.Add(requestedStudent);
			} else {
				throw new Exception($"Student with id:{reqStudentId} not found in database");
			}
		}

		bool success = courseInstanceRepository.UpdateInstance(instanceToUpdate, reqCourse, reqStudents, req);

		if (success)
        {
            return instanceToUpdate;
        }
        
        throw new Exception("Could not update courceInstance in the database");
	}

	public CourseInstance? Delete(string id)
	{
		CourseInstance? instanceToDelete = courseInstanceRepository.GetInstanceById(id);
		if (instanceToDelete == null)
		{
			throw new Exception($"Found no courseInstance with the id: {id}");
		}

		bool success = courseInstanceRepository.DeleteInstance(instanceToDelete);

		if (success)
		{
			return instanceToDelete;
		}
		
		throw new Exception("Could not delete courseInstance from the database");
	}
}