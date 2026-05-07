using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("add")]
        public async Task<IActionResult> AddQuestionAsync([FromBody] CreateQuestionDto dto)
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
                await _questionService.AddQuestionAsync(teacherId, dto);
                return Ok("Question added successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(409, new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestionAsync(Guid questionId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                await _questionService.DeleteQuestionAsync(questionId, teacherId);
                return Ok("Question deleted successfully");
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
        }
        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet]
        public async Task<IActionResult> GetQuestionsBySubjectAsync()
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                var questions = await _questionService.GetQuestionsBySubjectAsync(teacherId);
                return Ok(questions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }

            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetQuestionTypesAsync()
        {
            var questionTypes = await _questionService.GetQuestionTypesAsync();
            return Ok(questionTypes);
        }
        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("{questionId:guid}")]
        public async Task<IActionResult> GetQuestionByIdAsync(Guid questionId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(questionId, teacherId);
                return Ok(question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
        }
        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPut("{questionId:guid}")]
        public async Task<IActionResult> UpdateQuestionAsync([FromRoute] Guid questionId, [FromBody] UpdateQuestionDto dto)
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
                await _questionService.UpdateQuestionAsync(questionId, teacherId, dto);
                return Ok("Question updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex) 
            { 
            return  BadRequest(new { error = ex.Message });
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(new {error  = ex.Message});
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }

        }
    }
}
