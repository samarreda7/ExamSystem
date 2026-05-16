using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamGroupController : ControllerBase
    {
        private readonly IExamGroupService _examGroupService;

        public ExamGroupController(IExamGroupService examGroupService)
        {
            _examGroupService = examGroupService;
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignExamToGroupAsync([FromBody] AssignExamToGroupDto dto)
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
                await _examGroupService.AssignExamToGroupAsync(teacherId, dto);
                return Ok("Exam assigned to group successfully");
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
        [HttpGet("{examId:guid}/groups/{groupId:guid}/is-assigned")]
        public async Task<IActionResult> IsExamAssignedToGroupAsync([FromRoute] Guid examId, [FromRoute] Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var isAssigned = await _examGroupService.IsExamAssignedToGroupAsync(teacherId, examId, groupId);
                return Ok(isAssigned);
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

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("groups/{groupId:guid}/exams/count")]
        public async Task<IActionResult> GetExamCountByGroupIdAsync([FromRoute] Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var count = await _examGroupService.GetExamCountByGroupIdAsync(teacherId, groupId);
                return Ok(count);
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

        [Authorize(Roles = nameof(RoleName.Student))]
        [HttpGet("students/exams/count")]
        public async Task<IActionResult> GetAssignedExamCountByStudentIdAsync()
        {
            var studentIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim) || !Guid.TryParse(studentIdClaim, out Guid studentId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var count = await _examGroupService.GetAssignedExamCountByStudentIdAsync(studentId);
                return Ok(count);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpDelete("{examId:guid}/groups/{groupId:guid}")]
        public async Task<IActionResult> RemoveExamFromGroupAsync([FromRoute] Guid examId, [FromRoute] Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                await _examGroupService.RemoveExamFromGroupAsync(teacherId, examId, groupId);
                return Ok("Exam removed from group successfully");
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

        [Authorize(Roles = nameof(RoleName.Student))]
        [HttpGet("groups/{groupId:guid}/exams")]
        public async Task<IActionResult> GetExamsByGroupIdAsync([FromRoute] Guid groupId)
        {
            var studentIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim) || !Guid.TryParse(studentIdClaim, out Guid studentId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var exams = await _examGroupService.GetExamsByGroupIdAsync(studentId, groupId);
                return Ok(exams);
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
