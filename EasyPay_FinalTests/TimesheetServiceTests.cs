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
    public class TimesheetServiceTests
    {
        private Mock<ITimesheetRepository> _timesheetRepoMock;
        private Mock<IMapper> _mapperMock;
        private TimesheetService _service;

        [SetUp]
        public void Setup()
        {
            _timesheetRepoMock = new Mock<ITimesheetRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new TimesheetService(_timesheetRepoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task AddTimesheetEntryAsync_ShouldSetStatusPending_AndAdd()
        {
            var timesheet = new Timesheet
            {
                TimesheetId = 1,
                EmployeeId = 5,
                HoursWorked = 8,
                Date = DateTime.Today
            };

            _timesheetRepoMock
                .Setup(r => r.AddAsync(timesheet))
                .ReturnsAsync(timesheet);

            var result = await _service.AddTimesheetEntryAsync(timesheet);

            Assert.AreEqual("Pending", result.Status);
            Assert.AreEqual(timesheet, result);
            _timesheetRepoMock.Verify(r => r.AddAsync(timesheet), Times.Once);
        }

        [Test]
        public void AddTimesheetEntryAsync_ShouldThrow_WhenTimesheetIsNull()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddTimesheetEntryAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("timesheet"));
            _timesheetRepoMock.Verify(r => r.AddAsync(It.IsAny<Timesheet>()), Times.Never);
        }

        [Test]
        public async Task GetTimesheetsByEmployeeAsync_ShouldReturnList_WhenValidId()
        {
            int employeeId = 2;
            var timesheets = new List<Timesheet>
            {
                new Timesheet { TimesheetId = 1, EmployeeId = employeeId, HoursWorked = 8, Date = DateTime.Today },
                new Timesheet { TimesheetId = 2, EmployeeId = employeeId, HoursWorked = 7, Date = DateTime.Today.AddDays(-1) }
            };

            _timesheetRepoMock
                .Setup(r => r.GetByEmployeeIdAsync(employeeId))
                .ReturnsAsync(timesheets);

            var result = await _service.GetTimesheetsByEmployeeAsync(employeeId);

            Assert.AreEqual(2, result.Count());
            _timesheetRepoMock.Verify(r => r.GetByEmployeeIdAsync(employeeId), Times.Once);
        }

        [Test]
        public void GetTimesheetsByEmployeeAsync_ShouldThrow_WhenIdIsInvalid()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.GetTimesheetsByEmployeeAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("employeeId"));
            _timesheetRepoMock.Verify(r => r.GetByEmployeeIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task ApproveTimesheetAsync_ShouldReturnTrue_WhenStatusIsPending()
        {
            int timesheetId = 1;
            var existing = new Timesheet
            {
                TimesheetId = timesheetId,
                EmployeeId = 5,
                Status = "Pending"
            };

            _timesheetRepoMock
                .Setup(r => r.GetByIdAsync(timesheetId))
                .ReturnsAsync(existing);

            _timesheetRepoMock
                .Setup(r => r.UpdateAsync(existing))
                .ReturnsAsync(true);

            var result = await _service.ApproveTimesheetAsync(timesheetId, 10);

            Assert.IsTrue(result);
            Assert.AreEqual("Approved", existing.Status);
            _timesheetRepoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Test]
        public async Task ApproveTimesheetAsync_ShouldReturnFalse_WhenTimesheetNotFound()
        {
            _timesheetRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Timesheet)null);

            var result = await _service.ApproveTimesheetAsync(1, 1);

            Assert.IsFalse(result);
            _timesheetRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Timesheet>()), Times.Never);
        }

        [Test]
        public async Task ApproveTimesheetAsync_ShouldReturnFalse_WhenNotPending()
        {
            var existing = new Timesheet
            {
                TimesheetId = 1,
                EmployeeId = 5,
                Status = "Approved"
            };

            _timesheetRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existing);

            var result = await _service.ApproveTimesheetAsync(1, 1);

            Assert.IsFalse(result);
            _timesheetRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Timesheet>()), Times.Never);
        }
    }
}
