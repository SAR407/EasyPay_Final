using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.LeaveRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IMapper _mapper;

        public LeaveRequestController(ILeaveService leaveService, IMapper mapper)
        {
            _leaveService = leaveService;
            _mapper = mapper;
        }

        /// <summary>
        /// Submit a new leave request (Employee)
        /// </summary>
        [HttpPost("submit")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<LeaveResponseDTO>> SubmitLeaveRequest([FromBody] LeaveRequestDTO requestDto)
        {
            try
            {
                var leaveEntity = _mapper.Map<LeaveRequest>(requestDto);
                var createdLeave = await _leaveService.SubmitLeaveRequestAsync(leaveEntity);
                var responseDto = _mapper.Map<LeaveResponseDTO>(createdLeave);
                return CreatedAtAction(nameof(GetLeaveRequestsByEmployee), new { employeeId = createdLeave.EmployeeId }, responseDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get leave requests for a specific employee (Employee can only see their own, HR/Admin can see all)
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<IEnumerable<LeaveResponseDTO>>> GetLeaveRequestsByEmployee(int employeeId)
        {
            var leaves = await _leaveService.GetLeaveRequestsByEmployeeAsync(employeeId);
            return Ok(_mapper.Map<IEnumerable<LeaveResponseDTO>>(leaves));
        }

        /// <summary>
        /// Approve a leave request (HR/Admin)
        /// </summary>
        [HttpPut("approve/{leaveRequestId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> ApproveLeave(int leaveRequestId, [FromQuery] int managerId)
        {
            var success = await _leaveService.ApproveLeaveAsync(leaveRequestId, managerId);
            if (!success)
                return NotFound(new { message = $"Leave request with ID {leaveRequestId} not found." });

            return Ok(new { message = "Leave approved successfully." });
        }

        /// <summary>
        /// Reject a leave request (HR/Admin)
        /// </summary>
        [HttpPut("reject/{leaveRequestId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> RejectLeave(int leaveRequestId, [FromQuery] int managerId)
        {
            var success = await _leaveService.RejectLeaveAsync(leaveRequestId, managerId);
            if (!success)
                return NotFound(new { message = $"Leave request with ID {leaveRequestId} not found." });

            return Ok(new { message = "Leave rejected successfully." });
        }
    }
}
