using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface IExternalRepositorio
    {
        Task<List<LockerToken>> GetTokens();
        Task<ServerStatus> GetLocker(string nroSerieLocker);
        Task<Tamaño> GetTamaño(int idTamaño);
        int CantidadByLockerTamaño(ServerStatus status, Tamaño tamaño);
        int Disponibilidad(ServerStatus status, DateTime desde, DateTime hasta, List<LockerToken> tokens, Tamaño tamaño);
        Task<LockerToken> GetTokenByCodigo(int code);
    }
}
