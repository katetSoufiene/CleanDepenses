using Application.Features.DepenseFeatures.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class DepenseUpdateTests
    {

        IApplicationDbContext context;
        Mock<IMapper> mapper;

        DbContextOptions<ApplicationDbContext> options;
        public DepenseUpdateTests()
        {

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: "DepensesDataBase")
                  .Options;

            context = new ApplicationDbContext(options);


            mapper = new Mock<IMapper>();
        }

        /// <summary>
        ///  Une dépense ne peut pas avoir une date dans le futur,
        /// </summary>
        [Fact]
        public void When_Future_Date_Depense_Constructor_throws_ExceptionExeption()
        {
            //Arr

            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = DateTime.Now.AddDays(-1) };

            context.Depenses.Add(depense);
            context.Devises.Add(devise);
            context.Users.Add(user);

            context.SaveChangesAsync();

            var validator = new UpdateDepenseCommandValidator(context);

            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now.AddDays(2),
                NatureDepense = "Hotel",
                Montant = 10,
                DeviseId = 1,
                Commentaire = "Commentaire"
            };

            //Act

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Ass           

            result.Errors[0].ErrorMessage.Should().Be("Une dépense ne peut pas avoir une date dans le futur.");

            result.IsValid.Should().BeFalse();

        }

        /// <summary>
        /// Une dépense ne peut pas être datée de plus de 3 mois,
        /// </summary>
        [Fact]
        public void When_More_Then_3_Months_Date_Depense_Constructor_throws_ExceptionExeption()
        {
            //Arr

            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = DateTime.Now.AddDays(-1) };
            context.Depenses.Add(depense);
            context.Devises.Add(devise);
            context.Users.Add(user);
            context.SaveChangesAsync();
            var validator = new UpdateDepenseCommandValidator(context);

            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now.AddMonths(-4),
                NatureDepense = "Hotel",
                Montant = 10,
                DeviseId = 1,
                Commentaire = "Commentaire"
            };

            //Act 

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Ass

            result.Errors[0].ErrorMessage.Should().Be("Une dépense ne peut pas être datée de plus de 3 mois.");

            result.IsValid.Should().BeFalse();
        }


        /// <summary>
        /// "La devise de la dépense doit être identique à celle de l'utilisateur."
        /// </summary>
        [Fact]
        public void When_User_Devise_And_Depense_Devise_Are_Different_throws_ExceptionExeption()
        {
            //Arr

            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var devise2 = new Devise() { Id = 2, Code = "AAZ", Name = "AAEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = DateTime.Now.AddDays(-1) };
            context.Depenses.Add(depense);

            context.Devises.AddRange(devise, devise2);
            context.Users.Add(user);
            context.SaveChangesAsync();
            var validator = new UpdateDepenseCommandValidator(context);

            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now,
                NatureDepense = "Hotel",
                Montant = 10,
                DeviseId = 2,
                Commentaire = "Commentaire"
            };

            //Act 

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Ass

            result.Errors[0].ErrorMessage.Should().Be("La devise de la dépense doit être identique à celle de l'utilisateur.");

            result.IsValid.Should().BeFalse();
        }


        /// <summary>
        /// Le commentaire est obligatoire
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_Comment_IsNullOrEmpty_Date_Depense_Constructor_throws_ExceptionExeption(string commentaire)
        {
            //Arr           

            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = DateTime.Now.AddDays(-1) };
            context.Depenses.Add(depense);

            context.Devises.Add(devise);
            context.Users.Add(user);
            context.SaveChangesAsync();
            var validator = new UpdateDepenseCommandValidator(context);

            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now,
                NatureDepense = "Hotel",
                Montant = 10,
                DeviseId = 1,
                Commentaire = commentaire
            };

            //Act 

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Ass
            result.Errors[0].ErrorMessage.Should().Be("Le commentaire est obligatoire.");

        }



        /// <summary>
        /// "Un utilisateur ne peut pas déclarer deux fois la même dépense (même date et même montant)."
        /// </summary>
        [Fact]
        public async Task When_User_Create_Depense_Same_Date_Same_Montant_throws_ExceptionExeption()
        {
            //Arr
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = date };

            context.Devises.Add(devise);
            context.Users.Add(user);
            context.Depenses.Add(depense);

            await context.SaveChangesAsync();

            var validator = new UpdateDepenseCommandValidator(context);

            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = date,
                NatureDepense = "Hotel",
                Montant = 10,
                DeviseId = 1,
                Commentaire = "Commentaire"
            };

            //Act 

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Ass
            result.Errors[0].ErrorMessage.Should().Be("Un utilisateur ne peut pas déclarer deux fois la même dépense (même date et même montant).");


        }


        /// <summary>
        /// Update Valid depense
        /// </summary>
        [Fact]
        public async Task When_Valid_Properties__Return_Valid_Depense()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            var devise = new Devise() { Id = 1, Code = "AZE", Name = "AZEERteryty" };
            var user = new User() { Id = 1, FirstName = "aa", LastName = "bb", DeviseId = 1 };
            var depense = new Depense() { Id = 1, DeviseId = 1, UserId = 1, Montant = 10, Date = date };


            context.Devises.Add(devise);
            context.Users.Add(user);
            context.Depenses.Add(depense);

            await context.SaveChangesAsync();

            var validator = new UpdateDepenseCommandValidator(context);

            //Arr
            UpdateDepenseCommand command = new UpdateDepenseCommand()
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now.AddDays(-1),
                NatureDepense = "Hotel",
                Montant = 15,
                DeviseId = 1,
                Commentaire = "commentaire"
            };

            //Act 

            var result = validator.Validate(command, options => options.IncludeRuleSets("UpdateDepense"));

            //Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
