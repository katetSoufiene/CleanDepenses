using Application.Features.DeviseFeatures.Commands;
using Application.Features.DeviseFeatures.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests
{

    public class DeviseControllerTests
    {

        IApplicationDbContext context;
        Mock<IMapper> mapper;
        Mock<IMediator> mediator;

        DbContextOptions<ApplicationDbContext> options;
        public DeviseControllerTests()
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
        public async void Task_GetDeviseById_Return_OkResult()
        {
            //Arrange

            var item = new Devise() { Id = 3, Name = "xxxyyyzzz", Code = "XYZ" };

            mediator.Setup(m => m.Send(It.IsAny<GetDeviseByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new DevisesController(mediator.Object);

            //Act
            var data = await controller.GetById(3);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetDeviseById_Return_NotFoundResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetDeviseByIdQuery>(), CancellationToken.None)).ReturnsAsync((Devise)null);
            var controller = new DevisesController(mediator.Object);
            var deviseId = 3;

            //Act
            var data = await controller.GetById(deviseId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }


        [Fact]
        public async void Task_GetDeviseById_MatchResult()
        {
            //Arrange
            var item = new Devise() { Id = 3, Name = "xxxyyyzzz", Code = "XYZ" };
            mediator.Setup(m => m.Send(It.IsAny<GetDeviseByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new DevisesController(mediator.Object);
            var deviseId = 3;

            //Act
            var data = await controller.GetById(deviseId);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var itemDevise = (Devise)okResult.Value;

            Assert.Equal("xxxyyyzzz", itemDevise.Name);
            Assert.Equal("XYZ", itemDevise.Code);
        }

        #endregion

        #region Get All

        [Fact]
        public async void Task_GetDevises_Return_OkResult()
        {
            //Arrange
            var list = new List<Devise>()
            {
                new Devise() { Id = 3, Name = "xxxyyyzzz", Code = "XYZ" },
                new Devise() { Id = 2, Name = "xxxyyy", Code = "AEZ" }
            };

            mediator.Setup(m => m.Send(It.IsAny<GetAllDevisesQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new DevisesController(mediator.Object);
            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetDevises_Return_BadRequestResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetAllDevisesQuery>(), CancellationToken.None)).ReturnsAsync((List<Devise>)null);
            var controller = new DevisesController(mediator.Object);

            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetDevises_MatchResult()
        {
            //Arrange

            var list = new List<Devise>()
            {
                new Devise() { Id = 3, Name = "xxxyyyzzz", Code = "XYZ" },
                new Devise() { Id = 2, Name = "xxxyyy", Code = "AEZ" }
            };


            mediator.Setup(m => m.Send(It.IsAny<GetAllDevisesQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new DevisesController(mediator.Object);

            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var devises = okResult.Value as List<Devise>;

            Assert.Equal("XYZ", devises[0].Code);
            Assert.Equal("xxxyyyzzz", devises[0].Name);

            Assert.Equal("AEZ", devises[1].Code);
            Assert.Equal("xxxyyy", devises[1].Name);
        }

        #endregion

        #region Add New Devise

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<CreateDeviseCommand>(), CancellationToken.None)).ReturnsAsync(1);
            var controller = new DevisesController(mediator.Object);

            var command = new CreateDeviseCommand() { Code = "AZE", Name = "azerty" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateDeviseCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DevisesController(mediator.Object);

            var command = new CreateDeviseCommand() { Code = "AZE", Name = "azerty" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateDeviseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DevisesController(mediator.Object);

            var command = new CreateDeviseCommand() { Code = "AZE", Name = "azerty" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;


            Assert.Equal(1, okResult.Value);
        }

        #endregion

        #region Update Existing Devise

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDeviseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DevisesController(mediator.Object);

            var command = new UpdateDeviseCommand() { Id = 1, Code = "AZE", Name = "azerty" };

            var deivseId = 1;
            //Act
            var result = await controller.Update(deivseId, command);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDeviseCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DevisesController(mediator.Object);

            var command = new UpdateDeviseCommand() { Id = 2, Code = "AZE", Name = "azerty" };

            var deivseId = 1;
            //Act
            var result = await controller.Update(deivseId, command);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_NotFound()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateDeviseCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DevisesController(mediator.Object);

            var command = new UpdateDeviseCommand() { Id = 1, Code = "AZE", Name = "azerty" };

            var deivseId = 1;
            //Act
            var result = await controller.Update(deivseId, command);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Delete Devise

        [Fact]
        public async void Task_Delete_Devise_Return_OkResult()
        {
            //Arrange

            var command = new DeleteDeviseByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteDeviseByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new DevisesController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Delete_Depense_Return_NotFoundResult()
        {
            //Arrange
            var command = new DeleteDeviseByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteDeviseByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new DevisesController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        #endregion

    }
}
