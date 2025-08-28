using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Payroll;
using EasyPay_Final.Repositories;
using EasyPay_Final.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Tests.Services
{
    [TestFixture]
    public class PayrollServiceTests
    {
        private Mock<IPayrollRepository> _payrollRepoMock;
        private Mock<IEmployeeRepository> _employeeRepoMock;
        private Mock<IMapper> _mapperMock;
        private PayrollService _service;

        [SetUp]
        public void SetUp()
        {
            _payrollRepoMock = new Mock<IPayrollRepository>();
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PayrollService(_payrollRepoMock.Object, _employeeRepoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CalculatePayrollAsync_ShouldReturnPayroll_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = 1;
            var payrollDate = DateTime.Today;
            var employee = new Employee
            {
                EmployeeId = employeeId,
                BasicSalary = 5000,
                Allowances = 500,
                Deductions = 200
            };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(employee);
            _payrollRepoMock.Setup(r => r.AddAsync(It.IsAny<Payroll>())).ReturnsAsync((Payroll p) => p);

            // Act
            var result = await _service.CalculatePayrollAsync(employeeId, payrollDate);

            // Assert
            Assert.AreEqual(employeeId, result.EmployeeId);
            Assert.AreEqual(5300, result.GrossSalary); // 5000 + 500 - 200
            Assert.AreEqual(530, result.TaxAmount);    // 10% tax
            Assert.AreEqual(4770, result.NetSalary);
            _payrollRepoMock.Verify(r => r.AddAsync(It.IsAny<Payroll>()), Times.Once);
        }

        [Test]
        public void CalculatePayrollAsync_ShouldThrow_WhenEmployeeNotFound()
        {
            _employeeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee)null);

            Assert.ThrowsAsync<Exception>(() => _service.CalculatePayrollAsync(1, DateTime.Today));
        }

        [Test]
        public async Task VerifyPayrollAsync_ShouldReturnTrue_WhenValid()
        {
            var payroll = new Payroll
            {
                GrossSalary = 5000,
                NetSalary = 4000
            };
            _payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payroll);

            var result = await _service.VerifyPayrollAsync(1);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task VerifyPayrollAsync_ShouldReturnFalse_WhenInvalid()
        {
            _payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Payroll)null);

            var result = await _service.VerifyPayrollAsync(1);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ProcessPaymentsAsync_ShouldMarkAsProcessed_WhenNotProcessed()
        {
            var payroll = new Payroll { IsProcessed = false };
            _payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payroll);
            _payrollRepoMock.Setup(r => r.UpdateAsync(payroll)).ReturnsAsync(true);

            var result = await _service.ProcessPaymentsAsync(1);

            Assert.IsTrue(result);
            Assert.IsTrue(payroll.IsProcessed);
            _payrollRepoMock.Verify(r => r.UpdateAsync(payroll), Times.Once);
        }

        [Test]
        public void ProcessPaymentsAsync_ShouldThrow_WhenAlreadyProcessed()
        {
            var payroll = new Payroll { IsProcessed = true };
            _payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payroll);

            Assert.ThrowsAsync<Exception>(() => _service.ProcessPaymentsAsync(1));
        }

        [Test]
        public async Task ProcessPaymentsAsync_ShouldReturnFalse_WhenNotFound()
        {
            _payrollRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Payroll)null);

            var result = await _service.ProcessPaymentsAsync(1);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetPayrollHistoryAsync_ShouldReturnPayrolls()
        {
            var payrolls = new List<Payroll> { new Payroll { EmployeeId = 1 } };
            _payrollRepoMock.Setup(r => r.GetByEmployeeIdAsync(1)).ReturnsAsync(payrolls);

            var result = await _service.GetPayrollHistoryAsync(1);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetPayrollHistoryDtoAsync_ShouldMapToDTOs()
        {
            var payrolls = new List<Payroll> { new Payroll { EmployeeId = 1 } };
            var dtoList = new List<PayrollResponseDTO> { new PayrollResponseDTO { EmployeeId = 1 } };

            _payrollRepoMock.Setup(r => r.GetByEmployeeIdAsync(1)).ReturnsAsync(payrolls);
            _mapperMock.Setup(m => m.Map<IEnumerable<PayrollResponseDTO>>(payrolls)).Returns(dtoList);

            var result = await _service.GetPayrollHistoryDtoAsync(1);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().EmployeeId);
        }
    }
}
