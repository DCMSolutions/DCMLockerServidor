using DCMLockerServidor.Server.Repositorio.Contrato;
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
    public class TokenController : ControllerBase
    {

        private readonly IEmpresaRepositorio _empresa;
        private readonly ILockerRepositorio _locker;
        private readonly ITokenRepositorio _token;

        public TokenController(IEmpresaRepositorio empresa, ILockerRepositorio locker, ITokenRepositorio token)
        {
            _empresa = empresa;
            _locker = locker;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetTokens()
        {
            try
            {
                var response = await _token.GetTokens();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetTokenById(int Id)
        {
            try
            {
                var response = await _token.GetTokenById(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byToken/{idLocker:int}/{token}")]
        public async Task<IActionResult> GetTokenByTokenLocker(int idLocker, string token)
        {
            try
            {
                var response = await _token.GetTokenByTokenLocker(token, idLocker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] Token Token)
        {
            try
            {
                var response = await _token.AddToken(Token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditToken(Token Token)
        {
            try
            {
                var response = await _token.EditToken(Token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idToken:int}")]
        public async Task<IActionResult> DeleteToken(int idToken)
        {
            try
            {
                var response = await _token.DeleteToken(idToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //funciones
        [HttpPost("reservar/{nroSerieLocker}")]
        public async Task<IActionResult> Reservar([FromBody] Token token, string nroSerieLocker)
        {
            try
            {
                token.Confirmado = false;
                Locker locker = await _locker.GetLockerByNroSerie(nroSerieLocker);
                token.IdLocker = locker.Id;
                token.IdLockerNavigation = locker;
                var response = await _token.Reservar(token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarCompraToken([FromBody] int idToken)
        {
            try
            {
                var response = await _token.ConfirmarCompraToken(idToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AsignarTokenABox([FromBody] int idToken)
        {
            try
            {
                var response = await _token.AsignarTokenABox(idToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("disponibilidadLocker/{idSize:int}/{idLocker:int}/{inicio:datetime}/{fin:datetime}")]
        public async Task<IActionResult> CantDisponibleByLockerTamañoFechas(int idSize, int idLocker, DateTime inicio, DateTime fin)
        {
            try
            {
                Locker locker = await _locker.GetLockerById(idLocker);
                var response = await _token.CantDisponibleByLockerTamañoFechas(locker, idSize, inicio, fin);
                return Ok(response);
                //ejemplo: /disponibilidadLocker/3/5/2024-01-31T08:00:00/2024-02-01T12:00:00
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
