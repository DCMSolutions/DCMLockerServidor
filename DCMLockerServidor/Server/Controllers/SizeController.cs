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
    public class SizeController : ControllerBase
    {

        private readonly ISizeRepositorio _Size;

        public SizeController(ISizeRepositorio Size)
        {
            _Size = Size;
        }


        //El CRUD
        [HttpGet]
        public async Task<IActionResult> GetSizes()
        {
            var response = await _Size.GetSizes();
            return Ok(response);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetSizeById(int Id)
        {
            var response = await _Size.GetSizeById(Id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddSize([FromBody]Size Size)
        {
            var response = await _Size.AddSize(Size);
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
        public async Task<IActionResult> EditSize(Size Size)
        {
            var response = await _Size.EditSize(Size);
            if (response)
            {

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idSize:int}")]
        public async Task<IActionResult> DeleteSize(int idSize)
        {
            Size Size = await _Size.GetSizeById(idSize);
            var response = await _Size.DeleteSize(Size);
            if (response)
            {

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
