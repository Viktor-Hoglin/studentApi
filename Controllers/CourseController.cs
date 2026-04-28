using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Services;

namespace StudentApi.Controllers;

[ApiController]
[Route("[controller]")] // Using the name of the class as the route (minus the "Controller")
public class CoursesController(ICoursesService service) : ControllerBase 
{

    private readonly ICoursesService service = service;

    [HttpGet]
    public ActionResult<List<Course>> GetCourses()
    {
        try 
        {
            return Ok(service.GetCourses()); 
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Course?> GetCourseById(string id)
    {
        try 
        {
            Course? foundCourse = service.GetCourseById(id);
            if(foundCourse == null) 
            {
                return NotFound($"Found no course with the id: {id}");
            }
            
            return Ok(foundCourse); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public ActionResult<Course?> CreateCourse([FromBody] NewCourseRequest req)
    {
        try 
        {
            if(req.Title == null || req.Description == null) 
            {
                return BadRequest("One or both request parameters are missing.");
            } 
            if(req.Title.GetType() != typeof(string) || req.Description.GetType() != typeof(string)) 
            {
                return BadRequest("One or both request parameters are of the wrong type.");
            }

            return Created("/courses", service.CreateCourse(req));
        }
        catch 
        {   
            throw;
        }
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<Course?> UpdateCourse(string id, [FromBody] NewCourseRequest req)
    {
        try 
        {
            Course? courseToUpdate = service.UpdateCourse(id, req);
            
            if(courseToUpdate == null) 
            {
                return NotFound($"Found no course with the id: {id}");
            }
            if(req.Title == null || req.Description == null) 
            {
                return BadRequest("Could not update course info as one or both request parameters are missing.");
            }

            return Ok(courseToUpdate);
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<Course?> DeleteCourse(string id)
    {
        try 
        {
            if(service.DeleteCourse(id) == null) 
            {
                return NotFound($"Found no course to delete with the id: {id}");
            }

            return NoContent();
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }
}