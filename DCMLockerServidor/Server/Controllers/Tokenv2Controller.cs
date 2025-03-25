using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tokenv2Controller : ControllerBase
    {
        private readonly ITokenRepositorio _token;

        public Tokenv2Controller(ITokenRepositorio token)
        {
            _token = token;
        }

        //CRUD
        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] Token Token)
        {
            try
            {
                //se pone token.FechaCreacion = DateTime.Now; y token.Contador = 0, si no gusta se cambia, debatir
                var response = await _token.AddToken(Token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
    }
}
