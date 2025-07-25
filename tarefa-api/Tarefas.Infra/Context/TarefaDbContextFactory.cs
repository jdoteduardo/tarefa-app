using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tarefas.Infra.Context
{
    public class TarefaDbContextFactory : IDesignTimeDbContextFactory<TarefaDbContext>
    {
        public TarefaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Tarefas.API/appsettings.Development.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TarefaDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new TarefaDbContext(optionsBuilder.Options);
        }
    }
}
