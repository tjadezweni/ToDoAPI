using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Contracts.DataTransferObjects;
using ToDo.Contracts.Exceptions;
using ToDo.Infrastructure;
using ToDo.Infrastructure.Entities;
using ToDo.Services;
using Xunit;

namespace ToDo.Tests.Services
{
    public class ToDoTaskService_UnitTests
    {
        [Theory]
        [InlineData("Clean", "Clean the lounge")]
        [InlineData("Play", "Play playstation")]
        [InlineData("Sport", "Practice free-kicks")]
        public async Task CreateToDoTask_ShouldCreateToDoTask(string title, string description)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var createToDoTaskDto = new CreateToDoTaskDto { Title = title, Description = description };

            var mockToDoTaskDto = new ToDoTaskDto { Title = title, Description = description };

            var mockToDoTask = new ToDoTask { Title = title, Description = description };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<ToDoTask>(It.Is<CreateToDoTaskDto>(parameter =>
                parameter == createToDoTaskDto)))
                .Returns(mockToDoTask);
            mockMapper.Setup(mapper => mapper.Map<ToDoTaskDto>(It.Is<ToDoTask>(parameter =>
                parameter.Title == title && parameter.Description == description)))
                .Returns(mockToDoTaskDto);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var toDoTaskDto = await service.CreateToDoTaskAsync(createToDoTaskDto);

            // Assert
            Assert.NotNull(toDoTaskDto);
            Assert.Same(title, toDoTaskDto.Title);
            Assert.Same(description, toDoTaskDto.Description);
            Assert.False(toDoTaskDto.IsCompleted);
        }

        [Theory]
        [InlineData("Food", "Buy food for supper")]
        [InlineData("Cook", "Cook supper")]
        [InlineData("Sleep", "Sleep for 8 hours")]
        public async Task UpdateToDoTaskDetails_ShouldUpdateToDoTask(string title, string description)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var toDoTaskId = 1;
            var updateToDoTaskDto = new UpdateToDoTaskDto { Title = title, Description = description };
            var mockToDoTask = mockContext.ToDoTasks.Find(toDoTaskId);
            var updatedToDoTask = mockToDoTask;
            updatedToDoTask.Title = title;
            updatedToDoTask.Description = description;
            var mockToDoTaskDto = new ToDoTaskDto { Id = toDoTaskId, Title = title, Description = description };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map(updateToDoTaskDto, mockToDoTask))
                .Returns(updatedToDoTask);
            mockMapper.Setup(mapper => mapper.Map<ToDoTaskDto>(updatedToDoTask))
                .Returns(mockToDoTaskDto);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var toDoTaskDto = await service.UpdateToDoTaskDetailsAsync(toDoTaskId, updateToDoTaskDto);

            // Assert
            Assert.NotNull(toDoTaskDto);
            Assert.Equal(toDoTaskId, toDoTaskDto.Id);
            Assert.Same(title, toDoTaskDto.Title);
            Assert.Same(description, toDoTaskDto.Description);
        }

        [Theory]
        [InlineData("Food", "Buy food for supper")]
        [InlineData("Cook", "Cook supper")]
        [InlineData("Sleep", "Sleep for 8 hours")]
        public async Task UpdateToDoTaskDetails_ShouldThrowExceptionWhenNotFindingToDoTask(string title, string description)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var toDoTaskId = 10;
            var updateToDoTaskDto = new UpdateToDoTaskDto { Title = title, Description = description };
            var mockToDoTask = mockContext.ToDoTasks.Find(toDoTaskId);
            var updatedToDoTask = new ToDoTask();
            updatedToDoTask.Title = title;
            updatedToDoTask.Description = description;
            var mockToDoTaskDto = new ToDoTaskDto { Id = toDoTaskId, Title = title, Description = description };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map(updateToDoTaskDto, mockToDoTask))
                .Returns(updatedToDoTask);
            mockMapper.Setup(mapper => mapper.Map<ToDoTaskDto>(updatedToDoTask))
                .Returns(mockToDoTaskDto);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var action = () => service.UpdateToDoTaskDetailsAsync(toDoTaskId, updateToDoTaskDto);

            // Assert
            var exception = await Assert.ThrowsAsync<ToDoTaskNotFoundException>(action);
            Assert.Equal(new ToDoTaskNotFoundException(toDoTaskId).Message, exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteToDoTask_ShouldDeleteToDoTaskWithExistingId(int toDoTaskId)
        {
            // Arrange
            var mockContext = GetContextWithData();
            var mockMapper = new Mock<IMapper>();
            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            await service.DeleteToDoTaskAsync(toDoTaskId);

            // Assert
            var toDoTask = await mockContext.ToDoTasks.FindAsync(toDoTaskId);
            Assert.Null(toDoTask);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task DeleteToDoTask_ShouldNotDeleteToDoTaskWithExistingId(int toDoTaskId)
        {
            // Arrange
            var mockContext = GetContextWithData();
            var mockMapper = new Mock<IMapper>();
            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            await service.DeleteToDoTaskAsync(toDoTaskId);

            // Assert
            var toDoTask = await mockContext.ToDoTasks.FindAsync(toDoTaskId);
            Assert.Null(toDoTask);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetToDoTaskById_ShouldGetToDoTaskWithExistingId(int toDoTaskId)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var mockToDoTaskDto = new ToDoTaskDto { Id = toDoTaskId };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<ToDoTaskDto>(It.IsAny<ToDoTask>()))
                .Returns(mockToDoTaskDto);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var toDoTask = await service.GetToDoTaskByIdAsync(toDoTaskId);

            // Assert
            Assert.NotNull(toDoTask);
            Assert.Equal(toDoTaskId, toDoTask.Id);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetToDoTaskById_ShouldThrowExceptionWhenNotFindingToDoTask(int toDoTaskId)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var mockToDoTaskDto = new ToDoTaskDto { Id = toDoTaskId };
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<ToDoTaskDto>(It.IsAny<ToDoTask>()))
                .Returns(mockToDoTaskDto);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var action = () => service.GetToDoTaskByIdAsync(toDoTaskId);

            // Assert
            var exception = await Assert.ThrowsAsync<ToDoTaskNotFoundException>(action);
            Assert.Equal(new ToDoTaskNotFoundException(toDoTaskId).Message, exception.Message);
        }

        [Theory]
        [InlineData(1, 20, 1, 2, "Eat", "Sleep")]
        [InlineData(1, 30, 2, 3, "Woo", "Hello")]
        public async Task GetAllAsync_ShouldGetAllToDoTasks(int pageNumber, int pageCount, int toDoTaskId1, int toDoTaskId2, string title1, string title2)
        {
            // Arrange
            var mockContext = GetContextWithData();

            var mockToDoTaskDtoList = new List<ToDoTaskDto>()
            {
                new ToDoTaskDto { Id = toDoTaskId1, Title = title1 },
                new ToDoTaskDto { Id = toDoTaskId2, Title = title2 }
            };
            var mockToDoTaskDtoListLength = mockToDoTaskDtoList.Count;

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<ToDoTaskDto>>(It.IsAny<ToDoTask>()))
                .Returns(mockToDoTaskDtoList);

            var service = new ToDoTaskService(mockContext, mockMapper.Object);

            // Act
            var toDoTaskDtoList = await service.GetAllAsync(pageNumber, pageCount);

            // Assert
            Assert.NotNull(toDoTaskDtoList);
            Assert.Equal(mockToDoTaskDtoListLength, toDoTaskDtoList.Metadata.TotalCount);
        }
        private ApplicationDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            var cleanRoomTask = new ToDoTask { Id = 1, Title = "Clean", Description = "Clean Room" };
            var eatTask = new ToDoTask { Id = 2, Title = "Eat", Description = "Eat supper" };

            context.ToDoTasks.Add(cleanRoomTask);
            context.ToDoTasks.Add(eatTask);
            context.SaveChanges();

            return context;
        }
    }
}
