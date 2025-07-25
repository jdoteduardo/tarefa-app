using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Models;

namespace Tarefas.Infra.Context
{
    public class TarefaDbContext : DbContext
    {
        public TarefaDbContext(DbContextOptions<TarefaDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tarefa
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Titulo)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(e => e.Descricao)
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.DataLimite)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.Concluida)
                    .HasColumnType("tinyint(1)")
                    .IsRequired();
            });

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Login)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(e => e.Senha)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                // Seed do usuário administrador
                entity.HasData(new Usuario
                {
                    Id = 1,
                    Login = "admin",
                    Senha = BCrypt.Net.BCrypt.HashPassword("mv")
                });
            });
        }
    }
}
