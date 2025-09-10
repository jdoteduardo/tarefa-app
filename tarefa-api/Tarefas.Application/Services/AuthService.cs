using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;
using Tarefas.Domain.Interfaces;
using Tarefas.Domain.Models;

namespace Tarefas.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUsuarioRepository authRepository, 
            IMapper mapper, 
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        public async Task<string> Login(UsuarioDTO usuarioDTO)
        {
            _logger.LogInformation("Iniciando processo de login para usuário: {Login}", usuarioDTO.Login);
            
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            _logger.LogDebug("Mapeamento de DTO para entidade realizado com sucesso");

            var usuarioExistente = await _authRepository.BuscarPorLoginESenha(usuario);
            
            if (usuarioExistente == null)
            {
                _logger.LogWarning("Tentativa de login mal sucedida para o usuário: {Login}", usuarioDTO.Login);
                throw new ApplicationException("Usuário ou senha inválidos.");
            }

            _logger.LogInformation("Usuário {Login} autenticado com sucesso. Gerando token...", usuarioExistente.Login);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioExistente.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarioExistente.Login)
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            _logger.LogInformation("Token JWT gerado com sucesso para o usuário: {Login}", usuarioExistente.Login);

            return token;
        }
    }
}
