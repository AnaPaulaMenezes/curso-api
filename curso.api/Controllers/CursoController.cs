using curso.api.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/Curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        /// <summary>
        /// Cadastrar curso para um usuário autenticado
        /// </summary>
        /// <returns>Retorna o status 201 e dados do curso</returns>
        [HttpPost]
        [Route("")]
        [Authorize]
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar curso para o usuário")]
        [SwaggerResponse(statusCode: 401, description: "Acesso não autorizado!")]
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {
            //var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Created("", cursoViewModelInput);
        }


        /// <summary>
        /// Listar  cursos ativos para um usuário autenticado
        /// </summary>
        /// <returns>Retorna o status ok e dados do curso</returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter dados do curso")]
        [SwaggerResponse(statusCode: 401, description: "Acesso não autorizado!")]
        [Authorize]
        public async Task<IActionResult> Get( )
        {
            var cursos = new List<CursoViewModelOutput>();
            //var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            cursos.Add(new CursoViewModelOutput
            {
                Login = "1",
                Descricao = "teste",
                Nome = "teste"

            });
            return Ok(cursos);
        }
    }
}
