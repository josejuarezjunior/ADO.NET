using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    /*
         *  CRUD
         *  - GET       -> Obter a lista de usuários.
         *  - GET       -> Obter usuário passando o Id.
         *  - POST      -> Cadastrar um usuário.
         *  - PUT       -> Atualizar um cadastro.
         *  - DELETE    -> Remover um usuário.
         *  
         *  www.minhaapi.com.br
         *  www.minhaapi.com.br/api/controller
         *  www.minhaapi.com.br/api/Usuarios
         *  
         *  GET(lista), POST, PUT, DELETE irão utilizar o link abaixo, somente mudando o método:
         *  METHOD HTTP: www.minhaapi.com.br/api/Usuarios
         *  
         *  Para o GET(Obter por ID):
         *  www.minhaapi.com.br/api/Usuarios/id
         */

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioRepository _repository;
        public UsuariosController()
        {
            _repository = new UsuarioRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _repository.Get(id);
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Insert([FromBody]Usuario usuario)
        {
            _repository.Insert(usuario);
            return Ok(usuario);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Usuario usuario)
        {
            _repository.Update(usuario);
            return Ok(usuario);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}
