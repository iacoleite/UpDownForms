using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpDownForms.Controllers;
using UpDownForms.DTO.FormDTOs;
using UpDownForms.Models;
using UpDownForms.Pagination;
using UpDownForms.Services;
using UpDownForms.Services.Interfaces;

namespace UpDownForms.Tests
{
    [TestFixture]
    class FormServiceTests
    {
        private FormService _formService;
        private UpDownFormsContext _context;


        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<UpDownFormsContext>(new DbContextOptions<UpDownFormsContext>());

            //_formService = new FormService();
        }

        [Test]
        public void GetForms_ShouldReturnPageableFormsDTO()
        {
            //setup

            var form = new Form
            {
                Id = 1,
                UserId = "",
                Title = "asd",
                Description = "asd",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsPublished = false,
                IsDeleted = false,
                User = new User()
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        //// THIS IS A COPILOT SUGGESTION OF A TEST USING THE Interface...
        //[Test]
        //public async Task GetForms_ShouldReturnForms_WhenServiceReturnsData()
        //{
        //    // Arrange
        //    var mockFormService = Substitute.For<IFormService>();
        //    var pageParameters = new PageParameters { PageNumber = 1, PageSize = 10 };
        //    var mockResponse = new Pageable<FormDTO>(new List<FormDTO>().AsQueryable(), 10, 1, 0);

        //    mockFormService.GetForms(pageParameters).Returns(Task.FromResult(mockResponse));

        //    var controller = new FormController(mockFormService);

        //    // Act
        //    var result = await controller.GetForms(pageParameters);

        //    // Assert
        //    result.Should().BeOfType<OkObjectResult>();
        //    var okResult = result as OkObjectResult;
        //    okResult.Value.Should().BeEquivalentTo(mockResponse);
        //}

    }


}
