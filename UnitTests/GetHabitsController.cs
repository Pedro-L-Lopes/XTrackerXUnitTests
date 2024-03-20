using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTracker.Controllers;
using XTracker.DTOs;
using XTracker.Services.Interfaces;

namespace XTrackerXUnitTests.UnitTests;
public class GetHabitsController
{
    [Fact]
    public async Task GetAllHabits_Returns_HabitsList()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var habitsList = new List<HabitDTO>
        {
            new() { Id = 1, Title = "Habit 1", WeekDays = [1, 2, 3] },
            new() { Id = 2, Title = "Habit 2", WeekDays = [4, 5, 6] }
        };
        mockService.Setup(service => service.GetAllHabits()).ReturnsAsync(habitsList);

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetAllHabits() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var returnedHabits = result.Value as List<HabitDTO>;
        Assert.NotNull(returnedHabits);
        Assert.Equal(habitsList.Count, returnedHabits.Count);
    }

    [Fact]
    public async Task GetAllHabits_Returns_InternalServerError_On_Exception()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.GetAllHabits()).ThrowsAsync(new Exception("Internal Server Error"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetAllHabits() as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro interno do servidor: Internal Server Error", result.Value);
    }
}