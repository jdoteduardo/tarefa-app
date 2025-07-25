using Tarefas.Domain.Models;

namespace Tarefas.Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> ListarTarefas();
        Task<Tarefa?> BuscarPorId(Guid id);
        Task<Tarefa> CriarTarefa(Tarefa tarefa);
        Task<Tarefa> AtualizarTarefa(Tarefa tarefa);
        Task<bool> DeletarTarefa(Guid id);
    }
}
