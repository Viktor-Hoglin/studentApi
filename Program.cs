using StudentApi.Models;
using StudentApi.Models.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/student", () =>
{
    Student s = new("Jacob Hope", "jhope@gmail.com");
    return s;
});

List<Student> students = [
    new("Jacob Hope", "jhope@gmail.com"),
    new("Amy Falls", "afalls@gmail.com"),
    new("Luke Pry", "lpry@gmail.com"),
];

List<Course> courses = [
    new("History 101", "Introduction to world history."),
    new("Chemistry 101", "Introduction to chemistry."),
    new("English 101", "Introduction to english literature and language."),
];

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

app.MapGet("/students", () =>
{
    try {
        return Results.Ok(students); 
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
    
});

// Get specific student
app.MapGet("/students/{id}", (string id) =>
{

    try {
        Student? foundStudent = students.FirstOrDefault(s => s.Id == id);
        if(foundStudent == null) {
            return Results.NotFound($"Found no user with the id: {id}");
        }
        return Results.Ok(foundStudent); 
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex);
    }
    
});


// Add new student
app.MapPost("/students", (NewStudentRequest req) =>
{
    try {
        Student newStudent = new(req.Name, req.Email);
        students.Add(newStudent);

        return Results.Created("/students", newStudent);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
    
});
/* 
{
    "name": "Bob Larry",
    "email": "blarry@gmail.com"
}
 */

// Delete specific student
app.MapDelete("/students", (string id) =>
{
    try {
        Student? studentToDelete = students.FirstOrDefault(s => s.Id == id);
        if(studentToDelete == null) {
            return Results.NotFound($"Found no user to delete with the id: {id}");
        }
        students.Remove(studentToDelete);

        return Results.NoContent();
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
    
});


app.MapGet("/courses", () =>
{
    return courses;
});


// Get specific course
app.MapGet("/courses/{id}", (string id) =>
{
    var course = courses.Find(c => c.Id == id);
    return course;
});

app.MapGet("/courseInstances", () =>
{
    return courseInstances;
});

// Get courseInstances for a specific student
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


app.Run();

    