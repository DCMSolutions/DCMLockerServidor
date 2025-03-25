using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using DCMLockerServidor.Server.Repositorio.Contrato;
using System;
using Azure;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookLockerController : ControllerBase
    {
        private readonly HttpClient _clienteHttp;
        private readonly IEventoRepositorio _evento;
        private readonly ILockerRepositorio _locker;
        private readonly IEmpresaRepositorio _empresa;

        public WebhookLockerController(HttpClient cliente, IEventoRepositorio evento, ILockerRepositorio locker, IEmpresaRepositorio empresa)
        {
            _clienteHttp = cliente;
            _evento = evento;
            _locker = locker;
            _empresa = empresa;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] Webhook webhook)
        {
            try
            {
                //el locker para todo
                Locker locker = await _locker.GetLockerByNroSerie(webhook.NroSerieLocker);

                //el evento del locker
                Evento evento = new()
                {
                    IdLocker = locker.Id,
                    FechaCreacion = webhook.FechaCreacion,
                    Descripcion = webhook.Descripcion,
                    Identificador = webhook.Evento
                };
                await _evento.AddEvento(evento);

                //el webhook al gestion
                List<EmpresaUrl> urls = await _empresa.GetUrlsByIdEmpresa(locker.Empresa);

                await SendWebhookToUrls(urls, webhook, locker.Id);
                

                //aca está el switch de los casos
                /*
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
                */



                return Ok(new { message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing webhook: {ex.Message}");
                return StatusCode(500, "Webhook processing failed");
            }
        }

        private async Task<bool> SendWebhookToUrls(List<EmpresaUrl> urls, Webhook webhook, int idLocker)
        {
            bool exito = true;
            foreach (EmpresaUrl empresaUrl in urls)
            {
                try
                {
                    var oRta = await _clienteHttp.PostAsJsonAsync(empresaUrl.Url, webhook);
                    if (!oRta.IsSuccessStatusCode)
                    {
                        exito = false;
                        var content = await oRta.Content.ReadAsStringAsync();
                        await _evento.AddEvento(new()
                        {
                            IdLocker = idLocker,
                            FechaCreacion = webhook.FechaCreacion,
                            Descripcion = $"El aviso {webhook.Evento} falló al enviarse a la url {empresaUrl.Url} por respuesta no exitosa: {content}",
                            Identificador = "FalloAviso"
                        });
                    }
                }
                catch (HttpRequestException ex) 
                {
                    exito = false;

                    await _evento.AddEvento(new()
                    {
                        IdLocker = idLocker,
                        FechaCreacion = webhook.FechaCreacion,
                        Descripcion = $"El aviso {webhook.Evento} falló por error externo en la url {empresaUrl.Url}: {ex.Message}",
                        Identificador = "FalloAviso"
                    });
                }
                catch (Exception ex)
                {
                    exito = false;
                    await _evento.AddEvento(new()
                    {
                        IdLocker = idLocker,
                        FechaCreacion = webhook.FechaCreacion,
                        Descripcion = $"El aviso {webhook.Evento} falló al enviarse a la url {empresaUrl.Url} por error al enviarlo: {ex.Message}",
                        Identificador = "FalloAviso"
                    });
                }
            }
            return exito;
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

}
