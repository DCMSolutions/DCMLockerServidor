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
        Task<List<Token>> GetTokensModoFecha();
        Task<List<Token>> GetTokensForDelete();
        Task<Token> GetTokenById(int idToken);
        Task<Token> GetTokenByTokenLocker(string token, string nroSerieLocker);
        Task<List<Token>> GetTokensByLocker(int idLocker);
        Task<List<Token>> GetTokensByEmpresa(string tokenEmpresa);
        Task<int> AddToken(Token Token);
        Task<int> EditToken(Token Token);
        Task<bool> DeleteToken(int idToken);
        
        //funciones
        Task<bool> VerifyToken(Token token);
        Task<int> Reservar(Token token);
        Task<int> ConfirmarCompraToken(int idToken);
        Task<int> AsignarTokenABox(int idToken);
        Task<int> CantDisponibleByLockerTamañoFechas(Locker locker, int idSize, DateTime inicio, DateTime fin);
        Task<bool> ExtenderToken(int idToken, DateTime fin);
    }
}
