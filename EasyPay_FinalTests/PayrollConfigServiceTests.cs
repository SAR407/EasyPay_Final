using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Repositories;
using EasyPay_Final.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Tests.Services
{
    [TestFixture]
    public class PayrollConfigServiceTests
    {
        private Mock<IRepository<PayrollPolicy>> _repoMock;
        private PayrollConfigService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IRepository<PayrollPolicy>>();
            _service = new PayrollConfigService(_repoMock.Object);
        }

        [Test]
        public async Task ConfigurePayrollRulesAsync_ShouldUpdate_WhenPolicyExists()
        {
            // Arrange
            var existingPolicy = new PayrollPolicy
            {
                PayrollPolicyId = 1,
                TaxRate = 5,
                Allowances = 1000,
                Deductions = 200
            };
            var newConfig = new PayrollPolicy
            {
                TaxRate = 10,
                Allowances = 2000,
                Deductions = 300
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<PayrollPolicy> { existingPolicy });

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<PayrollPolicy>()))
                     .ReturnsAsync(true);

            // Act
            await _service.ConfigurePayrollRulesAsync(newConfig);

            // Assert
            Assert.AreEqual(10, existingPolicy.TaxRate);
            Assert.AreEqual(2000, existingPolicy.Allowances);
            Assert.AreEqual(300, existingPolicy.Deductions);
            _repoMock.Verify(r => r.UpdateAsync(existingPolicy), Times.Once);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<PayrollPolicy>()), Times.Never);
        }

        [Test]
        public async Task ConfigurePayrollRulesAsync_ShouldAdd_WhenNoPolicyExists()
        {
            // Arrange
            var newConfig = new PayrollPolicy
            {
                TaxRate = 15,
                Allowances = 2500,
                Deductions = 500
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<PayrollPolicy>());

            _repoMock.Setup(r => r.AddAsync(newConfig))
                     .ReturnsAsync(newConfig);

            // Act
            await _service.ConfigurePayrollRulesAsync(newConfig);

            // Assert
            _repoMock.Verify(r => r.AddAsync(newConfig), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<PayrollPolicy>()), Times.Never);
        }

        [Test]
        public void ConfigurePayrollRulesAsync_ShouldThrow_WhenConfigIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.ConfigurePayrollRulesAsync(null));
        }

        [Test]
        public async Task GetPayrollConfigAsync_ShouldReturnFirstPolicy_WhenExists()
        {
            // Arrange
            var policyList = new List<PayrollPolicy>
            {
                new PayrollPolicy { PayrollPolicyId = 1, TaxRate = 8 }
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(policyList);

            // Act
            var result = await _service.GetPayrollConfigAsync();

            // Assert
            Assert.AreEqual(1, result.PayrollPolicyId);
            Assert.AreEqual(8, result.TaxRate);
        }

        [Test]
        public async Task GetPayrollConfigAsync_ShouldReturnNull_WhenNoPolicyExists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<PayrollPolicy>());

            // Act
            var result = await _service.GetPayrollConfigAsync();

            // Assert
            Assert.IsNull(result);
        }
    }
}
