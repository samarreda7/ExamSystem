using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentGroupController : ControllerBase
    {
        private readonly IStudentGroupService _studentGroupService;
        public StudentGroupController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }


        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("group/{groupId:guid}")]
        public async Task<IActionResult> GetStudentsByGroupIdAsync(Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
               var students= await _studentGroupService.GetStudentsByGroupIdAsync(groupId, teacherId);
                return Ok(students);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("group/{groupId:guid}/students/count")]
        public async Task<IActionResult> GetStudentCountByGroupIdAsync(Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var count = await _studentGroupService.GetStudentCountByGroupIdAsync(groupId, teacherId);
                return Ok(count);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
        }

        [Authorize(Roles = nameof(RoleName.Student))]
        [HttpGet("my-groups")]
        public async Task<IActionResult> GetMyGroupsAsync()
        {
            var studentIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(studentIdClaim) || !Guid.TryParse(studentIdClaim, out Guid studentId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var groups = await _studentGroupService.GetGroupsByStudentIdAsync(studentId);
                return Ok(groups);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignStudentToGroupAsync([FromBody]AssignStudentToGroupDto dto)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                await _studentGroupService.AssignStudentToGroupAsync(dto.StudentId, dto.GroupId, teacherId);
                return Ok("Student assigned successfully");
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpGet("{groupId:guid}/students/{studentId:guid}/is-assigned")]
        public async Task<IActionResult> IsStudentAssignedToGroupAsync([FromRoute] Guid groupId, [FromRoute] Guid studentId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                var isAssigned = await _studentGroupService.IsStudentAssignedToGroupAsync(studentId, groupId, teacherId);
                return Ok(isAssigned);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
        }


        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpPut("reassign")]
        public async Task<IActionResult> ReassignStudentToAnotherGroupAsync([FromBody]ReassignStudentToAnotherGroupDto dto)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }

            try
            {
                await _studentGroupService.ReassignStudentToAnotherGroupAsync(dto.GroupId, dto.StudentId, dto.newGroupId, teacherId);
                return Ok("Student Reassigned successfully");
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }

        }

        [Authorize(Roles = nameof(RoleName.Teacher))]
        [HttpDelete("{groupId}/students/{studentId}")]
        public async Task<IActionResult> DeleteStudentAssignToGroupAsync([FromRoute]Guid studentId,[FromRoute]Guid groupId)
        {
            var teacherIdClaim = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(teacherIdClaim) || !Guid.TryParse(teacherIdClaim, out Guid teacherId))
            {
                return Unauthorized("Invalid token claims.");
            }
            try
            {
                await _studentGroupService.DeleteStudentAssignToGroupAsync(studentId, groupId, teacherId);
                return Ok("Student assignment deleted successfully");
            }
            catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }

        }
    }
}
