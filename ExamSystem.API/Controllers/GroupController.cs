using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("add")]
        public async Task<IActionResult> AddGroup([FromBody] CreateGroupDto grouptDto)
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
                var addedGroup = await _groupService.AddGroupAsync(teacherId, grouptDto);
                return Ok(addedGroup);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);        
            }
            catch (InvalidDataException ex)
            {
                return Conflict(ex.Message);          
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);          
            }
        }
        [Authorize(Roles =nameof(RoleName.Admin))]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            var groups = await _groupService.GetAllGroupsAsync();

            return Ok(groups);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGroupByIdAsync([FromRoute] Guid id)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(id);
                return Ok(group);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("teacher/all")]
        public async Task<IActionResult> GetTeacherGroupsAsync()
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var groups = await _groupService.GetTeacherGroupsAsync(teacherId);
                return Ok(groups);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
