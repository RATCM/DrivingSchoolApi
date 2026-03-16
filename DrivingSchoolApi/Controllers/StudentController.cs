using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(
        ILogger<StudentController> logger,
        IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRegistry student)
    {
        
    }
    
    /*
    public async Task<Student> GetStudentById(StudentKey id)
    
    public async Task<IEnumerable<Student>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId)
    
    public async Task DeleteStudent(StudentKey id)
    */
    
    //public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetAllDrivingSchools()
    //public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistry drivingSchool)
    
}