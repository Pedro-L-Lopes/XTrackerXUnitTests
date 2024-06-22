using Moq;
using Xunit;
using XTracker.Services;
using XTracker.Repository.Interfaces;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XTracker.Tests.Services
{
    public class GetAllServiceTests
    {
        private readonly Mock<IUnityOfWork> _mockUof;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HabitService _habitService;

        public GetAllServiceTests()
        {
            _mockUof = new Mock<IUnityOfWork>();
            _mockMapper = new Mock<IMapper>();
            _habitService = new HabitService(_mockUof.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllHabits_Should_Return_List_Of_HabitDTO()
        {
            // Arrange
            string userId = "test-user";
            var habitsFromRepo = new List<Habit>
            {
                new Habit
                {
                    Id = Guid.NewGuid(),
                    Title = "Drink Water",
                    CreatedAt = DateTime.Now.Date,
                    UserId = userId,
                    WeekDays = new List<HabitWeekDay>
                    {
                        new HabitWeekDay { WeekDay = 1 },
                        new HabitWeekDay { WeekDay = 2 }
                    }
                },
                new Habit
                {
                    Id = Guid.NewGuid(),
                    Title = "Exercise",
                    CreatedAt = DateTime.Now.Date,
                    UserId = userId,
                    WeekDays = new List<HabitWeekDay>
                    {
                        new HabitWeekDay { WeekDay = 3 },
                        new HabitWeekDay { WeekDay = 4 }
                    }
                }
            };

            _mockUof.Setup(uof => uof.HabitRepository.GetAllHabits(userId))
                    .ReturnsAsync(habitsFromRepo);

            _mockMapper.Setup(mapper => mapper.Map<HabitDTO>(It.IsAny<Habit>()))
                       .Returns((Habit source) => new HabitDTO
                       {
                           Id = source.Id,
                           Title = source.Title,
                           CreatedAt = source.CreatedAt,
                           UserId = source.UserId,
                           WeekDays = source.WeekDays.ConvertAll(hwd => hwd.WeekDay)
                       });

            // Act
            var result = await _habitService.GetAllHabits(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<HabitDTO>>(result);
            Assert.Equal(2, result.Count); // Assuming we have 2 habits in the mock data
        }
    }
}
