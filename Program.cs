using Microsoft.AspNetCore.Http.HttpResults;
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

// Student Endpoints

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
            return Results.NotFound($"Found no student with the id: {id}");
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
        if(req.Name == null || req.Email == null) {
            return Results.BadRequest("One or both request parameters are missing.");
        } 
        if(req.Name.GetType() != typeof(string) || req.Email.GetType() != typeof(string)) {
            return Results.BadRequest("One or both request parameters are of the wrong type.");
        }

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

app.MapPut("/students/{id}", (string id, NewStudentRequest req) =>
{
    try {
        Student? studentToUpdate = students.FirstOrDefault(s => s.Id == id);
        if(studentToUpdate == null) {
            return Results.NotFound($"Found no student with the id: {id}");
        }
        if(req.Name == null || req.Email == null) {
            return Results.BadRequest("Could not update student info as one or both request parameters are missing.");
        }
        
        studentToUpdate.Name = req.Name;
        studentToUpdate.Email = req.Email;

        return Results.Ok(studentToUpdate);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

app.MapDelete("/students/{id}", (string id) =>
{
    try {
        Student? studentToDelete = students.FirstOrDefault(s => s.Id == id);
        if(studentToDelete == null) {
            return Results.NotFound($"Found no student to delete with the id: {id}");
        }
        students.Remove(studentToDelete);

        return Results.NoContent();
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

// Courses Endpoints

app.MapGet("/courses", () =>
{
    try {
        return Results.Ok(courses); 
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});


app.MapGet("/courses/{id}", (string id) =>
{
    try {
        Course? foundCourse = courses.FirstOrDefault(c => c.Id == id);
        if(foundCourse == null) {
            return Results.NotFound($"Found no course with the id: {id}");
        }
        return Results.Ok(foundCourse); 
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex);
    }
});

app.MapPost("/courses", (NewCourseRequest req) =>
{
    try {
        if(req.Title == null || req.Description == null) {
            return Results.BadRequest("One or both request parameters are missing.");
        } 
        if(req.Title.GetType() != typeof(string) || req.Description.GetType() != typeof(string)) {
            return Results.BadRequest("One or both request parameters are of the wrong type.");
        }

        Course newCourse = new(req.Title, req.Description);
        courses.Add(newCourse);
        return Results.Created("/courses", newCourse);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

app.MapPut("/courses/{id}", (string id, NewCourseRequest req) =>
{
    try {
        Course? courseToUpdate = courses.FirstOrDefault(c => c.Id == id);
        if(courseToUpdate == null) {
            return Results.NotFound($"Found no course with the id: {id}");
        }
        if(req.Title == null || req.Description == null) {
            return Results.BadRequest("Could not update course info as one or both request parameters are missing.");
        }
        
        courseToUpdate.Title = req.Title;
        courseToUpdate.Description = req.Description;

        return Results.Ok(courseToUpdate);
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

app.MapDelete("/courses/{id}", (string id) =>
{
    try {
        Course? courseToDelete = courses.FirstOrDefault(c => c.Id == id);
        if(courseToDelete == null) {
            return Results.NotFound($"Found no course to delete with the id: {id}");
        }
        courses.Remove(courseToDelete);

        return Results.NoContent();
    }
    catch (Exception ex) {   
        return Results.InternalServerError(ex);
    }
});

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


app.Run();

    