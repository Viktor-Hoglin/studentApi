using StudentApi.Models;

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
    Student s = new(123, "Jacob Hope", "jhope@gmail.com");
    return s;
});

var students = new List<Student> {
    new(123, "Jacob Hope", "jhope@gmail.com"),
    new(232, "Amy Falls", "afalls@gmail.com"),
    new(342, "Luke Pry", "lpry@gmail.com"),
};

var courses = new List<Course> {
    new(1, "History 101", "Introduction to world history."),
    new(2, "Chemistry 101", "Introduction to chemistry."),
    new(3, "English 101", "Introduction to english literature and language."),
};

var CourseInstances = new List<CourseInstance>
{
    new(21, new DateTime(2026, 09, 01), new DateTime(2027, 05, 01), courses[0], [students[0], students[2]]),
    new(22, new DateTime(2026, 09, 01), new DateTime(2027, 04, 25), courses[1], [students[1], students[2]]),
    new(23, new DateTime(2026, 09, 01), new DateTime(2027, 05, 10), courses[2], [students[0], students[1]]),
};

var Grades = new List<Grade>
{
    new(001, "B", CourseInstances[0], students[0]),
    new(002, "A", CourseInstances[0], students[2]),
    new(003, "F", CourseInstances[1], students[1]),
    new(004, "A+", CourseInstances[1], students[2]),
    new(005, "A-", CourseInstances[2], students[0]),
    new(006, "C", CourseInstances[2], students[1]),
};

app.MapGet("/students", () =>
{
    return students; 
});

app.MapGet("/courses", () =>
{
    return courses;
});

app.MapGet("/courses/{id:int}", (int id) =>
{
    var course = courses.Find(c => c.Id == id);
    return course;
});

app.MapGet("/courseInstances", () =>
{
    return CourseInstances;
});

app.MapGet("/courseInstances/studentId={studentId:int}", (int studentId) =>
{
    List<Course> studentCourses = [];

    foreach (CourseInstance ci in CourseInstances) {
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

    foreach (CourseInstance ci in CourseInstances) {
        if(ci.StartDate > start && ci.EndDate < end) {
            coursesBetween.Add(ci.Course);
        }
    }
    
    return coursesBetween;
});

app.MapGet("/grades", () =>
{
    return Grades;
});

app.MapGet("/grades/studentId={studentId:int}", (int studentId) =>
{
    List<Grade> studentGrades = [];
    foreach (Grade g in Grades) {
        if(g.Student.Id == studentId) {
            studentGrades.Add(g);
        }
    }

    var formattedGradeList = new List<object>();
    foreach (Grade g in studentGrades) {
        formattedGradeList.Add(new {
            Course = g.CourseInstance.Course.Title,
            Grade = g.Value
        });
    }
    return formattedGradeList;
});


app.Run();

    