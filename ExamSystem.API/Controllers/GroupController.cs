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
        [Authorize(Roles = $"{nameof(RoleName.Teacher)},{nameof(RoleName.Admin)}")]
        [HttpPost("add")]
        public async Task<IActionResult> AddGroup([FromBody] CreateGroupDto grouptDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedGroup = await _groupService.AddGroupAsync(grouptDto);
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
