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
        public IActionResult GetEmpresaById([FromBody] int Id)
        {
            return Ok(_empresa.GetEmpresaById(Id));
        }
        [HttpPost]
        public IActionResult AddEmpresa(Empresa Empresa)
        {
            return Ok(_empresa.AddEmpresa(Empresa));
        }
        [HttpPut]
        public IActionResult EditEmpresa(Empresa Empresa)
        {
            return Ok(_empresa.EditEmpresa(Empresa));
        }
        [HttpDelete("{idEmpresa:int}")]
        public async Task<IActionResult> DeleteEmpresa([FromBody] int idEmpresa)
        {
            Empresa Empresa = await _empresa.GetEmpresaById(idEmpresa);
            return Ok(_empresa.DeleteEmpresa(Empresa));
        }
    }
}
