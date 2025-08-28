using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Benefit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly IBenefitService _benefitService;
        private readonly IMapper _mapper;

        public BenefitController(IBenefitService benefitService, IMapper mapper)
        {
            _benefitService = benefitService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new benefit for an employee (Admin/HR only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<BenefitResponseDTO>> AddBenefit([FromBody] BenefitRequestDTO requestDto)
        {
            try
            {
                var benefitEntity = _mapper.Map<Benefit>(requestDto);
                var createdBenefit = await _benefitService.AddBenefitAsync(benefitEntity);
                var responseDto = _mapper.Map<BenefitResponseDTO>(createdBenefit);
                return CreatedAtAction(nameof(GetBenefitsByEmployee), new { employeeId = createdBenefit.EmployeeId }, responseDto);
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
        /// Get all benefits for a specific employee (Admin, HR, or that Employee)
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<IEnumerable<BenefitResponseDTO>>> GetBenefitsByEmployee(int employeeId)
        {
            try
            {
                var benefits = await _benefitService.GetBenefitsByEmployeeAsync(employeeId);
                var dtoList = _mapper.Map<IEnumerable<BenefitResponseDTO>>(benefits);
                return Ok(dtoList);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update benefit details (Admin/HR only)
        /// </summary>
        [HttpPut("{benefitId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateBenefit(int benefitId, [FromBody] BenefitRequestDTO requestDto)
        {
            try
            {
                var benefitEntity = _mapper.Map<Benefit>(requestDto);
                var success = await _benefitService.UpdateBenefitAsync(benefitId, benefitEntity);

                if (!success)
                    return NotFound(new { message = $"Benefit with ID {benefitId} not found." });

                return Ok(new { message = "Benefit updated successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
