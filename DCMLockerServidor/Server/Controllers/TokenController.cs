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
            var response = await _token.GetTokens();
            return Ok(response);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetTokenById(int Id)
        {
            var response = await _token.GetTokenById(Id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] Token Token)
        {
            var response = await _token.AddToken(Token);
            if (response)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditToken(Token Token)
        {
            var response = await _token.EditToken(Token);
            if (response)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idToken:int}")]
        public async Task<IActionResult> DeleteToken(int idToken)
        {
            Token token = await _token.GetTokenById(idToken);
            var response = await _token.DeleteToken(token);
            if (response)
            {

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarCompraToken([FromBody] int idToken)
        {
            var response = await _token.ConfirmarCompraToken(idToken);
            if (response != 0)
            {
                Console.WriteLine(response);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AsignarTokenABox([FromBody] Token Token)
        {
            Console.WriteLine("assign");
            var response = await _token.AsignarTokenABox(Token);
            if (response != null)
            {
                Console.WriteLine(response);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
