using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarefas.Application.DTO;
using Tarefas.Application.Services;
using Tarefas.Domain.Interfaces;
using Tarefas.Domain.Models;

namespace Tarefas.Tests.Service
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _mockRepository = new Mock<ITarefaRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new TarefaService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task BuscarPorId_QuandoTarefaExiste_DeveRetornarTarefaDTO()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var tarefa = new Tarefa
            {
                Id = tarefaId,
                Titulo = "Teste",
                Descricao = "Descrição teste"
            };

            var tarefaDTO = new TarefaDTO
            {
                Id = tarefaId,
                Titulo = "Teste",
                Descricao = "Descrição teste"
            };

            _mockRepository
                .Setup(repo => repo.BuscarPorId(tarefaId))
                .ReturnsAsync(tarefa);

            _mockMapper
                .Setup(mapper => mapper.Map<TarefaDTO>(tarefa))
                .Returns(tarefaDTO);

            // Act
            var resultado = await _service.BuscarPorId(tarefaId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(tarefaId, resultado.Id);
            Assert.Equal("Teste", resultado.Titulo);

            _mockRepository.Verify(repo => repo.BuscarPorId(tarefaId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<TarefaDTO>(tarefa), Times.Once);
        }
    }
}
