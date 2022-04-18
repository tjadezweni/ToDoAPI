using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Contracts.DataTransferObjects;
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
        public async Task ShouldCreateToDoTask(string title, string description)
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
        public async Task ShouldUpdateToDoTask(string title, string description)
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
        public async Task ShouldThrowExceptionWhenUpdating(string title, string description)
        {
            // Arrange


            // Act


            // Assert

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
