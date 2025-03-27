using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services;

namespace UpDownForms.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private UpDownFormsContext _context;
        private IPasswordHelper _passwordHelper;
        private UserManager<User> _userManager;
        private IUserService _userServiceMock;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<UpDownFormsContext>(new DbContextOptions<UpDownFormsContext>());
            _passwordHelper = Substitute.For<IPasswordHelper>();
            _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _userServiceMock = Substitute.For<IUserService>();
            _userService = new UserService(_context, _passwordHelper, _userManager, _userServiceMock);
        }

        [Test]
        public async Task PostUser_ShouldCreateUser_WhenValidData()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123@"
            };

            var user = new User
            {
                UserName = createUserDTO.Email,
                Name = createUserDTO.Name,
                Email = createUserDTO.Email,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Forms = new List<Form>()
            };

            _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
            var result = await _userService.PostUser(createUserDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createUserDTO.Name, result.Name);
            Assert.AreEqual(createUserDTO.Email, result.Email);
            
            
        }

        [Test]
        public void PostUser_ShouldThrowException_WhenUserCreationFails()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Failed());

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _userService.PostUser(createUserDTO));
        }

        //[Test]
        //public void 
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _userManager.Dispose();
        }

        
        
    }
}
