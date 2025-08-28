using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Payroll;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public PayrollService(IPayrollRepository payrollRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<Payroll> CalculatePayrollAsync(int employeeId, DateTime payrollDate)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            // Example calculation logic
            var grossSalary = employee.BasicSalary + employee.Allowances - employee.Deductions;
            var taxAmount = grossSalary * 0.1m; // 10% tax example
            var netSalary = grossSalary - taxAmount;

            var payroll = new Payroll
            {
                EmployeeId = employeeId,
                PayrollDate = payrollDate,
                GrossSalary = grossSalary,
                TaxAmount = taxAmount,
                NetSalary = netSalary,
                IsProcessed = false
            };

            //return await _payrollRepository.AddAsync(payroll);
            await _payrollRepository.AddAsync(payroll);
            return payroll;

        }

        public async Task<bool> VerifyPayrollAsync(int payrollId)
        {
            var payroll = await _payrollRepository.GetByIdAsync(payrollId);
            if (payroll == null)
                return false;

            // Add verification rules here
            return payroll.NetSalary > 0 && payroll.GrossSalary >= payroll.NetSalary;
        }

        public async Task<bool> ProcessPaymentsAsync(int payrollId)
        {
            var payroll = await _payrollRepository.GetByIdAsync(payrollId);
            if (payroll == null)
                return false;

            if (payroll.IsProcessed)
                throw new Exception("Payroll already processed");

            // Payment processing logic could go here
            payroll.IsProcessed = true;
            await _payrollRepository.UpdateAsync(payroll);

            return true;
        }

        public async Task<IEnumerable<Payroll>> GetPayrollHistoryAsync(int employeeId)
        {
            return await _payrollRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Optional: DTO-based methods for controllers
        public async Task<IEnumerable<PayrollResponseDTO>> GetPayrollHistoryDtoAsync(int employeeId)
        {
            var payrolls = await GetPayrollHistoryAsync(employeeId);
            return _mapper.Map<IEnumerable<PayrollResponseDTO>>(payrolls);
        }
    }
}
