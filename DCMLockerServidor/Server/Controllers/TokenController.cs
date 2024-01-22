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
            var result = await _token.GetTokens();
            return Ok(result);
        }
        [HttpGet("{Id:int}")]
        public IActionResult GetTokenById([FromBody] int Id)
        {
            return Ok(_token.GetTokenById(Id));
        }
        [HttpPost("addToken")]
        public async Task<IActionResult> AddToken(Token Token)
        {
            try
            { 
                var response =await _token.AddToken(Token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("editToken")]
        public IActionResult EditToken(Token Token)
        {
            return Ok(_token.EditToken(Token));
        }
        [HttpPost("deleteToken")]
        public async Task<IActionResult> DeleteToken(Token Token)
        {
            await _token.DeleteToken(Token);
            return Ok("Delete was successful");
        }

    }
}
