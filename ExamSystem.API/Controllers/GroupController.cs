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
    }
}
