using Microsoft.AspNetCore.Mvc;
using Moq;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;
using TarefaAPI.Controllers;
using Xunit;

namespace Tarefas.Tests.Controllers
{
    public class TarefaControllerTests
    {
        private readonly Mock<ITarefaService> _mockTarefaService;
        private readonly TarefaController _controller;

        public TarefaControllerTests()
        {
            _mockTarefaService = new Mock<ITarefaService>();
            _controller = new TarefaController(_mockTarefaService.Object);
        }

        [Fact]
        public async Task ListarTarefas_DeveRetornarOkComListaDeTarefas()
        {
            // Arrange
            var tarefasEsperadas = new List<TarefaDTO>
            {
                new TarefaDTO { Id = Guid.NewGuid(), Titulo = "Tarefa 1", Descricao = "Descrição 1" },
                new TarefaDTO { Id = Guid.NewGuid(), Titulo = "Tarefa 2", Descricao = "Descrição 2" }
            };

            _mockTarefaService
                .Setup(service => service.ListarTarefas())
                .ReturnsAsync(tarefasEsperadas);

            // Act
            var resultado = await _controller.ListarTarefas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var tarefasRetornadas = Assert.IsAssignableFrom<IEnumerable<TarefaDTO>>(okResult.Value);
            Assert.Equal(tarefasEsperadas.Count, tarefasRetornadas.Count());
        }
    }
}