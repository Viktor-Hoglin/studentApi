using StudentApi.Repositories;
using StudentApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<IStudentsService, StudentsService>(); // Dependency Injection of IStudentsService
builder.Services.AddSingleton<IStudentRepository, StudentRepository>(); // Dependency Injection of IStudentRepository
builder.Services.AddSingleton<ICoursesService, CoursesService>(); // Dependency Injection of ICoursesService
builder.Services.AddSingleton<ICourseRepository, CourseRepository>(); // Dependency Injection of ICourseRepository

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

/*

List<CourseInstance> courseInstances = [
    new(new DateTime(2026, 09, 01), new DateTime(2027, 05, 01), courses[0], [students[0], students[2]]),
    new(new DateTime(2026, 09, 01), new DateTime(2027, 04, 25), courses[1], [students[1], students[2]]),
    new(new DateTime(2026, 09, 01), new DateTime(2027, 05, 10), courses[2], [students[0], students[1]]),
];

List<Grade> grades = [
    new("B", courseInstances[0], students[0]),
    new("A", courseInstances[0], students[2]),
    new("F", courseInstances[1], students[1]),
    new("A+", courseInstances[1], students[2]),
    new("A-", courseInstances[2], students[0]),
    new("C", courseInstances[2], students[1]),
];

// CourseInstances Endpoints

app.MapGet("/courseInstances", () =>
{
    try {
        return Results.Ok(courseInstances); 
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

// Get courseInstances by its id
app.MapGet("/courseInstances/{id}", (string id) =>
{
    try {
        CourseInstance? foundCourseInstance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if(foundCourseInstance == null) {
            return Results.NotFound($"Found no courseInstance with the id: {id}");
        }
        return Results.Ok(foundCourseInstance); 
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex);
    }
});

app.MapPost("/courseInstances", (NewCourseInstance req) =>
{
    try {
        if(req.CourseId == null || req.StudentIds == null || req.StartDate.GetType() != typeof(DateTime) || req.EndDate.GetType() != typeof(DateTime)) {
            return Results.BadRequest("One or more request parameters are missing or are invalid.");
        }

        Course? reqCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (reqCourse == null){
            return Results.BadRequest($"Could not find course with the id of: {req.CourseId}");
        }

        List<Student>? reqStudents = [];
        
        foreach (string reqStudentId in req.StudentIds) {
            Student? requestedStudent = students.FirstOrDefault(s => s.Id == reqStudentId);
            if(requestedStudent != null) {
                reqStudents.Add(requestedStudent);
            } else {
                return Results.BadRequest($"Could not find student with the id of: {reqStudentId}");
            }
        }

        if(req.StartDate > req.EndDate) {
            return Results.BadRequest("StartDate needs to be before endDate.");
        }
        
        CourseInstance newCourseInstance = new(req.StartDate, req.EndDate, reqCourse, reqStudents);

        courseInstances.Add(newCourseInstance); 
        return Results.Created("/courseInstances", newCourseInstance);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

app.MapPut("/courseInstances/{id}", (string id, NewCourseInstance req) =>
{
    try {
        CourseInstance? courseInstanceToUpdate = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if(courseInstanceToUpdate == null) {
            return Results.NotFound($"Found no courseInstance with the id: {id}");
        }

        if(req.CourseId == null || req.StudentIds == null || req.StartDate.GetType() != typeof(DateTime) || req.EndDate.GetType() != typeof(DateTime)) {
            return Results.BadRequest("Could not update instance as one or more request parameters are missing or are invalid.");
        }

        Course? reqCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (reqCourse == null){
            return Results.BadRequest($"Could not find course with the id of: {req.CourseId}");
        }

        List<Student>? reqStudents = [];
        
        foreach (string reqStudentId in req.StudentIds) {
            Student? requestedStudent = students.FirstOrDefault(s => s.Id == reqStudentId);
            if(requestedStudent != null) {
                reqStudents.Add(requestedStudent);
            } else {
                return Results.BadRequest($"Could not find student with the id of: {reqStudentId}");
            }
        }

        if(req.StartDate > req.EndDate) {
            return Results.BadRequest("Could not update courceInstance as startDate needs to be before endDate.");
        }
        
        courseInstanceToUpdate.StartDate = req.StartDate;
        courseInstanceToUpdate.EndDate = req.EndDate;
        courseInstanceToUpdate.Course = reqCourse;
        courseInstanceToUpdate.Students = reqStudents;

        return Results.Ok(courseInstanceToUpdate);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

app.MapDelete("/courseInstances/{id}", (string id) =>
{
    try {
        CourseInstance? courseInstanceToDelete = courseInstances.FirstOrDefault(c => c.Id == id);
        if(courseInstanceToDelete == null) {
            return Results.NotFound($"Found no courseInstance to delete with the id: {id}");
        }
        courseInstances.Remove(courseInstanceToDelete);

        return Results.NoContent();
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

// Get courseInstances for a specific student
// TODO: Add correct status codes with try/catch 
app.MapGet("/courseInstances/studentId={studentId}", (string studentId) =>
{
    List<Course> studentCourses = [];

    foreach (CourseInstance ci in courseInstances) {
        if(ci.Students.Find(s=> s.Id == studentId) != null) {
            studentCourses.Add(ci.Course);
        }
    }
    
    return studentCourses;
});

// TODO: Add correct status codes with try/catch
// url structure = "/between?start=2026-08-01T00:00:00&end=2027-04-26T00:00:00"
app.MapGet("/courseInstances/between", (DateTime start, DateTime end) =>
{
    List<Course> coursesBetween = [];

    foreach (CourseInstance ci in courseInstances) {
        if(ci.StartDate > start && ci.EndDate < end) {
            coursesBetween.Add(ci.Course);
        }
    }
    
    return coursesBetween;
});

app.MapGet("/grades", () =>
{
    return grades;
});

// Get grades for a specific student
app.MapGet("/grades/studentId={studentId}", (string studentId) =>
{
    List<Grade> studentgrades = [];
    foreach (Grade g in grades) {
        if(g.Student.Id == studentId) {
            studentgrades.Add(g);
        }
    }

    var formattedGradeList = new List<object>();
    foreach (Grade g in studentgrades) {
        formattedGradeList.Add(new {
            Course = g.CourseInstance.Course.Title,
            Grade = g.Value
        });
    }
    return formattedGradeList;
});

*/

app.Run();