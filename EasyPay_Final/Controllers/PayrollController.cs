using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all payroll endpoints
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        private readonly IMapper _mapper;

        public PayrollController(IPayrollService payrollService, IMapper mapper)
        {
            _payrollService = payrollService;
            _mapper = mapper;
        }

        /// <summary>
        /// Calculate payroll for a given employee and date.
        /// </summary>
        [HttpPost("calculate")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<PayrollResponseDTO>> CalculatePayroll([FromQuery] int employeeId, [FromQuery] DateTime payrollDate)
        {
            try
            {
                var payroll = await _payrollService.CalculatePayrollAsync(employeeId, payrollDate);
                var response = _mapper.Map<PayrollResponseDTO>(payroll);
                return CreatedAtAction(nameof(GetPayrollByEmployee), new { employeeId = payroll.EmployeeId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verify payroll before processing.
        /// </summary>
        [HttpGet("verify/{payrollId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<bool>> VerifyPayroll(int payrollId)
        {
            var isValid = await _payrollService.VerifyPayrollAsync(payrollId);
            return Ok(isValid);
        }

        /// <summary>
        /// Process payment for a specific payroll entry.
        /// </summary>
        [HttpPost("process/{payrollId}")]
        [Authorize(Roles = "Admin,Finance")]
        public async Task<ActionResult> ProcessPayments(int payrollId)
        {
            try
            {
                var processed = await _payrollService.ProcessPaymentsAsync(payrollId);
                if (!processed)
                    return NotFound(new { message = "Payroll not found." });

                return Ok(new { message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get payroll history for an employee.
        /// </summary>
        [HttpGet("history/{employeeId}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<IEnumerable<PayrollResponseDTO>>> GetPayrollByEmployee(int employeeId)
        {
            var payrollHistory = await _payrollService.GetPayrollHistoryAsync(employeeId);
            var payrollHistoryDto = _mapper.Map<IEnumerable<PayrollResponseDTO>>(payrollHistory);
            return Ok(payrollHistoryDto);

        }
    }
}

