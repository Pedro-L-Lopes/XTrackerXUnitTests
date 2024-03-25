using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTracker.Controllers;
using XTracker.Services.Interfaces;

namespace XTrackerXUnitTests.UnitTests;
public class PatchHabitsController
{
    [Fact]
    public async Task ToggleHabitForDay_Returns_Ok()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.ToggleHabitForDay(It.IsAny<int>(), It.IsAny<DateTime>()))
                   .Returns(Task.CompletedTask);

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.ToggleHabitForDay(1, "2024-03-25") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Hábito atualizado", result.Value);
    }

    [Fact]
    public async Task ToggleHabitForDay_InvalidDateFormat_Returns_BadRequest()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.ToggleHabitForDay(1, "25-03-2024") as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Formato de data inválido", result.Value);
    }


    [Fact]
    public async Task ToggleHabitForDay_ServiceThrowsException_Returns_InternalServerError()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.ToggleHabitForDay(It.IsAny<int>(), It.IsAny<DateTime>()))
                   .ThrowsAsync(new Exception("Service error"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.ToggleHabitForDay(1, "2024-03-25") as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro interno do servidor: Service error", result.Value);
    }
}

