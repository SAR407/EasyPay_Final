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
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IMapper> _mapperMock;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new UserService(_userRepoMock.Object, _mapperMock.Object);
        }

        // ---------- GetAllUsersAsync ----------
        [Test]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            var users = new List<User> { new User { UserId = 1, Username = "Alice" } };
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _service.GetAllUsersAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Alice", result.First().Username);
            _userRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        // ---------- GetUserByIdAsync ----------
        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenValidId()
        {
            var user = new User { UserId = 1, Username = "Bob" };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.AreEqual("Bob", result.Username);
            _userRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public void GetUserByIdAsync_ShouldThrow_WhenInvalidId()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.GetUserByIdAsync(0));
            _userRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        // ---------- CreateUserAsync ----------
        [Test]
        public async Task CreateUserAsync_ShouldHashPassword_AndAddUser()
        {
            var user = new User { UserId = 1, Username = "Charlie" };
            string plainPassword = "Test123";

            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            var result = await _service.CreateUserAsync(user, plainPassword);

            Assert.NotNull(result.PasswordHash, "PasswordHash should not be null.");
            Assert.AreNotEqual(plainPassword, result.PasswordHash, "PasswordHash should not equal the plain password.");
            Assert.IsTrue(result.IsActive);
            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Username == "Charlie")), Times.Once);
        }

        [Test]
        public void CreateUserAsync_ShouldThrow_WhenUserIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateUserAsync(null, "password"));
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void CreateUserAsync_ShouldThrow_WhenPasswordIsEmpty()
        {
            var user = new User();
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateUserAsync(user, " "));
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        // ---------- UpdateUserAsync ----------
        [Test]
        public async Task UpdateUserAsync_ShouldUpdateFields_WhenUserExists()
        {
            var existing = new User { UserId = 1, Username = "Old", Email = "old@mail.com", RoleId = 1, IsActive = true };
            var updated = new User { Username = "New", Email = "new@mail.com", RoleId = 2, IsActive = false };

            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _userRepoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

            var result = await _service.UpdateUserAsync(1, updated);

            Assert.AreEqual("New", result.Username);
            Assert.AreEqual("new@mail.com", result.Email);
            Assert.AreEqual(2, result.RoleId);
            Assert.IsFalse(result.IsActive);
            _userRepoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Test]
        public void UpdateUserAsync_ShouldThrow_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateUserAsync(1, new User()));
            _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        // ---------- DeleteUserAsync ----------
        [Test]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var existing = new User { UserId = 1 };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _userRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteUserAsync(1);

            Assert.IsTrue(result);
            _userRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);

            var result = await _service.DeleteUserAsync(1);

            Assert.IsFalse(result);
            _userRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
