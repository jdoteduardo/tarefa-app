using Tarefas.Application.DTO;

namespace Tarefas.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaDTO>> BuscarTarefas();
        Task<TarefaDTO?> BuscarPorId(Guid id);
        Task<TarefaDTO> CriarTarefa(TarefaDTO tarefaDTO);
        Task<TarefaDTO> AtualizarTarefa(TarefaDTO tarefaDTO);
        Task<bool> DeletarTarefa(Guid id);
    }
}
