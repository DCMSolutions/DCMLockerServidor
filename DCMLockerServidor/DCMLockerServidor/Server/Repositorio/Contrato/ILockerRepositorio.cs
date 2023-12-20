using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface ILockerRepositorio
    {
        int AgregarLockerToken(LockerToken lockerToken);
        int ConfirmarLockerToken(int idToken);
        int GenerarRandomTokenNuevo(List<LockerToken> tokenList);
        int? Asignar(LockerToken token, ServerStatus locker);
    }
}
