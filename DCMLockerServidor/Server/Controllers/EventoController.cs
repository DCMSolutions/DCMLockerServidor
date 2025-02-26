using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        private readonly IEventoRepositorio _Evento;

        public EventoController(IEventoRepositorio Evento)
        {
            _Evento = Evento;
        }


        //CRUD
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            try
            {
                var response = await _Evento.GetEventos();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{IdLocker:int}")]
        public async Task<IActionResult> GetEventosByIdLocker(int IdLocker)
        {
            try
            {
                var response = await _Evento.GetEventosByIdLocker(IdLocker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEvento([FromBody] Evento Evento)
        {
            try
            {
                var response = await _Evento.AddEvento(Evento);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditEvento(Evento Evento)
        {
            try
            {
                var response = await _Evento.EditEvento(Evento);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{idEvento:int}")]
        public async Task<IActionResult> DeleteEvento(int idEvento)
        {
            try
            {
                var response = await _Evento.DeleteEvento(idEvento);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
