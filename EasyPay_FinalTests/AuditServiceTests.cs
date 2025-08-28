using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Audit;
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
    public class AuditServiceTests
    {
        private Mock<IAuditLogRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private AuditService _auditService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IAuditLogRepository>();
            _mapperMock = new Mock<IMapper>();
            _auditService = new AuditService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task LogActionAsync_ShouldAddAuditLog_WhenParametersAreValid()
        {
            // Arrange
            var performedBy = "Admin";
            var action = "Create User";
            var details = "User created successfully";

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<AuditLog>()))
                .ReturnsAsync((AuditLog log) => log);

            // Act
            await _auditService.LogActionAsync(performedBy, action, details);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.Is<AuditLog>(
                log => log.PerformedBy == performedBy &&
                       log.Action == action &&
                       log.Details == details &&
                       log.Timestamp <= DateTime.UtcNow
            )), Times.Once);
        }

        [Test]
        public void LogActionAsync_ShouldThrowException_WhenPerformedByIsEmpty()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _auditService.LogActionAsync("", "Some Action", "Details"));

            Assert.That(ex.ParamName, Is.EqualTo("performedBy"));
        }

        [Test]
        public void LogActionAsync_ShouldThrowException_WhenActionIsEmpty()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _auditService.LogActionAsync("Admin", "", "Details"));

            Assert.That(ex.ParamName, Is.EqualTo("action"));
        }

        [Test]
        public async Task GetAuditLogsAsync_ShouldReturnLogsFromRepository()
        {
            // Arrange
            var logs = new List<AuditLog>
            {
                new AuditLog { AuditLogId = 1, PerformedBy = "Admin", Action = "Test", Details = "Details" }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(logs);

            // Act
            var result = await _auditService.GetAuditLogsAsync();

            // Assert
            Assert.AreEqual(1, ((List<AuditLog>)result).Count);
            Assert.AreEqual("Admin", ((List<AuditLog>)result)[0].PerformedBy);
        }

        [Test]
        public async Task GetAuditLogsAsDtoAsync_ShouldMapEntitiesToDtos()
        {
            // Arrange
            var logs = new List<AuditLog>
            {
                new AuditLog {  AuditLogId = 1, PerformedBy = "Admin", Action = "Login", Details = "Success" }
            };

            var dtoLogs = new List<AuditLogResponseDTO>
            {
                new AuditLogResponseDTO { PerformedBy = "Admin", Action = "Login", Details = "Success" }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(logs);
            _mapperMock.Setup(m => m.Map<IEnumerable<AuditLogResponseDTO>>(logs))
                       .Returns(dtoLogs);

            // Act
            var result = await _auditService.GetAuditLogsAsDtoAsync();

            // Assert
            Assert.AreEqual(1, ((List<AuditLogResponseDTO>)result).Count);
            Assert.AreEqual("Admin", ((List<AuditLogResponseDTO>)result)[0].PerformedBy);
        }
    }
}
