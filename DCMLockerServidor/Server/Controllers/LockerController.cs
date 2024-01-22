using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

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

        //El CRUD
        [HttpGet]
        public async Task<IActionResult> GetLockers()
        {

            var response = await _locker.GetLockers();
            return Ok(response);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetLockerById(int Id)
        {
            var response = await _locker.GetLockerById(Id);
            return Ok(response);
        }
        [HttpGet("{NroSerie}")]
        public IActionResult GetLockerByNroSerie([FromBody] string NroSerie)
        {
            return Ok(_locker.GetLockerByNroSerie(NroSerie));
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
        [HttpPut("editLocker")]
        public async Task<IActionResult> EditLocker([FromBody]Locker locker)
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
        
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteLocker(int Id)
        {
            try
            {
                Locker locker = await _locker.GetLockerById(Id);
                await _locker.DeleteLocker(locker);
                return Ok();
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
                var locker = await _locker.GetLockerByNroSerie(serverCommunication.NroSerie);

                var response = await _token.VerifyToken(serverCommunication,locker);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
                    //ver como pasar de List<TLockerMapDTO> a Box
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

        //tamaños
        [HttpGet("GetSizes")]
        public async Task<IActionResult> GetSizes()
        {
            var response = await _locker.GetSizes();
            return Ok(response);
        }
    }
}
