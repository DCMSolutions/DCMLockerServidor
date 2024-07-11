using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockerController : ControllerBase
    {
        private readonly ILockerRepositorio _locker;
        private readonly ITokenRepositorio _token;

        public LockerController(ILockerRepositorio locker, ITokenRepositorio token)
        {
            _locker = locker;
            _token = token;
        }

        //CRUD lockers
        [HttpGet]
        public async Task<IActionResult> GetLockers()
        {
            try
            {
                var response = await _locker.GetLockers();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetLockerById(int Id)
        {
            try
            {
                var response = await _locker.GetLockerById(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{NroSerie}")]
        public async Task<IActionResult> GetLockerByNroSerie([FromBody] string NroSerie)
        {
            try
            {
                var response = await _locker.GetLockerByNroSerie(NroSerie);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byTokenEmpresa/{tokenEmpresa}")]
        public async Task<IActionResult> GetLockersByTokenEmpresa(string tokenEmpresa)
        {
            try
            {
                var response = await _locker.GetLockersByTokenEmpresa(tokenEmpresa);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addLocker")]
        public async Task<IActionResult> AddLocker(Locker Locker)
        {
            try
            {
                var response = await _locker.AddLocker(Locker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditLocker([FromBody] Locker locker)
        {
            try
            {
                var response = await _locker.EditLocker(locker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idLocker:int}")]
        public async Task<IActionResult> DeleteLocker(int idLocker)
        {
            try
            {
                var response = await _locker.DeleteLocker(idLocker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //CRUD boxes
        [HttpGet("box/{idBox:int}")]
        public async Task<IActionResult> GetBoxById(int idBox)
        {
            try
            {
                var response = await _locker.GetBoxById(idBox);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //comunication de servers
        [HttpPost]
        public async Task<ServerToken> Post(ServerToken serverCommunication)
        {
            try
            {

                var token = await _token.GetTokenByTokenLocker(serverCommunication.Token,serverCommunication.NroSerie);
            
                if (token.Modo=="" || token == null || token.Confirmado != true || serverCommunication.Box != null) return serverCommunication;
                if (token.IdBox == null)
                {
                    serverCommunication.Box = await _token.AsignarTokenABox(token.Id);
                }
                else
                {
                    serverCommunication.Box = token.IdBoxNavigation.IdFisico;
                    token.Contador++;
                    _token.EditToken(token);
                }
                return serverCommunication;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ConsoleColor.Red);
                throw new Exception("Hubo un error en la comunicacion de servidores");
            }
        }

        [HttpPost("status")]
        public async Task<ActionResult> PostConfig(ServerStatus status)
        {
            try
            {

                Locker locker = await _locker.GetLockerByNroSerie(status.NroSerie);
                if (locker != null)
                {
                    locker.Boxes = await _locker.SaveBoxes(status);
                    locker.LastUpdateTime = DateTime.Now;
                    await _locker.EditLocker(locker);
                }
                else
                {
                    locker = new Locker();
                    locker.Status = "connected";
                    locker.NroSerieLocker = status.NroSerie;
                    locker.Boxes = await _locker.SaveBoxes(status);
                    locker.LastUpdateTime = DateTime.Now;
                    locker.Empresa = status.Empresa;
                    await _locker.AddLocker(locker);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok();
        }

    }
}
