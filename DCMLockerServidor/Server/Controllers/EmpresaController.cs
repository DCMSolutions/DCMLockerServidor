using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaRepositorio _empresa;

        public EmpresaController(IEmpresaRepositorio empresa)
        {
            _empresa = empresa;
        }

        //CRUD

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmpresas()
        {
            try
            {
                var response = await _empresa.GetEmpresas();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetEmpresaById(int Id)
        {
            try
            {
                var response = await _empresa.GetEmpresaById(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEmpresa(Empresa Empresa)
        {
            try
            {
                var response = await _empresa.AddEmpresa(Empresa);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditEmpresa(Empresa Empresa)
        {
            try
            {
                var response = await _empresa.EditEmpresa(Empresa);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{idEmpresa:int}")]
        public async Task<IActionResult> DeleteEmpresa(int idEmpresa)
        {
            try
            {
                var response = await _empresa.DeleteEmpresa(idEmpresa);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //funciones

        [Authorize]
        [HttpPut("updateTokenEmpresa")]
        public async Task<IActionResult> UpdateTokenEmpresa([FromBody] int idEmpresa)
        {
            try
            {
                var response = await _empresa.UpdateTokenEmpresa(idEmpresa);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
