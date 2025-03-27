using Microsoft.AspNetCore.Identity;
using NSubstitute;
using FluentAssertions;
using UpDownForms.Controllers;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.DTO.QuestionDTOs;
using UpDownForms.DTO.UserDTOs;
using UpDownForms.Models;
using UpDownForms.Security;
using UpDownForms.Services;
using NSubstitute.ReturnsExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace UpDownForms.Tests
{
    [TestFixture]
    public class FormController_Test
    {


        [SetUp]
        public void Setup()
        {


        }


        [Test]
        public async Task FormService_CreateForm_ReturnFormDTO()
        {

        }

        //[OneTimeTearDown]
        //[TearDownAttribute]
        //public void TearDown()
        //{

        //}
    }
}