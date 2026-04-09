using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("add")]
        public async Task<IActionResult> AddExamAsync([FromBody]CreateExamDto dto) 
        {
            if (!ModelState.IsValid) 
            {
            return BadRequest(ModelState);
            }
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                await _examService.AddExamAsync(teacherId, dto);
                return Ok("Exam Added Successfuly");
            }
            catch (KeyNotFoundException ex) 
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("all")]
        public async Task<IActionResult> GetTeacherExams()
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                var exams = await _examService.GetTeacherExamsAsync(teacherId);
                return Ok(exams);
            }
            catch (KeyNotFoundException ex) 
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteExam(Guid Id)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                await _examService.DeleteExamAsync(teacherId, Id);
                return Ok("Exam delete successfully");
            }
            catch(KeyNotFoundException ex) 
            {
                    return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
        }

    }
}
