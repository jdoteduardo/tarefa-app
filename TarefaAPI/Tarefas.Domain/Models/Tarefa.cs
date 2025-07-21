namespace Tarefas.Domain.Models
{
    public class Tarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataLimite { get; set; }
        public bool Concluida { get; set; }
    }
}
