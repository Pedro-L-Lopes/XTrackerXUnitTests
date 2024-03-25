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
public class DeleteHabitController
{
    [Fact]
    public async Task Delete_Returns_Ok()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.Delete(It.IsAny<int>()))
                   .Returns(Task.CompletedTask);

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.Delete(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Hábito excluído com sucesso!", result.Value);
    }

    [Fact]
    public async Task Delete_ServiceThrowsException_Returns_InternalServerError()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.Delete(It.IsAny<int>()))
                   .ThrowsAsync(new Exception("Erro interno do servidor: Service error"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.Delete(1) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro interno do servidor: Service error", result.Value);
    }
}
