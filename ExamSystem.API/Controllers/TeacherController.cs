using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> AddTeacher([FromBody]CreateTeacherDto teacherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _teacherService.AddTeacherAsync(teacherDto);
                return Ok("Teacher added successfully");
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
        [Authorize(Roles = nameof(RoleName.Admin))]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTeachersAsync()
        {
            var teachers = await _teacherService.GetTeachersWithAllDetailsAsync();
                return Ok(teachers);
                     
        }

        [Authorize(Roles = $"{nameof(RoleName.Teacher)},{nameof(RoleName.Admin)}")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTeacherByIdAsync(Guid id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(id);
                return Ok(teacher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
