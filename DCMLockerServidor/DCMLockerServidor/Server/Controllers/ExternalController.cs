﻿using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Server.Repositorio.Implementacion;
using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.Json;
using static DCMLockerServidor.Server.Controllers.EmpresasController;
using static DCMLockerServidor.Server.Controllers.LockerController;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly ILogger<LockerController> _logger;
        private readonly ServerHub _serverHub;
        private readonly HttpClient _httpClient;
        private readonly IExternalRepositorio _external;
        private readonly ILockerRepositorio _locker;
        private IHubContext<ServerHub> _hubContext;
        public ExternalController(ILogger<LockerController> logger, IHubContext<ServerHub> hubContext, ServerHub serverHub, HttpClient httpClient, IExternalRepositorio external, ILockerRepositorio locker)
        {
            _logger = logger;
            _serverHub = serverHub;
            _httpClient = httpClient;
            _external = external;
            _locker = locker;
        }

        //get todos los tokens
        [HttpGet("allToken")]
        public async Task<List<LockerToken>> GetAllToken()
        {
            return await _external.GetTokens();
        }

        //tokens de un locker
        [HttpGet("tokenLocker")]
        public async Task<List<LockerToken>> GetTokensDeLocker(string nroSerieLocker)
        {
            List<LockerToken>? tokens = await _external.GetTokens();
            List<LockerToken> tokensLocker = tokens.Where(x => x.Locker.NroSerie == nroSerieLocker).ToList();
            return tokensLocker;
        }

        //crea un token
        [HttpGet("createToken")]
        public async Task<int> PostToken(DateTime desde, DateTime hasta, int idTamaño, string nroSerieLocker, int cantidad)
        {
            List<LockerToken> listaTokens = await _external.GetTokens();
            ServerStatus locker = await _external.GetLocker(nroSerieLocker);
            Tamaño tamaño = await _external.GetTamaño(idTamaño);
            bool isDisponible = _external.Disponibilidad(locker, desde, hasta, listaTokens, tamaño) > 0;


            if (isDisponible)
            {
                int token = _locker.GenerarRandomTokenNuevo(listaTokens);
                int idToken = _locker.AgregarLockerToken(new LockerToken()
                {
                    Locker = locker,
                    Token = token,
                    Box = null,
                    Tamaño = tamaño,
                    FechaInicio = desde,
                    FechaFin = hasta,
                    FechaCreacion = DateTime.Now,
                    Modo = null,
                    Contador = cantidad,
                    Confirmado = false
                });
                return idToken;
            }
            return 0;
        }

        //confirmar un token
        [HttpGet("confirmarToken")]
        public async Task<int> ConfirmarToken(int idToken)
        {
            return _locker.ConfirmarLockerToken(idToken);
        }

        //get cantidad de disponibles por fecha y tamaño y locker
        [HttpGet("disponibles")]
        public async Task<int> DisponiblesByTodo(DateTime desde, DateTime hasta, string nroSerieLocker, int idTamaño)
        {
            ServerStatus status = await _external.GetLocker(nroSerieLocker);
            List<LockerToken> tokensDelLocker = await _external.GetTokens();
            Tamaño tamaño = await _external.GetTamaño(idTamaño);
            int result = _external.Disponibilidad(status, desde, hasta, tokensDelLocker, tamaño);
            return result;
        }

        //dado un token te da un box 
        [HttpGet("asignar")]
        public async Task<int?> Asign(ServerToken serverCommunication)
        {
            string nroSerieLocker = serverCommunication.NroSerie;
            int token = Convert.ToInt16(serverCommunication.Token);
            LockerToken lockerToken = await _external.GetTokenByCodigo(token);

            ServerStatus locker = await _external.GetLocker(nroSerieLocker);

            if (lockerToken.Box != null) return lockerToken.Box; //chequear que esté en fecha y que no sea null

            return _locker.Asignar(lockerToken, locker);
        }

        //falta verificacion de confirmar en token
    }
}
