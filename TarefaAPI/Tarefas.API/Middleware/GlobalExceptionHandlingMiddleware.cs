
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
                _logger.LogError(ex.Message, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            ProblemDetails respostaExcecao = new ProblemDetails();

            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    respostaExcecao.Status = (int)HttpStatusCode.BadRequest;
                    respostaExcecao.Type = "Erro Interno";
                    respostaExcecao.Title = "Erro no Servidor";
                    respostaExcecao.Detail = ex.Message;
                    break;
                case UnauthorizedAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    respostaExcecao.Status = (int)HttpStatusCode.Unauthorized;
                    respostaExcecao.Type = "Erro de Acesso";
                    respostaExcecao.Title = "Acesso Negado";
                    respostaExcecao.Detail = "Algo deu errado, tente novamente mais tarde!";
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    respostaExcecao.Status = (int)HttpStatusCode.InternalServerError;
                    respostaExcecao.Type = "Erro Interno";
                    respostaExcecao.Title = "Erro no Servidor";
                    respostaExcecao.Detail = "Algo deu errado, tente novamente mais tarde!";
                    break;

            }
            var exResult = JsonSerializer.Serialize(respostaExcecao);
            await context.Response.WriteAsync(exResult);
        }
    }
}
