using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        //CRUD
        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        //funciones

        [Authorize]
        [HttpPost("AddListSizes")]
        public async Task<IActionResult> AddListSizes([FromBody] List<Size> sizes)
        {
            try
            {
                var response = await _Size.AddListSizes(sizes);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //test
        [HttpGet("Hola")]
        public IActionResult ObtenerRecursoProtegido()
        {
            // Obtener el header Authorization
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { mensaje = "Token no proporcionado o formato incorrecto" });
            }

            // Extraer el token real
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Validar el token en la base de datos o contra una lista de tokens permitidos
            if (!ValidarTokenEmpresa(token))
            {
                return Forbid("Token inválido");
            }

            return Ok(new { mensaje = "Hola capo", empresa = "Nombre de la empresa asociada" });
        }

        private bool ValidarTokenEmpresa(string token)
        {
            // Aquí deberías validar el token contra una base de datos o lista de tokens
            return token == "token_de_prueba"; // Reemplázalo con lógica real
        }
    }
}
