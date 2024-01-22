using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Server.Repositorio.Contrato;
//using DCMLockerServidor.Server.Repositorio.Implementacion;
using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.Json;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {

        private readonly ServerHub _serverHub;
        private readonly HttpClient _httpClient;
        private readonly IExternalRepositorio _external;
        private readonly ILockerRepositorio _locker;
        private IHubContext<ServerHub> _hubContext;
        public ExternalController( IHubContext<ServerHub> hubContext, ServerHub serverHub, HttpClient httpClient, IExternalRepositorio external, ILockerRepositorio locker)
        {

            _serverHub = serverHub;
            _httpClient = httpClient;
            _external = external;
            _locker = locker;
        }

        //get todos los tokens
        //[HttpGet("allToken")]
        //public async Task<List<LockerToken>> GetAllToken()
        //{
        //    return await _external.GetTokens();
        //}

        //tokens de un locker
        //[HttpGet("tokenLocker")]
        //public async Task<List<LockerToken>> GetTokensDeLocker(string nroSerieLocker)
        //{
        //    List<LockerToken>? tokens = await _external.GetTokens();
        //    List<LockerToken> tokensLocker = tokens.Where(x => x.Locker.NroSerie == nroSerieLocker).ToList();
        //    return tokensLocker;
        //}

        ////crea un token
        //[HttpGet("createToken")]
        //public async Task<int> PostToken(DateTime desde, DateTime hasta, int idSize, string nroSerieLocker, int cantidad)
        //{
        //    List<LockerToken> listaTokens = await _external.GetTokens();
        //    ServerStatus locker = await _external.GetLocker(nroSerieLocker);
        //    Size Size = await _external.GetSize(idSize);
        //    bool isDisponible = _external.Disponibilidad(locker, desde, hasta, listaTokens, Size) > 0;


        //    if (isDisponible)
        //    {
        //        //int token = _locker.GenerarRandomTokenNuevo(listaTokens);
        //        int token = 0;
        //        //int idToken = _locker.AgregarLockerToken(new LockerToken()
        //        //{
        //        //    Locker = locker,
        //        //    Token = token,
        //        //    Box = null,
        //        //    Size = Size,
        //        //    FechaInicio = desde,
        //        //    FechaFin = hasta,
        //        //    FechaCreacion = DateTime.Now,
        //        //    Modo = null,
        //        //    Contador = cantidad,
        //        //    Confirmado = false
        //        //});
        //        int idToken = 1;
        //        return idToken;
        //    }
        //    return 0;
        //}

        //confirmar un token
        //[HttpGet("confirmarToken")]
        //public async Task<int> ConfirmarToken(int idToken)
        //{
        //    return _locker.ConfirmarLockerToken(idToken);
        //}

        //get cantidad de disponibles por fecha y Size y locker
        //[HttpGet("disponibles")]
        //public async Task<int> DisponiblesByTodo(DateTime desde, DateTime hasta, string nroSerieLocker, int idSize)
        //{
        //    ServerStatus status = await _external.GetLocker(nroSerieLocker);
        //    List<LockerToken> tokensDelLocker = await _external.GetTokens();
        //    Size Size = await _external.GetSize(idSize);
        //    int result = _external.Disponibilidad(status, desde, hasta, tokensDelLocker, Size);
        //    return result;
        //}

        //dado un token te da un box 
        //[HttpGet("asignar")]
        //public async Task<int?> Asign(ServerToken serverCommunication)
        //{
        //    string nroSerieLocker = serverCommunication.NroSerie;
        //    int token = Convert.ToInt16(serverCommunication.Token);
        //    LockerToken lockerToken = await _external.GetTokenByCodigo(token);

        //    ServerStatus locker = await _external.GetLocker(nroSerieLocker);

        //    if (lockerToken.Box != null) return lockerToken.Box; //chequear que esté en fecha y que no sea null

        //    return _locker.Asignar(lockerToken, locker);
        //}

        //falta verificacion de confirmar en token
    }
}
