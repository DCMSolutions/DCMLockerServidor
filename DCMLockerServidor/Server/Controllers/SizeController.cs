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
            try
            {
                var response = await _Size.GetSizes();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetSizeById(int Id)
        {
            try
            {
                var response = await _Size.GetSizeById(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSize([FromBody] Size Size)
        {
            try
            {
                var response = await _Size.AddSize(Size);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditSize(Size Size)
        {
            try
            {
                var response = await _Size.EditSize(Size);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idSize:int}")]
        public async Task<IActionResult> DeleteSize(int idSize)
        {
            try
            {
                var response = await _Size.DeleteSize(idSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
