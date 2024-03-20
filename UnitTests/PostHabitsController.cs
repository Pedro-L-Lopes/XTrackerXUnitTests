using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using XTracker.Controllers;
using XTracker.DTOs;
using XTracker.Services.Interfaces;
using Xunit;

namespace XTrackerXUnitTests.UnitTests
{
    public class HabitControllerTests
    {
        [Fact]
        public async Task CreateHabit_Returns_Created()
        {
            // Arrange
            var mockService = new Mock<IHabitService>();
            mockService.Setup(service => service.Create(It.IsAny<HabitDTO>())).Returns(Task.CompletedTask);
            var controller = new HabitController(mockService.Object);

            // Act
            var habitDTO = new HabitDTO { Title = "Test Habit", WeekDays = [1, 2, 3] };
            var result = await controller.CreateHabit(habitDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task CreateHabit_InvalidModel_Returns_BadRequest()
        {
            // Arrange
            var mockService = new Mock<IHabitService>();
            var controller = new HabitController(mockService.Object);
            controller.ModelState.AddModelError("WeekDays", "Required");

            // Act
            var habitDTO = new HabitDTO { Title = "Test Habit", WeekDays = null };
            var result = await controller.CreateHabit(habitDTO) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateHabit_ServiceThrowsException_Returns_InternalServerError()
        {
            // Arrange
            var mockService = new Mock<IHabitService>();
            mockService.Setup(service => service.Create(It.IsAny<HabitDTO>())).ThrowsAsync(new Exception("Service error"));
            var controller = new HabitController(mockService.Object);

            // Act
            var habitDTO = new HabitDTO { Title = "Test Habit", WeekDays = [1, 2, 3] };
            var result = await controller.CreateHabit(habitDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Service error", result.Value);
        }
    }
}
