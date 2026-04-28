using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Services;

namespace StudentApi.Controllers;

[ApiController]
[Route("[controller]")] // Using the name of the class as the route (minus the "Controller")
public class StudentsController(IStudentsService service) : ControllerBase 
{

    private readonly IStudentsService service = service;

    [HttpGet]
    public ActionResult<List<Student>> GetStudents() 
    {
        try {
            return Ok(service.GetStudents()); 
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex); 
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Student?> GetStudentById(string id)
    {   
        try 
        {
            Student? foundStudent = service.GetStudentById(id);
            if(foundStudent == null) 
            {
                return NotFound($"Found no student with the id: {id}");
            }
            
            return Ok(foundStudent); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public ActionResult<Student?> CreateStudent([FromBody] NewStudentRequest req)
    {
        try 
        {
            if(req.Name == null || req.Email == null) 
            {
                return BadRequest("One or both request parameters are missing.");
            } 
            if(req.Name.GetType() != typeof(string) || req.Email.GetType() != typeof(string)) 
            {
                return BadRequest("One or both request parameters are of the wrong type.");
            }

            return Created("/students", service.CreateStudent(req));
        }
        catch 
        {   
            throw;
        }
    }


    /* Example of body for updating student
    {
        "name": "Bob Larry",
        "email": "blarry@gmail.com"
    }
        */
    [HttpPut]
    [Route("{id}")]
    public ActionResult<Student?> UpdateStudent(string id, [FromBody] NewStudentRequest req)
    {
        try 
        {
            Student? studentToUpdate = service.UpdateStudent(id, req);
            
            if(studentToUpdate == null) 
            {
                return NotFound($"Found no student with the id: {id}");
            }
            if(req.Name == null || req.Email == null) 
            {
                return BadRequest("Could not update student info as one or both request parameters are missing.");
            }

            return Ok(studentToUpdate);
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<Student?> DeleteStudent(string id)
    {
        try 
        {
            if(service.DeleteStudent(id) == null) 
            {
                return NotFound($"Found no student to delete with the id: {id}");
            }

            return NoContent();
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }
}
