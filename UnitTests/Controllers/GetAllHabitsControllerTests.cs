using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XTracker.Controllers;
using XTracker.DTOs.HabitDTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Tests.Controllers
{
    public class GetAllHabitsControllerTests
    {
        private readonly Mock<IHabitService> _mockHabitService;
        private readonly HabitController _habitController;

        public GetAllHabitsControllerTests()
        {
            _mockHabitService = new Mock<IHabitService>();
            _habitController = new HabitController(_mockHabitService.Object);
        }

        [Fact]
        public async Task GetAllHabits_Should_Return_OkObjectResult_With_List_Of_HabitDTOs()
        {
            // Arrange
            var userId = "user-id";
            var habitDTOs = new List<HabitDTO>
            {
                new HabitDTO { Id = Guid.NewGuid(), Title = "Exercise", CreatedAt = DateTime.Now, UserId = userId },
                new HabitDTO { Id = Guid.NewGuid(), Title = "Read", CreatedAt = DateTime.Now, UserId = userId }
            };

            _mockHabitService.Setup(service => service.GetAllHabits(userId))
                             .ReturnsAsync(habitDTOs);

            // Act
            var result = await _habitController.GetAllHabits(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HabitDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllHabits_Should_Return_InternalServerError_On_Exception()
        {
            // Arrange
            var userId = "user-id";

            _mockHabitService.Setup(service => service.GetAllHabits(userId))
                             .ThrowsAsync(new System.Exception("Test Exception"));

            // Act
            var result = await _habitController.GetAllHabits(userId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno do servidor: Test Exception", objectResult.Value);
        }
    }
}
