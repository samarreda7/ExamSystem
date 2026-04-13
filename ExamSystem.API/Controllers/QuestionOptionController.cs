using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionOptionController : ControllerBase
    {
        private readonly IQuestionOptionService _questionOptionService;

        public QuestionOptionController(IQuestionOptionService questionOptionService)
        {
            _questionOptionService = questionOptionService;
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignOptionToQuestionAsync([FromBody] CreateQuestionOptionDto dto)
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
                await _questionOptionService.AssignOptionToQuestionAsync(teacherId, dto);
                return Ok("Option assigned to question successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
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
        [HttpPut("{optionId:guid}")]
        public async Task<IActionResult> UpdateOptionAsync([FromRoute] Guid optionId, [FromBody] UpdateQuestionOptionDto dto)
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
                await _questionOptionService.UpdateOptionAsync(optionId, teacherId, dto);
                return Ok("Option updated successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
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
