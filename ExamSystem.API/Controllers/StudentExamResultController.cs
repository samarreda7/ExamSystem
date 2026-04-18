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
        [HttpGet("{examId:guid}")]
        public async Task<IActionResult> GetStudentResultByExamIdAsync([FromRoute] Guid examId)
        {
            var studentIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim) || !Guid.TryParse(studentIdClaim, out Guid studentId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var result = await _studentExamResultService
                    .GetStudentResultByStudentIdAndExamIdAsync(studentId, examId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
