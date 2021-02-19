using Application.Features.DeviseFeatures.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class DeviseCreateTests
    {

        IApplicationDbContext context;
        Mock<IMapper> mapper;

        DbContextOptions<ApplicationDbContext> options;
        public DeviseCreateTests()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: "DepensesDataBase")
                  .Options;
            context = new ApplicationDbContext(options);

            mapper = new Mock<IMapper>();
        }


        /// <summary>
        /// Unique devise code  .
        /// </summary>
        [Fact]
        public async Task When_Create_Duplicate_Devise_Code_throws_Exception()
        {
            //Arr

            context.Devises.Add(new Devise() { Id = 1, Code = "USD", Name = "US dollar" });

            await context.SaveChangesAsync();

            var validator = new CreateDeviseCommandValidator(context);

            CreateDeviseCommand command = new CreateDeviseCommand()
            {
                Code = "USD",
                Name = "test",
            };

            //act            


            var result = validator.Validate(command, options => options.IncludeRuleSets("CreateDevise"));

            //Ass

            result.Errors[0].ErrorMessage.Should().Be("The specified Code already exists.");

            result.IsValid.Should().BeFalse();


        }


        /// <summary>
        /// Unique devise Name  .
        /// </summary>
        [Fact]
        public async Task When_Create_Duplicate_Devise_Name_throws_Exception()
        {
            //Arr

            context.Devises.Add(new Devise() { Id = 2, Code = "USD", Name = "US dollar" });

            await context.SaveChangesAsync();

            var validator = new CreateDeviseCommandValidator(context);

            CreateDeviseCommand command = new CreateDeviseCommand()
            {
                Code = "USA",
                Name = "US dollar",
            };

            //act           

            var result = validator.Validate(command, options => options.IncludeRuleSets("CreateDevise"));
            //Ass

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("The specified Name already exists.");

            result.IsValid.Should().BeFalse();

        }

        /// <summary>
        /// "Code must be 3 characters.  .
        /// </summary>
        [Fact]
        public async Task When_Create_Short_Devise_Code_throws_Exception()
        {
            //Arr

            var validator = new CreateDeviseCommandValidator(context);

            CreateDeviseCommand command = new CreateDeviseCommand()
            {
                Code = "US",
                Name = "US dollar",
            };

            //Acc

            var result = validator.Validate(command, options => options.IncludeRuleSets("CreateDevise"));

            //Ass

            result.Errors[0].ErrorMessage.Should().Be("Code must be 3 characters.");

            result.IsValid.Should().BeFalse();

        }


        /// <summary>
        /// Create Valid Devise .
        /// </summary>
        [Fact]
        public async Task Create_Valid_Devise()
        {
            //Arr

            context.Devises.Add(new Devise() { Id = 3, Code = "USD", Name = "US dollar" });

            await context.SaveChangesAsync();

            var validator = new CreateDeviseCommandValidator(context);

            CreateDeviseCommand command = new CreateDeviseCommand()
            {
                Code = "ALD",
                Name = "AL Dinar",
            };

            //Act

            var result = validator.Validate(command, options => options.IncludeRuleSets("CreateDevise"));

            //Ass  

            result.IsValid.Should().BeTrue();
        }
    }
}
