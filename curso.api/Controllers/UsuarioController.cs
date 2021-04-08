using curso.api.Business.Entities;
using curso.api.Filters;
using curso.api.Infraestruture.Data;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
           

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = 1,
                Login = "anateste",
                Email = "ana@teste.com"
            };

            //Gerar token JWT 
            var secret = Encoding.ASCII.GetBytes("174c77dbe874add49229cb7a4813038c");
            var symmetricSecutiryKey = new SymmetricSecurityKey(secret);
            var securityTokensDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
                    new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login),
                    new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email),

                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials (symmetricSecutiryKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokensDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);
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
            var optionsBuilder = new DbContextOptionsBuilder<CursoDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=CURSO;Integrated Security=True;Pooling=False");

            CursoDbContext contexto = new CursoDbContext(optionsBuilder.Options);

            var migracoesPendentes = contexto.Database.GetPendingMigrations();
            if (migracoesPendentes.Count() > 0)
            {
                contexto.Database.Migrate();
            }

            var usuario = new Usuario();
            usuario.Login = registrarViewModelInput.Login;
            usuario.Senha = registrarViewModelInput.Senha;
            usuario.Email = registrarViewModelInput.Email;

            contexto.Usuario.Add(usuario);
            contexto.SaveChanges();

            return Created("", registrarViewModelInput);
        }
    }
}
