using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Employee;
using EasyPay_Final.Repositories;
using EasyPay_Final.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> _employeeRepoMock;
        private Mock<IMapper> _mapperMock;
        private IEmployeeService _employeeService;

        [SetUp]
        public void SetUp()
        {
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();

            _employeeService = new EmployeeService(
                _employeeRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GetAllEmployeesAsync_ShouldReturnMappedDTOs()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };

            var employeeDTOs = new List<EmployeeResponseDTO>
            {
                new EmployeeResponseDTO { EmployeeId = 1, FullName = "John Doe", Email = "john@example.com" }
            };

            _employeeRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeResponseDTO>>(employees))
                       .Returns(employeeDTOs);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("John Doe", result.First().FullName);
        }

        [Test]
        public async Task GetEmployeeByIdAsync_WhenFound_ShouldReturnMappedDTO()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" };
            var employeeDTO = new EmployeeResponseDTO { EmployeeId = 1, FullName = "Jane Smith", Email = "jane@example.com" };

            _employeeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeResponseDTO>(employee)).Returns(employeeDTO);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Jane Smith", result.FullName);
        }


        [Test]
        public async Task CreateEmployeeAsync_ShouldMapAndReturnCreatedEntity()
        {
            // Arrange
            var createDTO = new EmployeeCreateDTO
            {
                FirstName = "Alice",
                LastName = "Brown",
                Email = "alice@example.com"
            };

            var employee = new Employee
            {
                EmployeeId = 2,
                FirstName = "Alice",
                LastName = "Brown",
                Email = "alice@example.com"
            };

            var employeeDTO = new EmployeeResponseDTO
            {
                EmployeeId = 2,
                FullName = "Alice Brown",
                Email = "alice@example.com"
            };

            _mapperMock.Setup(m => m.Map<Employee>(createDTO)).Returns(employee);
            _employeeRepoMock.Setup(r => r.AddAsync(employee)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeResponseDTO>(employee)).Returns(employeeDTO);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(createDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice Brown", result.FullName);
            Assert.AreEqual(2, result.EmployeeId);

            _mapperMock.Verify(m => m.Map<Employee>(createDTO), Times.Once);
            _employeeRepoMock.Verify(r => r.AddAsync(employee), Times.Once);
            _mapperMock.Verify(m => m.Map<EmployeeResponseDTO>(employee), Times.Once);
        }
    




        [Test]
        public async Task DeleteEmployeeAsync_ShouldReturnTrue_WhenDeleted()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" };

            _employeeRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(employee); // simulate found employee

            _employeeRepoMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true); // simulate delete success

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task UpdateEmployeeAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var id = 1;
            _employeeRepoMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Employee)null);

            var updatedEmployee = new Employee { FirstName = "Jane" };

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(id, updatedEmployee);

            // Assert
            Assert.IsNull(result);
            _employeeRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Never);
        }


    }
}
