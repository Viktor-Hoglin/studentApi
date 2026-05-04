using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Models.Requests;
using StudentApi.Services;

namespace StudentApi.Controllers;

[ApiController]
[Route("[controller]")] // Using the name of the class as the route (minus the "Controller")
public class CourseInstancesController(ICourseInstanceService service) : ControllerBase 
{
    private readonly ICourseInstanceService service = service;

    [HttpGet]
    public ActionResult<List<CourseInstance>> GetInstances() 
    {
        try {
            return Ok(service.GetAll()); 
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex); 
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<CourseInstance?> GetInstanceById(string id)
    {   
        try 
        {
            CourseInstance? foundCourseInstance = service.GetById(id);
            if(foundCourseInstance == null) {
                return NotFound($"Found no courseInstance with the id: {id}");
            }
            
            return Ok(foundCourseInstance); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public ActionResult<CourseInstance?> AddInstance([FromBody] NewCourseInstanceRequest req)
    {
        try 
        {
            if(req.CourseId == null || req.StudentIds == null || req.StudentIds.Count == 0 || req.StartDate.GetType() != typeof(DateTime) || req.EndDate.GetType() != typeof(DateTime)) {
                return BadRequest("One or more request parameters are missing or are invalid.");
            }

            if(req.StartDate > req.EndDate) {
                return BadRequest("StartDate needs to be before endDate.");
            }
            
            return Created("/students", service.Add(req));
        }
        catch 
        {   
            throw;
        }
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<CourseInstance?> UpdateInstance(string id, [FromBody] NewCourseInstanceRequest req)
    {
        try 
        {
            if(req.StartDate > req.EndDate) {
                return BadRequest("StartDate needs to be before endDate.");
            }

            CourseInstance? instanceToUpdate = service.Update(id, req);
            if(instanceToUpdate == null)
            {
                return NotFound($"Found no courseInstance with the id: {id}");
            }

            return Ok(instanceToUpdate);
        }
        catch
        {   
            throw;
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<CourseInstance?> DeleteInstance(string id)
    {
        try 
        {
            if(service.Delete(id) == null) 
            {
                return NotFound($"Found no courseInstance to delete with the id: {id}");
            }

            return NoContent();
        }
        catch (Exception ex) 
        {   
            return BadRequest(ex);
        }
    }
}
