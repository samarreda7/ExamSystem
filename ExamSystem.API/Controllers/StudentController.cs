using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> AddStudent([FromBody]CreateStudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _studentService.AddStudentAsync(studentDto);
                return Created(string.Empty, "Student added successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // 400 — null input
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);  // 409 — email/username already exists
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 — role not found
            }


        }
    }
}
