using StudentApi.Repositories;
using StudentApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<IStudentService, StudentService>(); // Dependency Injection of IStudentsService
builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>(); // Dependency Injection of IStudentRepository
builder.Services.AddSingleton<ICourseService, CourseService>(); // Dependency Injection of ICoursesService
builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>(); // Dependency Injection of ICourseRepository
builder.Services.AddSingleton<ICourseInstanceService, CourseInstanceService>(); // Dependency Injection of ICourseInstanceService
builder.Services.AddSingleton<ICourseInstanceRepository, InMemoryCourseInstanceRepository>(); // Dependency Injection of ICourseInstanceRepository

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

/* 
Possible future TODOS:
Get courseInstances for a specific student
Add correct status codes with try/catch 
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

TODO: Add correct status codes with try/catch
url structure = "/between?start=2026-08-01T00:00:00&end=2027-04-26T00:00:00"
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

*/

app.Run();