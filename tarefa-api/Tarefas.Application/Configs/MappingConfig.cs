using AutoMapper;
using Tarefas.Application.DTO;
using Tarefas.Domain.Models;

namespace Tarefas.Application.Configs
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Tarefa, TarefaDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
