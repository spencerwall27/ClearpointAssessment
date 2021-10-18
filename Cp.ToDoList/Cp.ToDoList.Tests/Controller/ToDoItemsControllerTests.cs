using Cp.ToDoList.Domain.Entities;
using Cp.ToDoList.Domain.Repositories;
using Cp.ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Cp.ToDoList.Tests.Controller
{
    [TestClass]
    public class ToDoItemsControllerTests
    {
        [TestMethod]
        public async Task Assert_GetToDoItem_Returns_ToDoItem()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.GetToDoItemAsync(It.Is<Guid>(g => g == toDoItemId)))
                .Returns(Task.FromResult(new ToDoItem
                {
                    Id = toDoItemId,
                    Description = "TodItemTest1"
                }));

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.GetToDoItem(toDoItemId);

            var okObjectResult = controllerResult as OkObjectResult;

            var toDoItem = okObjectResult.Value as ToDoItem;

            //Assert

            Assert.AreEqual(toDoItem.Id, toDoItemId);
            Assert.AreEqual(toDoItem.Description, "TodItemTest1");

        }

        [TestMethod]
        public async Task Assert_PutToDoItem_Returns_NoContent()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var todoItem = new ToDoItem
            {
                Id = toDoItemId,
                Description = "UpdatedToDoItem"
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.UpdateToDoItemAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask);

            mockToDoItemRepository
                .Setup(r => r.ToDoItemIdExists(It.Is<Guid>(g => g == toDoItemId)))
                .Returns(true);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PutToDoItem(toDoItemId, todoItem);

            //Assert

            Assert.IsInstanceOfType(controllerResult, typeof(NoContentResult));

        }

        [TestMethod]
        public async Task Assert_PutToDoItem_Returns_BadRequest_Invalid_Id()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var todoItem = new ToDoItem
            {
                Id = Guid.NewGuid(),
                Description = "UpdatedToDoItem"
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.UpdateToDoItemAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask);

            mockToDoItemRepository
                .Setup(r => r.ToDoItemIdExists(It.Is<Guid>(g => g == toDoItemId)))
                .Returns(true);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PutToDoItem(toDoItemId, todoItem);

            //Assert

            Assert.IsInstanceOfType(controllerResult, typeof(BadRequestResult));

        }

        [TestMethod]
        public async Task Assert_PutToDoItem_Returns_NotFound()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var todoItem = new ToDoItem
            {
                Id = toDoItemId,
                Description = "UpdatedToDoItem"
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.UpdateToDoItemAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask);

            mockToDoItemRepository
                .Setup(r => r.ToDoItemIdExists(It.Is<Guid>(g => g == toDoItemId)))
                .Returns(false);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PutToDoItem(toDoItemId, todoItem);

            //Assert

            Assert.IsInstanceOfType(controllerResult, typeof(NotFoundResult));

        }

        [TestMethod]
        public async Task Assert_PostToDoItem_Returns_CreatedAtAction()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var toDoItemToCreate = new ToDoItem
            {
                Id = toDoItemId,
                Description = "CreatedToDoItem",
                IsCompleted = true
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.CreateToDoItemAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PostToDoItem(toDoItemToCreate);

            var createdAtActionResult = controllerResult as CreatedAtActionResult;

            var toDoItem = createdAtActionResult.Value as ToDoItem;

            //Assert

            Assert.AreEqual(createdAtActionResult.ActionName, "PostToDoItem");
            Assert.AreEqual(toDoItem.Id, toDoItemId);
            Assert.AreEqual(toDoItem.Description, "CreatedToDoItem");
        }

        [TestMethod]
        public async Task Assert_PostToDoItem_BadRequest_Empty_Description()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var toDoItemToCreate = new ToDoItem
            {
                Id = toDoItemId,
                IsCompleted = true
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.CreateToDoItemAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PostToDoItem(toDoItemToCreate);

            var badRequestObjectResult = controllerResult as BadRequestObjectResult;

            //Assert

            Assert.IsInstanceOfType(controllerResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(badRequestObjectResult.Value, "Description is required");
        }

        [TestMethod]
        public async Task Assert_PostToDoItem_BadRequest_Description_Already_Exists()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var toDoItemToCreate = new ToDoItem
            {
                Id = toDoItemId,
                Description = "AlreadyExists",
                IsCompleted = true
            };

            var mockToDoItemRepository = new Mock<IToDoItemRepository>();

            var mockLogger = new Mock<ILogger<ToDoItemsController>>();

            mockToDoItemRepository
                .Setup(r => r.ToDoItemDescriptionExists(It.IsAny<string>()))
                .Returns(true);

            var toDoItemController = new ToDoItemsController(mockToDoItemRepository.Object,
                mockLogger.Object);

            //Act

            var controllerResult = await toDoItemController.PostToDoItem(toDoItemToCreate);

            var badRequestObjectResult = controllerResult as BadRequestObjectResult;

            //Assert

            Assert.IsInstanceOfType(controllerResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(badRequestObjectResult.Value, "Description already exists");
        }
    }
}
