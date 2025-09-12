using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TarefaAPI.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            var correlationId = Guid.NewGuid().ToString();

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Extensions = { ["correlationId"] = correlationId }
            };

            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    problemDetails.Title = "Erro de Validação";
                    problemDetails.Detail = ex.Message;
                    break;

                case UnauthorizedAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                    problemDetails.Title = "Acesso Negado";
                    problemDetails.Detail = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    problemDetails.Title = "Erro Interno do Servidor";
                    problemDetails.Detail = "Ocorreu um erro inesperado. Use o correlationId para rastreamento.";
                    break;
            }

            _logger.LogError(exception,
                "CorrelationId: {CorrelationId} - {Title} - Path: {Path}",
                correlationId,
                problemDetails.Title,
                context.Request.Path);

            var result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }
    }
}