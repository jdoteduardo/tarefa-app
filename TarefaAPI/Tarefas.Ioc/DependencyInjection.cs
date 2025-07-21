using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tarefas.Application.Configs;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;
using Tarefas.Application.Services;
using Tarefas.Application.Validator;
using Tarefas.Domain.Interfaces;
using Tarefas.Infra.Context;
using Tarefas.Infra.Repositories;

namespace Tarefas.Ioc
{
    public static class DependencyInjection
    {
        public static void ApplyMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TarefaDbContext>();

            var pendingMigrations = db.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                db.Database.Migrate();
            }
        }


        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TarefaDbContext>(options =>
                            options.UseMySql(
                                connectionString,
                                ServerVersion.AutoDetect(connectionString),
                                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorNumbersToAdd: null
                                )
                            ).EnableSensitiveDataLogging());

            services.AddScoped<ITarefaRepository, TarefaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }

        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<ITarefaService, TarefaService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IValidator<TarefaDTO>, TarefaValidator>();

            return services;
        }
    }
}
