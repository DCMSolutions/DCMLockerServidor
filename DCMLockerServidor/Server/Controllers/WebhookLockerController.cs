using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using DCMLockerServidor.Server.Repositorio.Contrato;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookLockerController : ControllerBase
    {
        private readonly IEventoRepositorio _evento;
        private readonly ILockerRepositorio _locker;

        public WebhookLockerController(IEventoRepositorio evento, ILockerRepositorio locker)
        {
            _evento = evento;
            _locker = locker;
        }

        //get de test
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTest()
        {
            try
            {
                Console.WriteLine("okas");
                return Ok("okis");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] Webhook webhook)
        {
            try
            {
                int idLocker = await _locker.GetLockerIdByNroSerie(webhook.NroSerieLocker);

                Evento evento = new Evento
                {
                    IdLocker = idLocker,
                    FechaCreacion = webhook.FechaCreacion,
                    Descripcion = webhook.Descripcion,
                    Identificador = webhook.Evento
                };

                var response = await _evento.AddEvento(evento);

                //here do something with the data
                switch (webhook.Evento)
                {
                    case "PeticionToken":
                        var peticionToken = webhook.Data != null ? JsonSerializer.Deserialize<DataToken>(webhook.Data) : null;
                        if (peticionToken != null)
                        {
                            // Do something with peticionToken.Token
                        }
                        break;

                    case "RespuestaToken":
                        var respuestaToken = webhook.Data != null ? JsonSerializer.Deserialize<DataTokenBoxRespuesta>(webhook.Data) : null;
                        if (respuestaToken != null)
                        {
                            respuestaToken.Box = respuestaToken.Box != 0 ? respuestaToken.Box : 0; // Ensure it's never an idBox
                                                                                                   // Do something with respuestaToken.Token, respuestaToken.Box, respuestaToken.Respuesta
                        }
                        break;

                    case "LockerAbierto":
                    case "LockerCerrado":
                    case "SensorLiberado":
                    case "SensorOcupado":
                        var boxEvent = webhook.Data != null ? JsonSerializer.Deserialize<DataBox>(webhook.Data) : null;
                        if (boxEvent != null)
                        {
                            // Do something with boxEvent.Box
                        }
                        break;

                    case "ConfiguracionURL":
                    case "ConfiguracionID":
                        var configuracion = webhook.Data != null ? JsonSerializer.Deserialize<DataViejoNuevo>(webhook.Data) : null;
                        if (configuracion != null)
                        {
                            // Do something with configuracion.Viejo, configuracion.Nuevo
                        }
                        break;

                    case "Sistema":
                    case "Debug":
                    case "Cerraduras":
                    case "Conexion":
                        var accion = webhook.Data != null ? JsonSerializer.Deserialize<DataAccion>(webhook.Data) : null;
                        if (accion != null)
                        {
                            // Do something with accion.Accion
                        }
                        break;

                    default:
                        Console.WriteLine($"Unhandled Evento: {webhook.Evento}");
                        break;
                }



                return Ok(new { message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing webhook: {ex.Message}");
                return StatusCode(500, "Webhook processing failed");
            }
        }
    }


    public class Webhook
    {
        public DateTime FechaCreacion { get; set; }
        public string Evento { get; set; }
        public string NroSerieLocker { get; set; }
        public string Descripcion { get; set; }
        public string? Data { get; set; } // Nullable to avoid issues with missing fields

        // Parameterless constructor (required for deserialization)
        public Webhook() { }

        public Webhook(string evento, string nroSerieLocker, string descripcion, object data)
        {
            FechaCreacion = DateTime.Now;
            Evento = evento;
            NroSerieLocker = nroSerieLocker;
            Descripcion = descripcion;
            Data = data != null ? JsonSerializer.Serialize(data) : null;
        }
    }

    //las clases posibles de data del webhook:
    // Define DTOs for deserialization
    public class DataToken
    {
        public string Token { get; set; }
    }

    public class DataTokenBoxRespuesta
    {
        public string Token { get; set; }
        public int Box { get; set; }
        public string Respuesta { get; set; }
    }

    public class DataBox
    {
        public int Box { get; set; }
    }

    public class DataViejoNuevo
    {
        public string Viejo { get; set; }
        public string Nuevo { get; set; }
    }

    public class DataAccion
    {
        public string Accion { get; set; }
    }


}
