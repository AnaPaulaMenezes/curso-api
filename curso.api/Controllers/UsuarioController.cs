using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configuration;
using curso.api.Filters;
using curso.api.Infraestruture.Data;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace curso.api.Controllers
{
    [Route("api/v1/Usuario")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        
        private readonly IUsuarioRepository _usuarioRepository;
       
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(IUsuarioRepository usuarioRepository , IAuthenticationService authenticationService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;

        }

        /// <summary>
        /// Autenticar usuário cadastrado
        /// </summary>
        /// <param name="loginViewModelInput"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("logar")]
        [SwaggerResponse(statusCode:200, description:"Sucesso ao autenticar usuário", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode:400, description:"Informe os campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode:500, description:"Erro interno", Type = typeof(ErroGenericoViewModel))]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
            Usuario  usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);

            if (usuario == null)
            {
                return BadRequest("Houve um erro ao buscar os dados");
            }

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = usuario.Login,
                Email = usuario.Email
            };

            var token = _authenticationService.GerarToken(usuarioViewModelOutput);
            
            return Ok(new
            {
               Token =  token,
               Usuario = usuarioViewModelOutput
            });
        }

        /// <summary>
        /// Registrar usuário
        /// </summary>
        /// <param name="registrarViewModelInput"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("registrar")]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao registrar usuário", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Informe os campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistrarViewModelInput registrarViewModelInput)
        {
           
            //var migracoesPendentes = contexto.Database.GetPendingMigrations();
            //if (migracoesPendentes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}

            var usuario = new Usuario();
            usuario.Login = registrarViewModelInput.Login;
            usuario.Senha = registrarViewModelInput.Senha;
            usuario.Email = registrarViewModelInput.Email;

            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", registrarViewModelInput);
        }
    }
}
