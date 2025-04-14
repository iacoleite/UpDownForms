using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.Models;
using UpDownForms.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UpDownForms.Services;



namespace UpDownForms.Tests
{
    [TestFixture]
    class QuestionServiceTests
    {
        private IQuestionService _questionService;
        private IUpDownFormsContext _context;
        private ILoggedUserService _loggedUserService;
        private QuestionOpenEnded fakeOpenEndedQuestion;


        //public QuestionServiceTests(IQuestionService questionService)
        //{
        //    _questionService = new QuestionService(_context, _loggedUserService);
        //}

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IUpDownFormsContext>();
            _loggedUserService = Substitute.For<ILoggedUserService>();
            _questionService = new QuestionService(_context, _loggedUserService);

            fakeOpenEndedQuestion = new QuestionOpenEnded
            {
                Id = 1,
                FormId = 1,
                Text = "this is a fake generic question",
                Order = 1,
                IsDeleted = true,
                IsRequired = false,
                Form = new Form(),
                Answers = new List<Answer>(),
            };
        }

        [Test]
        public async Task DeleteQuestion_QuestionExists_ShouldReturnQuestionDTOWithIsDeletedTrue()
        {
            var result = await _questionService.DeleteQuestion(fakeOpenEndedQuestion.Id);
            Assert.AreNotEqual(result.IsDeleted, fakeOpenEndedQuestion.IsDeleted);
        }

    }
}



