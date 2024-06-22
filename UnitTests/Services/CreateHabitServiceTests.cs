using Moq;
using Xunit;
using XTracker.Services;
using XTracker.Repository.Interfaces;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;
using AutoMapper;
using System.Threading.Tasks;

namespace XTracker.Tests.Services
{
    public class CreateHabitServiceTests
    {
        private readonly Mock<IUnityOfWork> _mockUof;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HabitService _habitService;

        public CreateHabitServiceTests()
        {
            _mockUof = new Mock<IUnityOfWork>();
            _mockMapper = new Mock<IMapper>();
            _habitService = new HabitService(_mockUof.Object, _mockMapper.Object);
        }

        // Define um método de teste para verificar se o método Create do repositório de hábitos é chamado
        [Fact]
        public async Task Create_Should_Call_Create_On_HabitRepository()
        {
            // Arrange: configura os dados e o comportamento esperado do mock
            var habitDTO = new HabitDTO
            {
                Title = "Exercise",
                WeekDays = new List<int> { 1, 2, 3, 4, 5 },
                UserId = "user-id"
            };

            // Configura o mock do repositório de hábitos para retornar uma nova instância de Habit ao chamar o método Create
            _mockUof.Setup(uof => uof.HabitRepository.Create(It.IsAny<Habit>()))
                    .ReturnsAsync(new Habit());

            // Act: chama o método Create no serviço de hábitos
            await _habitService.Create(habitDTO);

            // Assert: verifica se o método Create do repositório de hábitos foi chamado exatamente uma vez
            _mockUof.Verify(uof => uof.HabitRepository.Create(It.IsAny<Habit>()), Times.Once);
        }
    }
}
