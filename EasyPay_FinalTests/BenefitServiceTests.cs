using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Benefit;
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
    public class BenefitServiceTests
    {
        private Mock<IBenefitRepository> _benefitRepoMock;
        private Mock<IMapper> _mapperMock;
        private BenefitService _benefitService;

        [SetUp]
        public void Setup()
        {
            _benefitRepoMock = new Mock<IBenefitRepository>();
            _mapperMock = new Mock<IMapper>();
            _benefitService = new BenefitService(_benefitRepoMock.Object, _mapperMock.Object);
        }

        //[Test]
        //public async Task AddBenefitAsync_ShouldAddBenefit_AndReturnIt()
        //{
        //    // Arrange
        //    var benefit = new Benefit { BenefitId = 1, BenefitType = "Health", Amount = 1000, EmployeeId = 2 };

        //    _benefitRepoMock.Setup(r => r.AddAsync(benefit)).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _benefitService.AddBenefitAsync(benefit);

        //    // Assert
        //    Assert.AreEqual(benefit, result);
        //    _benefitRepoMock.Verify(r => r.AddAsync(benefit), Times.Once);
        //}
        [Test]
        public async Task AddBenefitAsync_ShouldAddBenefit_AndReturnIt()
        {
            // Arrange
            var benefit = new Benefit { BenefitId = 1, BenefitType = "Health", Amount = 1000, EmployeeId = 2 };

            _benefitRepoMock.Setup(r => r.AddAsync(benefit))
                            .ReturnsAsync(benefit); 

            // Act
            var result = await _benefitService.AddBenefitAsync(benefit);

            // Assert
            Assert.AreEqual(benefit, result);
            _benefitRepoMock.Verify(r => r.AddAsync(benefit), Times.Once);
        }


        [Test]
        public void AddBenefitAsync_ShouldThrow_WhenBenefitIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _benefitService.AddBenefitAsync(null));
        }

        [Test]
        public async Task GetBenefitsByEmployeeAsync_ShouldReturnBenefits()
        {
            // Arrange
            var benefits = new List<Benefit> { new Benefit { BenefitId = 1, BenefitType = "Health", Amount = 1000, EmployeeId = 2 } };
            _benefitRepoMock.Setup(r => r.GetByEmployeeIdAsync(2)).ReturnsAsync(benefits);

            // Act
            var result = await _benefitService.GetBenefitsByEmployeeAsync(2);

            // Assert
            Assert.AreEqual(benefits, result);
            _benefitRepoMock.Verify(r => r.GetByEmployeeIdAsync(2), Times.Once);
        }

        [Test]
        public void GetBenefitsByEmployeeAsync_ShouldThrow_WhenEmployeeIdInvalid()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _benefitService.GetBenefitsByEmployeeAsync(0));
        }
        [Test]
        public async Task UpdateBenefitAsync_ShouldReturnTrue_WhenUpdated()
        {
            // Arrange
            var benefitId = 1;

            var existing = new Benefit
            {
                BenefitId = benefitId,
                BenefitType = "Health",
                Amount = 1000,
                EmployeeId = 2
            };

            var updated = new Benefit
            {
                BenefitType = "Dental",
                Amount = 2000,
                EmployeeId = 3
            };

            // Mock repository methods
            _benefitRepoMock.Setup(r => r.GetByIdAsync(benefitId))
                .ReturnsAsync(existing);

            _benefitRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Benefit>()))
    .ReturnsAsync(true);
            // This is correct since UpdateAsync returns Task

            // Act
            var result = await _benefitService.UpdateBenefitAsync(benefitId, updated);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Dental", existing.BenefitType);
            Assert.AreEqual(2000, existing.Amount);
            Assert.AreEqual(3, existing.EmployeeId);

            _benefitRepoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }


        [Test]
        public async Task UpdateBenefitAsync_ShouldReturnFalse_WhenBenefitNotFound()
        {
            _benefitRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Benefit)null);

            var result = await _benefitService.UpdateBenefitAsync(1, new Benefit());

            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateBenefitAsync_ShouldThrow_WhenBenefitIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _benefitService.UpdateBenefitAsync(1, null));
        }

        [Test]
        public async Task GetBenefitsByEmployeeAsDtoAsync_ShouldMapBenefitsToDtos()
        {
            // Arrange
            var benefits = new List<Benefit> { new Benefit { BenefitId = 1, BenefitType = "Health", Amount = 1000, EmployeeId = 2 } };
            var dtos = new List<BenefitResponseDTO> { new BenefitResponseDTO { BenefitType = "Health", Amount = 1000 } };

            _benefitRepoMock.Setup(r => r.GetByEmployeeIdAsync(2)).ReturnsAsync(benefits);
            _mapperMock.Setup(m => m.Map<IEnumerable<BenefitResponseDTO>>(benefits)).Returns(dtos);

            // Act
            var result = await _benefitService.GetBenefitsByEmployeeAsDtoAsync(2);

            // Assert
            Assert.AreEqual(dtos, result);
            _mapperMock.Verify(m => m.Map<IEnumerable<BenefitResponseDTO>>(benefits), Times.Once);
        }
    }
}
