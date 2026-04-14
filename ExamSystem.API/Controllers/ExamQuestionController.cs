using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamQuestionController : ControllerBase
    {
        private readonly IExamQuestionService _examQuestionService;

        public ExamQuestionController(IExamQuestionService examQuestionService)
        {
            _examQuestionService = examQuestionService;
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignQuestionToExamAsync([FromBody] AssignQuestionToExamDto dto)
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
                await _examQuestionService.AssignQuestionToExamAsync(teacherId, dto);
                return Ok("Question assigned to exam successfully");
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

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpDelete("{examId:guid}/questions/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionFromExamAsync([FromRoute] Guid examId, [FromRoute] Guid questionId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                await _examQuestionService.RemoveQuestionFromExamAsync(teacherId, examId, questionId);
                return Ok("Question removed from exam successfully");
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

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("{examId:guid}/questions")]
        public async Task<IActionResult> GetQuestionsByExamIdAsync([FromRoute] Guid examId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var questions = await _examQuestionService.GetQuestionsByExamIdAsync(teacherId, examId);
                return Ok(questions);
            }
            catch (KeyNotFoundException ex)
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
