using Cp.ToDoList.Domain.Entities;
using Cp.ToDoList.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cp.ToDoList.Tests.Repositories
{
    [TestClass]
    public class ToDoItemRepositoryTests
    {

        [TestMethod]

        public async Task Assert_GetToDoItemsAsync_Returns_All_ToDoItems()
        {
            //Arrange

            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem
            {
                Id = new System.Guid(),
                Description = "ToDoItem1",
                IsCompleted = false

            },
                new ToDoItem
            {
                Id = new System.Guid(),
                Description = "ToDoItem2",
                IsCompleted = false
                }
            }.AsQueryable();

            var mockToDoItems = new Mock<DbSet<ToDoItem>>();

            mockToDoItems.As<IAsyncEnumerable<ToDoItem>>()
    .Setup(m => m.GetAsyncEnumerator(new System.Threading.CancellationToken()))
    .Returns(new TestAsyncEnumerator<ToDoItem>(toDoItems.GetEnumerator()));

            mockToDoItems.As<IQueryable<ToDoItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<ToDoItem>(toDoItems.Provider));
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Expression).Returns(toDoItems.Expression);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.ElementType).Returns(toDoItems.ElementType);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.GetEnumerator()).Returns(toDoItems.GetEnumerator());

            var mockToDoContext = new Mock<ToDoContext>();
            mockToDoContext.Setup(m => m.ToDoItems).Returns(mockToDoItems.Object);

            var toDoItemRepository = new ToDoItemRepository(mockToDoContext.Object);

            //Act

            var toDoItemsResult = await toDoItemRepository.GetToDoItemsAsync();

            //Assert

            Assert.AreEqual(toDoItemsResult.Count, 2);
            Assert.AreEqual(toDoItemsResult[0].Description, "ToDoItem1");
            Assert.AreEqual(toDoItemsResult[1].Description, "ToDoItem2");
        }

        [TestMethod]

        public void Assert_ToDoItemIdExists_True()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var toDoItems = new List<ToDoItem>
            {
              new ToDoItem
                {
                    Id = toDoItemId,
                    Description = "ToDoItem2",
                    IsCompleted = false
                    }
                }.AsQueryable();

            var mockToDoItems = new Mock<DbSet<ToDoItem>>();

            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Provider).Returns(toDoItems.Provider);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Expression).Returns(toDoItems.Expression);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.ElementType).Returns(toDoItems.ElementType);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.GetEnumerator()).Returns(toDoItems.GetEnumerator());

            var mockToDoContext = new Mock<ToDoContext>();
            mockToDoContext.Setup(m => m.ToDoItems).Returns(mockToDoItems.Object);

            var toDoItemRepository = new ToDoItemRepository(mockToDoContext.Object);

            //Act

            var toDoItemExists = toDoItemRepository.ToDoItemIdExists(toDoItemId);

            //Assert
                
            Assert.AreEqual(toDoItemExists, true);

        }

        [TestMethod]

        public void Assert_ToDoItemDescriptionExists_True()
        {
            //Arrange

            Guid toDoItemId = Guid.NewGuid();

            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem
            {
                Id = new System.Guid(),
                Description = "ToDoItem99",
                IsCompleted = false

            }
            }.AsQueryable();

            var mockToDoItems = new Mock<DbSet<ToDoItem>>();

            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Provider).Returns(toDoItems.Provider);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Expression).Returns(toDoItems.Expression);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.ElementType).Returns(toDoItems.ElementType);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.GetEnumerator()).Returns(toDoItems.GetEnumerator());

            var mockToDoContext = new Mock<ToDoContext>();
            mockToDoContext.Setup(m => m.ToDoItems).Returns(mockToDoItems.Object);

            var toDoItemRepository = new ToDoItemRepository(mockToDoContext.Object);

            //Act

            var toDoItemDescriptionExists = toDoItemRepository.ToDoItemDescriptionExists("ToDoItem99");

            //Assert

            Assert.AreEqual(toDoItemDescriptionExists, true);

        }

        [TestMethod]

        public async Task Assert_GetToDoItemAsync_Returns_Correct_ToDoItem()
        {
            //Arrange

            Guid toDoItemId1 = Guid.NewGuid();
            Guid toDoItemId2 = Guid.NewGuid();

            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem
            {
                Id = toDoItemId1,
                Description = "ToDoItem1",
                IsCompleted = false

            },
                new ToDoItem
            {
                Id = toDoItemId2,
                Description = "ToDoItem2",
                IsCompleted = false
                }
            }.AsQueryable();

            var mockToDoItems = new Mock<DbSet<ToDoItem>>();

            mockToDoItems.As<IAsyncEnumerable<ToDoItem>>()
    .Setup(m => m.GetAsyncEnumerator(new System.Threading.CancellationToken()))
    .Returns(new TestAsyncEnumerator<ToDoItem>(toDoItems.GetEnumerator()));

            mockToDoItems.As<IQueryable<ToDoItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<ToDoItem>(toDoItems.Provider));
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.Expression).Returns(toDoItems.Expression);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.ElementType).Returns(toDoItems.ElementType);
            mockToDoItems.As<IQueryable<ToDoItem>>().Setup(m => m.GetEnumerator()).Returns(toDoItems.GetEnumerator());

            var mockToDoContext = new Mock<ToDoContext>();
            mockToDoContext.Setup(m => m.ToDoItems).Returns(mockToDoItems.Object);
            mockToDoContext.Setup(m => m.ToDoItems.FindAsync(It.Is<Guid>(t => t == toDoItemId1)))
                .Returns(new ValueTask<ToDoItem>(new ToDoItem
                {
                    Id = toDoItemId1,
                    Description = "ToDoItem2",
                    IsCompleted = false
                }));

            var toDoItemRepository = new ToDoItemRepository(mockToDoContext.Object);

            //Act

            var toDoItemResult = await toDoItemRepository.GetToDoItemAsync(toDoItemId1);

            //Assert

            Assert.AreEqual(toDoItemResult.Id, toDoItemId1);
            Assert.AreEqual(toDoItemResult.Description, "ToDoItem2");
            Assert.AreEqual(toDoItemResult.IsCompleted, false);
        }

    }
}
