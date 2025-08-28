using AutoMapper;
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
    public class LeaveServiceTests
    {
        private Mock<ILeaveRequestRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private LeaveService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<ILeaveRequestRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new LeaveService(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task SubmitLeaveRequestAsync_ShouldSetPendingStatus_AndReturnLeaveRequest()
        {
            // Arrange
            var leave = new LeaveRequest { LeaveRequestId = 1, EmployeeId = 2, Status = null };
            _repoMock.Setup(r => r.AddAsync(leave)).ReturnsAsync(leave);

            // Act
            var result = await _service.SubmitLeaveRequestAsync(leave);

            // Assert
            Assert.AreEqual("Pending", result.Status);
            _repoMock.Verify(r => r.AddAsync(leave), Times.Once);
        }

        [Test]
        public void SubmitLeaveRequestAsync_ShouldThrow_WhenLeaveRequestIsNull()
        {
            // Arrange & Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.SubmitLeaveRequestAsync(null));
        }

        [Test]
        public async Task GetLeaveRequestsByEmployeeAsync_ShouldReturnRequests()
        {
            // Arrange
            var leaves = new List<LeaveRequest>
            {
                new LeaveRequest { LeaveRequestId = 1, EmployeeId = 2, Status = "Pending" }
            };
            _repoMock.Setup(r => r.GetByEmployeeIdAsync(2)).ReturnsAsync(leaves);

            // Act
            var result = await _service.GetLeaveRequestsByEmployeeAsync(2);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().EmployeeId);
        }

        [Test]
        public async Task ApproveLeaveAsync_ShouldReturnTrue_WhenRequestExists()
        {
            // Arrange
            var leave = new LeaveRequest { LeaveRequestId = 1, Status = "Pending" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
            _repoMock.Setup(r => r.UpdateAsync(leave)).ReturnsAsync(true);

            // Act
            var result = await _service.ApproveLeaveAsync(1, 100);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Approved", leave.Status);
            _repoMock.Verify(r => r.UpdateAsync(leave), Times.Once);
        }

        [Test]
        public async Task ApproveLeaveAsync_ShouldReturnFalse_WhenRequestNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _service.ApproveLeaveAsync(1, 100);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RejectLeaveAsync_ShouldReturnTrue_WhenRequestExists()
        {
            // Arrange
            var leave = new LeaveRequest { LeaveRequestId = 1, Status = "Pending" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
            _repoMock.Setup(r => r.UpdateAsync(leave)).ReturnsAsync(true);

            // Act
            var result = await _service.RejectLeaveAsync(1, 100);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Rejected", leave.Status);
            _repoMock.Verify(r => r.UpdateAsync(leave), Times.Once);
        }

        [Test]
        public async Task RejectLeaveAsync_ShouldReturnFalse_WhenRequestNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _service.RejectLeaveAsync(1, 100);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
