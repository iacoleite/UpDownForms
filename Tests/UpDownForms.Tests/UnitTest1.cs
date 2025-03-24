using Microsoft.AspNetCore.Identity;
using NSubstitute;
using FluentAssertions;
using UpDownForms.Controllers;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.ApiResponse;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services;
using NSubstitute.ReturnsExtensions;

namespace UpDownForms.Tests
{
    [TestFixture]
    public class FormController_Test
    {
        private FormController _formController;
        private FormService _formService;
        private UpDownFormsContext _context;
        private IPasswordHelper _passwordHelper;
        private UserManager<User> _userManager;
        private IUserService _userService;

        
        public FormController_Test()
        {
            _context = Substitute.For<UpDownFormsContext>();

        }

        [SetUp]
        public void Setup()
        {
            _passwordHelper = Substitute.For<IPasswordHelper>();
            _userManager = Substitute.For<UserManager<User>>();
            _userService = Substitute.For<IUserService>();
            _formService = Substitute.For<FormService>();
            _formController = new FormController(_formService);

        }

        [OneTimeTearDown]
        //[TearDownAttribute]
        public void TearDown()
        {
            _context.Dispose();
            _userManager.Dispose();
        }

        //[Test]
        //public async Task FormService_GetAllForms_ReturnListFormsAsync()
        //{
        //    var userDTO = new UserDetailsDTO
        //    {
        //        Id = "2189ughasbj08y2198g",
        //        Name = "nome",
        //        Email = "teste@teste.com",
        //        PasswordHash = "123",
        //        CreatedAt = DateTime.Now,
        //        IsDeleted = false
        //    };

        //    QuestionDTO questionDTO = new BaseQuestionDTO
        //    {
        //        Id = 1,
        //        FormId = 1,
        //        Text = "pergunta teste",
        //        Order = 1,
        //        Type = "OpenEnded",
        //        IsRequired = false,
        //        IsDeleted = false,
        //        Answers = new List<AnswerDTO>()
        //    };

        //    var formDTO = new FormDTO
        //    {
        //        Id = 1,
        //        UserId = "2189ughasbj08y2198g",
        //        Title = "asd",
        //        Description = "asd",
        //        CreatedAt = DateTime.Now,
        //        UpdatedAt = DateTime.Now,
        //        IsPublished = false,
        //        IsDeleted = false,
        //        User = userDTO,
        //        Questions = new List<QuestionDTO>()
        //    };
        //    var createFormDTO = new CreateFormDTO
        //    {


        //        Title = "asd",
        //        Description = "asd",

        //    };

        //    var expectedResponseData = new List<FormDTO>()
        //    {
        //        formDTO
        //    };

        //    var expectedResult = new ApiResponse<IEnumerable<FormDTO>>(true, "ok", expectedResponseData);


        //    await _formService.PostForm(createFormDTO);
        //    var result = await _formService.GetForms();

        //    Assert.AreEqual(expectedResult, result);

        //}
        [Test]
        public async Task FormService_CreateForm_ReturnFormDTO()
        {
            var createFormDTO = new CreateFormDTO
            {
                Title = "asd",
                Description = "asd",
            };
            //var userDTO = new UserDetailsDTO
            //{
            //    Id = "2189ughasbj08y2198g",
            //    Name = "nome",
            //    Email = "teste@teste.com",
            //    PasswordHash = "123",
            //    CreatedAt = DateTime.Now,
            //    IsDeleted = false
            //};
            //var expectedresult = new FormDTO            
            //{
            //    Id = 1,
            //    UserId = "2189ughasbj08y2198g",
            //    Title = "asd",
            //    Description = "asd",
            //    CreatedAt = DateTime.Now,
            //    UpdatedAt = DateTime.Now,
            //    IsPublished = false,
            //    IsDeleted = false,
            //    User = userDTO,
            //    Questions = new List<QuestionDTO>()
            //};



            var result = await _formService.PostForm(createFormDTO);



            result.Should().NotBeNull();
        }
    }
}