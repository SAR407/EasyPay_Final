using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Timesheet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;
        private readonly IMapper _mapper;

        public TimesheetController(ITimesheetService timesheetService, IMapper mapper)
        {
            _timesheetService = timesheetService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new timesheet entry for an employee.
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(TimesheetResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            var timesheet = _mapper.Map<Timesheet>(request);
            var created = await _timesheetService.AddTimesheetEntryAsync(timesheet);
            var response = _mapper.Map<TimesheetResponseDTO>(created);

            return CreatedAtAction(nameof(GetTimesheetsByEmployee),
                new { employeeId = request.EmployeeId }, response);
        }

        /// <summary>
        /// Get all timesheets for a specific employee.
        /// </summary>
        [HttpGet("byEmployee/{employeeId}")]
        [ProducesResponseType(typeof(IEnumerable<TimesheetResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTimesheetsByEmployee(int employeeId)
        {
            if (employeeId <= 0)
                return BadRequest("Invalid employee ID.");

            var timesheets = await _timesheetService.GetTimesheetsByEmployeeAsync(employeeId);
            var response = _mapper.Map<IEnumerable<TimesheetResponseDTO>>(timesheets);

            return Ok(response);
        }

        /// <summary>
        /// Approve a pending timesheet entry.
        /// </summary>
        [HttpPut("approve/{timesheetId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApproveTimesheet(int timesheetId, [FromQuery] int managerId)
        {
            if (timesheetId <= 0 || managerId <= 0)
                return BadRequest("Invalid timesheet ID or manager ID.");

            var success = await _timesheetService.ApproveTimesheetAsync(timesheetId, managerId);

            if (!success)
                return NotFound("Timesheet not found or cannot be approved.");

            return Ok("Timesheet approved successfully.");
        }
    }
}
