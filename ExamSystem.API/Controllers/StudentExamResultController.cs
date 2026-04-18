using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentExamResultController : ControllerBase
    {
        private readonly IStudentExamResultService _studentExamResultService;

        public StudentExamResultController(IStudentExamResultService studentExamResultService)
        {
            _studentExamResultService = studentExamResultService;
        }

        [Authorize(Roles = nameof(RoleName.Student))]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitExamAsync([FromBody] SubmitExamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim) || !Guid.TryParse(studentIdClaim, out Guid studentId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                await _studentExamResultService.SubmitExamAsync(studentId, dto);
                return Ok("Exam submitted successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}
