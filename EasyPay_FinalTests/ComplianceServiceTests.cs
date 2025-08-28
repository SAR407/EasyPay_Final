using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Compilance;
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
    public class ComplianceServiceTests
    {
        private Mock<IComplianceReportRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private ComplianceService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IComplianceReportRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ComplianceService(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GenerateTaxReportsAsync_ShouldReturnFilteredReports()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 12, 31);

            var reports = new List<ComplianceReport>
            {
                new ComplianceReport { ReportId = 1,  ReportType  = "Tax", StartDate = new DateTime(2024, 2, 1), EndDate = new DateTime(2024, 5, 1) },
                new ComplianceReport { ReportId = 2,  ReportType = "Tax", StartDate = new DateTime(2023, 2, 1), EndDate = new DateTime(2023, 5, 1) } // outside range
            };

            _repoMock.Setup(r => r.GetByTypeAsync("Tax")).ReturnsAsync(reports);

            // Act
            var result = await _service.GenerateTaxReportsAsync(startDate, endDate);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().ReportId);
        }

        [Test]
        public async Task GenerateStatutoryReportsAsync_ShouldReturnAllStatutoryReports()
        {
            // Arrange
            var reports = new List<ComplianceReport>
            {
                new ComplianceReport { ReportId = 1, ReportType  = "Statutory" },
                new ComplianceReport { ReportId = 2, ReportType = "Statutory" }
            };

            _repoMock.Setup(r => r.GetByTypeAsync("Statutory")).ReturnsAsync(reports);

            // Act
            var result = await _service.GenerateStatutoryReportsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GenerateTaxReportsDtoAsync_ShouldMapAndReturnDTOs()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 12, 31);

            var reports = new List<ComplianceReport>
            {
                new ComplianceReport { ReportId = 1,  ReportType  = "Tax", StartDate = new DateTime(2024, 2, 1), EndDate = new DateTime(2024, 5, 1) }
            };

            var dtoReports = new List<ComplianceReportResponseDTO>
            {
                new ComplianceReportResponseDTO { ReportId = 1 }
            };

            _repoMock.Setup(r => r.GetByTypeAsync("Tax")).ReturnsAsync(reports);
            _mapperMock.Setup(m => m.Map<IEnumerable<ComplianceReportResponseDTO>>(It.IsAny<IEnumerable<ComplianceReport>>()))
                       .Returns(dtoReports);

            // Act
            var result = await _service.GenerateTaxReportsDtoAsync(startDate, endDate);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().ReportId);
        }

        [Test]
        public async Task GenerateStatutoryReportsDtoAsync_ShouldMapAndReturnDTOs()
        {
            // Arrange
            var reports = new List<ComplianceReport>
            {
                new ComplianceReport { ReportId = 1, ReportType = "Statutory" }
            };

            var dtoReports = new List<ComplianceReportResponseDTO>
            {
                new ComplianceReportResponseDTO { ReportId = 1 }
            };

            _repoMock.Setup(r => r.GetByTypeAsync("Statutory")).ReturnsAsync(reports);
            _mapperMock.Setup(m => m.Map<IEnumerable<ComplianceReportResponseDTO>>(reports)).Returns(dtoReports);

            // Act
            var result = await _service.GenerateStatutoryReportsDtoAsync();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().ReportId);
        }
    }
}
