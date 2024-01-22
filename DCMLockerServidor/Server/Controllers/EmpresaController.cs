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
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaRepositorio _empresa;

        public EmpresaController(IEmpresaRepositorio empresa)
        {
            _empresa = empresa;
        }


        //El CRUD
        [HttpGet]
        public async Task<IActionResult> GetEmpresas()
        {
            var response = await _empresa.GetEmpresas();
            return Ok(response);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetEmpresaById(int Id)
        {
            var response = await _empresa.GetEmpresaById(Id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmpresa(Empresa Empresa)
        {
            var response = await _empresa.AddEmpresa(Empresa);
            if (response)
            {

            return Ok();
            }else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditEmpresa(Empresa Empresa)
        {
            var response = await _empresa.EditEmpresa(Empresa);
            if (response)
            {

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idEmpresa:int}")]
        public async Task<IActionResult> DeleteEmpresa(int idEmpresa)
        {
            Empresa Empresa = await _empresa.GetEmpresaById(idEmpresa);
            var response = await _empresa.DeleteEmpresa(Empresa);
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
