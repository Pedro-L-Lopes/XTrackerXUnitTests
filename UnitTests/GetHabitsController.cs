using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTracker.Controllers;
using XTracker.DTOs;
using XTracker.Services;
using XTracker.Services.Interfaces;

namespace XTrackerXUnitTests.UnitTests;
public class GetHabitsController
{
    // GetAllhabits
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

    // GetHabitsForDay
    [Fact]
    public async Task GetHabitsForDay_Returns_HabitsForGivenDay()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var expectedDate = "2024-03-25";
        var expectedPossibleHabits = new List<HabitDTO>
            {
                new() { Id = 1, Title = "Test Habit 1", WeekDays = [1, 2, 3] },
                new() { Id = 2, Title = "Test Habit 2", WeekDays = [1, 4, 5] }
            };
        var expectedCompletedHabits = new List<int?> { 1, 3 };

        mockService.Setup(service => service.GetHabitsForDay(expectedDate))
                   .ReturnsAsync((expectedPossibleHabits, expectedCompletedHabits));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetHabitsForDay(expectedDate) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var responseObject = result.Value;
        var responseTuple = responseObject as Tuple<List<HabitDTO>, List<int?>>;

        Assert.NotNull(responseTuple);

        var possibleHabits = responseTuple.Item1;
        var completedHabits = responseTuple.Item2;

        Assert.NotNull(possibleHabits);
        Assert.NotNull(completedHabits);
        Assert.Equal(expectedPossibleHabits.Count, possibleHabits.Count);
        Assert.Equal(expectedCompletedHabits.Count, completedHabits.Count);

    }

    [Fact]
    public async Task GetHabitsForDay_ServiceThrowsArgumentException_Returns_BadRequest()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var invalidDate = "2024-03-25";
        mockService.Setup(service => service.GetHabitsForDay(invalidDate))
                   .ThrowsAsync(new ArgumentException("Invalid date"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetHabitsForDay(invalidDate) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task GetHabitsForDay_ServiceThrowsException_Returns_InternalServerError()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var validDate = "2024-03-25";
        mockService.Setup(service => service.GetHabitsForDay(validDate))
                   .ThrowsAsync(new Exception("Service error"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetHabitsForDay(validDate) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
    }

    // GetSummary
    [Fact]
    public async Task GetSummary_Returns_SummaryList()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        var expectedSummary = new List<SummaryDTO>
            {
                new() { Id = 1, Date = DateTime.Parse("2024-03-25"), Completed = 2, Amount = 5 },
                new() { Id = 2, Date = DateTime.Parse("2024-03-26"), Completed = 3, Amount = 6 }
            };
        mockService.Setup(service => service.GetSummary()).ReturnsAsync(expectedSummary);

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetSummary() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var returnedSummary = result.Value as List<SummaryDTO>;
        Assert.NotNull(returnedSummary);
        Assert.Equal(expectedSummary.Count, returnedSummary.Count);
    }

    [Fact]
    public async Task GetSummary_Returns_InternalServerError_On_Exception()
    {
        // Arrange
        var mockService = new Mock<IHabitService>();
        mockService.Setup(service => service.GetSummary()).ThrowsAsync(new Exception("Internal Server Error"));

        var controller = new HabitController(mockService.Object);

        // Act
        var result = await controller.GetSummary() as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro interno do servidor: Internal Server Error", result.Value);
    }
}