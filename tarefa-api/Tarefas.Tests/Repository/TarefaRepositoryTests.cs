using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Models;
using Tarefas.Infra.Context;
using Tarefas.Infra.Repositories;
using Xunit;

namespace Tarefas.Tests.Repositories
{
    public class TarefaRepositoryTests
    {
        private readonly DbContextOptions<TarefaDbContext> _options;

        public TarefaRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TarefaDbContext>()
                .UseInMemoryDatabase(databaseName: "TarefasDbTest")
                .Options;
        }

        [Fact]
        public async Task CriarTarefa_DeveGerarNovoId()
        {
            // Arrange
            using var context = new TarefaDbContext(_options);
            var repository = new TarefaRepository(context);

            var tarefa1 = new Tarefa { Titulo = "Tarefa 1", Descricao = "Tarefa 1", DataLimite = DateTime.Now.AddDays(5), Concluida = true };
            var tarefa2 = new Tarefa { Titulo = "Tarefa 2", Descricao = "Tarefa 2", DataLimite = DateTime.Now.AddDays(5), Concluida = false };

            // Act
            var resultado1 = await repository.CriarTarefa(tarefa1);
            var resultado2 = await repository.CriarTarefa(tarefa2);

            // Assert
            Assert.NotEqual(resultado1.Id, resultado2.Id);
        }
    }
}