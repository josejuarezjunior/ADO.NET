using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosProcedureController : ControllerBase
    {
        private IUsuarioRepository _repository;
        public UsuariosProcedureController()
        {
            _repository = new UsuarioProcedureRepository();
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
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Usuario usuario)
        {
            try
            {
                _repository.Insert(usuario);
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut]
        public IActionResult Update([FromBody] Usuario usuario)
        {
            try
            {
                _repository.Update(usuario);
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}
