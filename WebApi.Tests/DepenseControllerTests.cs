using Application.Features.DepenseFeatures.Commands;
using Application.Features.DepenseFeatures.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests
{
    public class DepenseControllerTests
    {

        IApplicationDbContext context;
        Mock<IMapper> mapper;
        Mock<IMediator> mediator;

        DbContextOptions<ApplicationDbContext> options;
        public DepenseControllerTests()
        {

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: "DepensesDataBase")
                  .Options;

            context = new ApplicationDbContext(options);
            mapper = new Mock<IMapper>();
            mediator = new Mock<IMediator>();
        }

        #region Get By Id

        [Fact]
        public async void Task_GetDepenseById_Return_OkResult()
        {
            //Arrange

            var item = new Depense() { Id = 3, UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, NatureDepense = NatureDepense.Hotel, Commentaire = "comment" };

            mediator.Setup(m => m.Send(It.IsAny<GetDepenseByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new DepensesController(mediator.Object);

            //Act
            var data = await controller.GetById(3);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetDepenseById_Return_NotFoundResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetDepenseByIdQuery>(), CancellationToken.None)).ReturnsAsync((Depense)null);
            var controller = new DepensesController(mediator.Object);
            var DepenseId = 3;

            //Act
            var data = await controller.GetById(DepenseId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }


        [Fact]
        public async void Task_GetDepenseById_MatchResult()
        {
            //Arrange
            var item = new Depense() { Id = 3, UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, NatureDepense = NatureDepense.Hotel, Commentaire = "comment" };
            mediator.Setup(m => m.Send(It.IsAny<GetDepenseByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new DepensesController(mediator.Object);
            var DepenseId = 3;

            //Act
            var data = await controller.GetById(DepenseId);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var itemDepense = (Depense)okResult.Value;

            Assert.Equal("comment", itemDepense.Commentaire);
        }

        #endregion

        #region Get All

        [Fact]
        public async void Task_GetDepenses_Return_OkResult()
        {
            //Arrange
            var list = new List<Depense>()
            {
                 new Depense() { Id = 1, UserId=1,Date= DateTime.Now,DeviseId=1,Montant=10,NatureDepense=NatureDepense.Hotel,Commentaire="comment" },
                 new Depense() { Id = 2, UserId = 2, Date = DateTime.Now, DeviseId = 1, Montant = 10, NatureDepense = NatureDepense.Hotel, Commentaire = "comment" }
           };

            mediator.Setup(m => m.Send(It.IsAny<GetAllDepensesQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new DepensesController(mediator.Object);
            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetDepenses_Return_null()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetAllDepensesQuery>(), CancellationToken.None)).ReturnsAsync((List<Depense>)null);
            var controller = new DepensesController(mediator.Object);

            //Act
            var data = await controller.GetAll();


            var items = data as OkObjectResult;

            //Assert
            Assert.Null(items.Value);
        }

        [Fact]
        public async void Task_GetDepenses_MatchResult()
        {
            //Arrange

            var list = new List<Depense>()
            {
                new Depense() { Id = 3, UserId=1,Date= DateTime.Now,DeviseId=1,Montant=10,NatureDepense=NatureDepense.Hotel,Commentaire="comment" },
            new Depense() { Id = 2, UserId = 2, Date = DateTime.Now, DeviseId = 1, Montant = 10, NatureDepense = NatureDepense.Hotel, Commentaire = "comment" }
        };


            mediator.Setup(m => m.Send(It.IsAny<GetAllDepensesQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new DepensesController(mediator.Object);

            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var Depenses = okResult.Value as List<Depense>;

            Assert.Equal(3, Depenses[0].Id);         

          
        }

        #endregion

        #region Add New Depense

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<CreateDepenseCommand>(), CancellationToken.None)).ReturnsAsync(1);
            var controller = new DepensesController(mediator.Object);

           

            var command = new CreateDepenseCommand() { UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateDepenseCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DepensesController(mediator.Object);

            var command = new CreateDepenseCommand() { UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateDepenseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DepensesController(mediator.Object);

            var command = new CreateDepenseCommand() { UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;


            Assert.Equal(1, okResult.Value);
        }

        #endregion

        #region Update Existing Depense

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDepenseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DepensesController(mediator.Object);

            var command = new UpdateDepenseCommand() {Id=1, UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            var depenseId = 1;
            //Act
            var result = await controller.Update(depenseId, command);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDepenseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DepensesController(mediator.Object);

            var command = new UpdateDepenseCommand() { Id = 2, UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            var depenseId = 1;
            //Act
            var result = await controller.Update(depenseId, command);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_NotFound()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDepenseCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DepensesController(mediator.Object);

            var command = new UpdateDepenseCommand() { Id = 2, UserId = 1, Date = DateTime.Now, DeviseId = 1, Montant = 10, Commentaire = "comment" };

            var depenseId = 2;
            //Act
            var result = await controller.Update(depenseId, command);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Delete Depense

        [Fact]
        public async void Task_Delete_Depense_Return_OkResult()
        {
            //Arrange

            var command = new DeleteDepenseByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteDepenseByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DepensesController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Delete_Depense_Return_NotFoundResult()
        {
            //Arrange
            var command = new DeleteDepenseByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteDepenseByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DepensesController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        #endregion

    }
}
