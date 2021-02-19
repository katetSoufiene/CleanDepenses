using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
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
    public class UserControllerTests
    {

        IApplicationDbContext context;
        Mock<IMapper> mapper;
        Mock<IMediator> mediator;

        DbContextOptions<ApplicationDbContext> options;
        public UserControllerTests()
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
        public async void Task_GetUserById_Return_OkResult()
        {
            //Arrange

            var item = new User() { Id = 3, FirstName = "lina", LastName = "sofi" };

            mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new UsersController(mediator.Object);

            //Act
            var data = await controller.GetById(3);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetUserById_Return_null()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), CancellationToken.None)).ReturnsAsync((User)null);
            var controller = new UsersController(mediator.Object);
            var UserId = 3;

            //Act
            var data = await controller.GetById(UserId);

            var item = data as OkObjectResult;

            //Assert
            Assert.Null(item.Value);
        }


        [Fact]
        public async void Task_GetUserById_MatchResult()
        {
            //Arrange
            var item = new User() { Id = 3, FirstName = "lina", LastName = "sofi" };
            mediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), CancellationToken.None)).ReturnsAsync(item);
            var controller = new UsersController(mediator.Object);
            var UserId = 3;

            //Act
            var data = await controller.GetById(UserId);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var itemUser = (User)okResult.Value;

            Assert.Equal("lina", itemUser.FirstName);
            Assert.Equal("sofi", itemUser.LastName);
        }

        #endregion

        #region Get All

        [Fact]
        public async void Task_GetUsers_Return_OkResult()
        {
            //Arrange
            var list = new List<User>()
            {             new User() { Id = 3, FirstName = "lina", LastName = "sofi" },
              new User() { Id = 2, FirstName = "lina2", LastName = "sofi2" }
             };

            mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new UsersController(mediator.Object);
            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetUsers_Return_null()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), CancellationToken.None)).ReturnsAsync((List<User>)null);
            var controller = new UsersController(mediator.Object);

            //Act
            var data = await controller.GetAll();

            var items = data as OkObjectResult;

            //Assert
            Assert.Null(items.Value);
        }

        [Fact]
        public async void Task_GetUsers_MatchResult()
        {
            //Arrange

            var list = new List<User>()
            {
             new User() { Id = 3, FirstName = "lina", LastName = "sofi" },
             new User() { Id = 2, FirstName = "lina2", LastName = "sofi2" }
            };


            mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), CancellationToken.None)).ReturnsAsync(list);
            var controller = new UsersController(mediator.Object);

            //Act
            var data = await controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;
            var Users = okResult.Value as List<User>;

            Assert.Equal("lina", Users[0].FirstName);
            Assert.Equal("sofi", Users[0].LastName);

            Assert.Equal("lina2", Users[1].FirstName);
            Assert.Equal("sofi2", Users[1].LastName);
        }

        #endregion

        #region Add New User

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange

            mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None)).ReturnsAsync(1);
            var controller = new UsersController(mediator.Object);

            var command = new CreateUserCommand() { FirstName = "aaaa", LastName = "bbbbb" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_0()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new UsersController(mediator.Object);

            var command = new CreateUserCommand() { FirstName = "", LastName = "bbbbb" };

            //Act
            var data = await controller.Create(command);

            var item = data as OkObjectResult;

            //Assert
            Assert.Equal(0, item.Value);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new UsersController(mediator.Object);

            var command = new CreateUserCommand() { FirstName = "aaaa", LastName = "bbbbb" };

            //Act
            var data = await controller.Create(command);

            //Assert
            Assert.IsType<OkObjectResult>(data);

            var okResult = data as OkObjectResult;


            Assert.Equal(1, okResult.Value);
        }

        #endregion

        #region Update Existing User

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new UsersController(mediator.Object);

            var command = new UpdateUserCommand() { Id = 1, FirstName = "aaaa", LastName = "bbbbb" };

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
            mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new UsersController(mediator.Object);

            var command = new UpdateUserCommand() { Id = 2, FirstName = "aaaa", LastName = "bbbbb" };

            var deivseId = 1;
            //Act
            var result = await controller.Update(deivseId, command);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_0()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new UsersController(mediator.Object);

            var command = new UpdateUserCommand() { Id = 1, FirstName = "aaaa", LastName = "bbbbb" };

            var deivseId = 1;
            //Act
            var result = await controller.Update(deivseId, command);

            var item = result as OkObjectResult;

            //Assert
            Assert.Equal(0, item.Value);
        }

        #endregion

        #region Delete User

        [Fact]
        public async void Task_Delete_User_Return_OkResult()
        {
            //Arrange

            var command = new DeleteUserByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteUserByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(1));
            var controller = new UsersController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Delete_Depense_Return_NotFoundResult()
        {
            //Arrange
            var command = new DeleteUserByIdCommand() { Id = 1 };
            mediator.Setup(m => m.Send(It.IsAny<DeleteUserByIdCommand>(), CancellationToken.None)).Returns(Task.FromResult(0));
            var controller = new UsersController(mediator.Object);

            //Act
            var data = await controller.Delete(1);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }



        #endregion

    }
}
