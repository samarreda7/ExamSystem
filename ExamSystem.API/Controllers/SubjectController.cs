using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [Authorize(Roles = nameof(RoleName.Admin))]
        [HttpPost("add")]
        public async Task<IActionResult> AddSubject([FromBody]CreateSubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _subjectService.AddSubjectAsync(subjectDto);
                return Ok("Subject added successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet] 
        public async Task<IActionResult> GetAllSubjectAsync()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
        
            return Ok(subjects);
        }
    }
}
