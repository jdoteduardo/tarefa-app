using AutoMapper;
using Microsoft.Extensions.Configuration;
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

        public AuthService(IUsuarioRepository authRepository, IMapper mapper, IConfiguration config)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> Login(UsuarioDTO usuarioDTO)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            var usuarioExistente = await _authRepository.BuscarPorLoginESenha(usuario);

            if (usuarioExistente == null)
                throw new ApplicationException("Usuário ou senha inválidos.");

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

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
