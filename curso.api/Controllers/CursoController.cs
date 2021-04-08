using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/Curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoController(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

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
            Curso curso = new Curso();
            curso.Nome = cursoViewModelInput.Nome;
            curso.Descricao = cursoViewModelInput.Descricao;
            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            curso.CodigoUsuario = codigoUsuario;

            _cursoRepository.Adicionar(curso);
            _cursoRepository.Commit();


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
           var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            
           var cursos =  _cursoRepository.ObterPorUsuario(codigoUsuario)
                .Select(s => new CursoViewModelOutput() { 
                    Nome = s.Nome,
                    Descricao =s.Descricao,
                    Login = s.usuario.Login
                });

            return Ok(cursos);
        }
    }
}
