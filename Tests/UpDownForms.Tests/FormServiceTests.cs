using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpDownForms.Models;
using UpDownForms.Services;

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
            _formService = Substitute.For<FormService>();
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
    }
}
