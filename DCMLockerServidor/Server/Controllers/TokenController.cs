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
    public class TokenController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILockerRepositorio _locker;
        private readonly ITokenRepositorio _token;
        private readonly IConfiguration _configuration;

        public TokenController(IMapper mapper, ILockerRepositorio locker, ITokenRepositorio token, IConfiguration configuration)
        {
            _mapper = mapper;
            _locker = locker;
            _token = token;
            _configuration = configuration;
        }

        //CRUD
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

        [HttpGet("byToken/{nroSerieLocker}/{token}")]
        public async Task<IActionResult> GetTokenByTokenLocker(string nroSerieLocker, string token)
        {
            try
            {
                var response = await _token.GetTokenByTokenLocker(token, nroSerieLocker);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byEmpresa")]
        public async Task<IActionResult> GetTokensByEmpresa([FromBody] string tokenEmpresa)
        {
            try
            {
                var response = await _token.GetTokensByEmpresa(tokenEmpresa);
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

        [HttpGet("disponibilidadLockerBySize/{idSize:int}/{nroSerieLocker}/{inicio:datetime}/{fin:datetime}")]
        public async Task<IActionResult> CantDisponibleByLockerTamañoFechas(int idSize, string nroSerieLocker, DateTime inicio, DateTime fin)
        {
            try
            {
                Locker locker = await _locker.GetLockerByNroSerie(nroSerieLocker);
                var response = await _token.CantDisponibleByLockerTamañoFechas(locker, idSize, inicio, fin);
                return Ok(response);
                //ejemplo: /disponibilidadLocker/3/5/2024-01-31T08:00:00/2024-02-01T12:00:00
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("disponibilidadLocker/{nroSerieLocker}/{inicio:datetime}/{fin:datetime}")]
        public async Task<IActionResult> CantDisponibleByLockerFechas(string nroSerieLocker, DateTime inicio, DateTime fin)
        {
            try
            {
                Locker locker = await _locker.GetLockerByNroSerie(nroSerieLocker);
                List<SizeDTO> listaDeSizesConCantidad = new();
                if (locker != null)
                {
                    foreach (var size in locker.Boxes.Where(box => box.IdSize != null && box.Enable == true ).Select(box => box.IdSizeNavigation).Distinct())
                    {
                        SizeDTO sizeDTO = _mapper.Map<SizeDTO>(size);
                        var cant = await _token.CantDisponibleByLockerTamañoFechas(locker, size.Id, inicio, fin);
                        sizeDTO.Cantidad = cant;
                        listaDeSizesConCantidad.Add(sizeDTO);
                    }
                    return Ok(listaDeSizesConCantidad);
                }
                else
                {
                    return Ok(listaDeSizesConCantidad);
                }
                //ejemplo: /disponibilidadLocker/pepepep/2024-01-31T08:00:00/2024-02-01T12:00:00
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //gestion tiempo del token deleter

        [HttpGet("GetTimeDeleter")]
        public IActionResult GetInterval()
        {
            var configPath = "appsettings.json";
            var json = System.IO.File.ReadAllText(configPath);
            using var doc = JsonDocument.Parse(json);
            var root = JsonNode.Parse(json)!.AsObject();

            if (root.TryGetPropertyValue("TokenDeleterConfigTime", out JsonNode? intervalNode))
            {
                int intervalInMinutes = intervalNode.GetValue<int>();
                return Ok(intervalInMinutes);
            }

            return NotFound("No se encontro el valor.");
        }


        [HttpPost("TimeTokenDeleter")]
        public IActionResult UpdateInterval([FromBody] int intervalInMinutes)
        {
            var configPath = "appsettings.json";
            var json = System.IO.File.ReadAllText(configPath);

            using var doc = JsonDocument.Parse(json);
            var root = JsonNode.Parse(json)!.AsObject();

            root["TokenDeleterConfigTime"] = intervalInMinutes;

            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(root, options);

            System.IO.File.WriteAllText(configPath, updatedJson);

            Console.WriteLine("toma tu time actualizao: " + intervalInMinutes);

            return Ok();
        }
    }
}
