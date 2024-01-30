using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;
using DCMLockerServidor.Shared.Models;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface ITokenRepositorio
    {
        Task<List<Token>> GetTokens();
        Task<List<Token>> GetTokensValidosByLocker(int? idLocker);
        Task<Token> GetTokenById(int idToken);
        Task<bool> AddToken(Token Token);
        Task<bool> EditToken(Token Token);
        Task<bool> DeleteToken(Token Token);
        Task<ServerToken> VerifyToken(ServerToken token, Locker locker);
        Task<int> ConfirmarCompraToken(int idToken);
        Task<int?> AsignarTokenABox(Token token);
    }
}
