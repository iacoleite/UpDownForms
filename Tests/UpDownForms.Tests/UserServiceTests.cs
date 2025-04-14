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
using Microsoft.AspNetCore.Http;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        private IUpDownFormsContext _context;
        private UserManager<User> _userManager;
        private IHttpContextAccessor _iHttpContextAcessor;
        private ILoggedUserService _loggedUserServiceMock;


        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<UpDownFormsContext>(new DbContextOptions<UpDownFormsContext>());
            //_userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _iHttpContextAcessor = Substitute.For<IHttpContextAccessor>();
            //_loggedUserServiceMock = Substitute.For<LoggedUserService>();
            //_loggedUserServiceMock = new LoggedUserService(_iHttpContextAcessor);
            _loggedUserServiceMock = Substitute.For<ILoggedUserService>();

            _userService = new UserService(_context, _userManager, _loggedUserServiceMock);
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

        //[Test]
        //public async Task UpdateUser_ShouldUpdateUser_WhenDataIsValid()
        //{
        //    // Arrange
        //    var userId = "123";
        //    var user = new User
        //    {
        //        Id = userId,
        //        UserName = "test@example.com",
        //        Name = "Test User",
        //        Email = "test@example.com",
        //        CreatedAt = DateTime.UtcNow,
        //        IsDeleted = false,
        //        Forms = new List<Form>(),
        //        PasswordHash = "123"
        //    };

        //    var updateUserDTO = new UpdateUserDTO
        //    {
        //        Name = "new User Name",
        //        Password = "P@ssword123"
        //    };


        //    // Mock the user retrieval from the context
        //    _context.Users.FindAsync(userId).Returns(user);

        //    // Mock the update operation
        //    _userManager.UpdateAsync(Arg.Any<User>()).Returns(IdentityResult.Success);
        //    _loggedUserServiceMock.GetLoggedInUserId().Returns(userId);
            


        //    // Act
        //    var result = await _userService.UpdateUser(userId, updateUserDTO);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(updateUserDTO.Name, result.Name);
        //    Assert.AreEqual(userId, result.Id);
        //}

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


        [TearDown]
        public void TearDown()
        {

            // TODO: Verify how can dispose a IDbContext 
            //_context.Dispose();
            _userManager.Dispose();
        }
    }
}
