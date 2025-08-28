using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensures all endpoints require authentication unless explicitly overridden
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees (Only Admin & HR can access)
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDTO>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Get employee by ID (Accessible to Admin, HR, or the employee themself)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<EmployeeResponseDTO>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                // Optional self-access check if role is Employee
                if (User.IsInRole("Employee"))
                {
                    var userIdClaim = User.FindFirst("UserId")?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || employee.EmployeeId.ToString() != userIdClaim)
                        return Forbid(); // Prevent Employee from accessing another Employee's data
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new employee (Admin or HR only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<EmployeeResponseDTO>> CreateEmployee([FromBody] EmployeeCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEmployee = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);
        }

        /// <summary>
        /// Update employee details (Admin & HR)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            var updated = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (updated == null)
                return NotFound(new { Message = $"Employee with ID {id} not found." });

            return Ok(updated);
        }

        /// <summary>
        /// Delete an employee (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Employee with ID {id} not found." });

            return NoContent();
        }
    }
}
