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
        private readonly ILockerRepositorio _locker;

        public WebhookLockerController(ILockerRepositorio locker)
        {
            _locker = locker;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] Webhook webhook)
        {
            try
            {
                // Log webhook receipt (you can replace this with database storage)
                Console.WriteLine($"Webhook Received: {webhook.Evento}");
                Console.WriteLine($"Fecha: {webhook.FechaCreacion}");
                Console.WriteLine($"Locker: {webhook.NroSerieLocker}");
                Console.WriteLine($"Data: {webhook.Data}");

                // TODO: Add your processing logic here (e.g., store in DB, trigger events)

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
        public string? Data { get; set; } // Nullable to avoid issues with missing fields

        // Parameterless constructor (required for deserialization)
        public Webhook() { }

        public Webhook(string evento, string nroSerieLocker, object data)
        {
            FechaCreacion = DateTime.Now;
            Evento = evento;
            NroSerieLocker = nroSerieLocker;
            Data = data != null ? JsonSerializer.Serialize(data) : null;
        }
    }

}
