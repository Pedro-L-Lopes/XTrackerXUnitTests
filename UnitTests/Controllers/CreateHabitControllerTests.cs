using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using XTracker.Controllers;
using XTracker.DTOs.HabitDTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Tests.Controllers
{
    public class CreateHabitControllerTests
    {
        private readonly Mock<IHabitService> _mockHabitService;
        private readonly HabitController _habitController;

        public CreateHabitControllerTests()
        {
            _mockHabitService = new Mock<IHabitService>();
            _habitController = new HabitController(_mockHabitService.Object);
        }

        // Define um método de teste para verificar se o método CreateHabit retorna ObjectResult
        [Fact]
        public async Task CreateHabit_Should_Return_ObjectResult()
        {
            // Arrange: configura os dados e o comportamento esperado do mock
            var habitDTO = new HabitDTO
            {
                Title = "Exercise",
                WeekDays = new List<int> { 1, 2, 3, 4, 5 },
                UserId = "user-id"
            };

            // Configura o mock para retornar uma tarefa concluída quando o método Create for chamado
            _mockHabitService.Setup(service => service.Create(It.IsAny<HabitDTO>()))
                             .Returns(Task.CompletedTask);

            // Act: chama o método CreateHabit no controlador
            var result = await _habitController.CreateHabit(habitDTO);

            // Assert: verifica se o resultado é do tipo ObjectResult
            Assert.IsType<ObjectResult>(result);
        }

        // Define um método de teste para verificar se o método CreateHabit retorna BadRequest se o ModelState for inválido
        [Fact]
        public async Task CreateHabit_Should_Return_BadRequest_If_ModelState_Invalid()
        {
            // Arrange: configura os dados e adiciona um erro ao ModelState do controlador
            var habitDTO = new HabitDTO();
            _habitController.ModelState.AddModelError("Title", "Required");

            // Act: chama o método CreateHabit no controlador
            var result = await _habitController.CreateHabit(habitDTO);

            // Assert: verifica se o resultado é do tipo BadRequestObjectResult
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Define um método de teste para verificar se o método CreateHabit retorna InternalServerError em caso de exceção
        [Fact]
        public async Task CreateHabit_Should_Return_InternalServerError_On_Exception()
        {
            // Arrange: configura os dados e o comportamento esperado do mock
            var habitDTO = new HabitDTO
            {
                Title = "Exercise",
                WeekDays = new List<int> { 1, 2, 3, 4, 5 },
                UserId = "user-id"
            };

            // Configura o mock para lançar uma exceção quando o método Create for chamado
            _mockHabitService.Setup(service => service.Create(It.IsAny<HabitDTO>()))
                             .ThrowsAsync(new System.Exception("Test Exception"));

            // Act: chama o método CreateHabit no controlador
            var result = await _habitController.CreateHabit(habitDTO);

            // Assert: verifica se o resultado é do tipo ObjectResult e se o status code é 500 (InternalServerError)
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
