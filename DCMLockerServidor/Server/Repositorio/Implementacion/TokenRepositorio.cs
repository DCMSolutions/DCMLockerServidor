using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class TokenRepositorio : ITokenRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        public TokenRepositorio(DcmlockerContext dbContext)
        {
            _dbContext = dbContext;
        }
        //CRUD
        public async Task<List<Token>> GetTokens()
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdBoxNavigation)
                    .Include(e => e.IdLockerNavigation)
                    .AsNoTracking()
                    .ToListAsync();

            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Token>> GetTokensValidosByLocker(int? idLocker)
        {
            if (idLocker == null) return new List<Token>();
            List<Token> allTokens = await GetTokens();
            return allTokens.Where(token => token.IdLocker == idLocker && token.FechaInicio < DateTime.Now && token.FechaFin > DateTime.Now).ToList();
        }
        public async Task<Token> GetTokenById(int idToken)
        {
            List<Token> allTokens = await GetTokens();
            return allTokens.Where(token => token.Id == idToken).FirstOrDefault();
        }
        public async Task<bool> AddToken(Token Token)
        {
            try
            {
                Token.FechaCreacion = DateTime.Now;
                _dbContext.Set<Token>().Add(Token);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<bool> EditToken(Token Token)
        {
            return true;
        }
        public async Task<bool> DeleteToken(Token Token)
        {
            try
            {
               var token = await GetTokenById(Token.Id);
                _dbContext.Tokens.Remove(token);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<ServerToken> VerifyToken(ServerToken token, Locker locker)
        {

            ServerToken response = new();

            var tokens = await GetTokensValidosByLocker(locker.Id);
            if (tokens.Any(x => x.Token1 == token.Token))
                response.Box = tokens.Where(x => x.Token1 == token.Token).First().IdBoxNavigation.IdFisico;


            return response;

        }


        //------------------------------------------//
        //Funciones utiles
        //public async Task<int> ConfirmarCompraToken(int idToken)
        //{
        //    Token token = await GetTokenById(idToken);
        //    List<Token> tokens = await GetTokensValidosByLocker(token.IdLocker);
        //    int token1 = GenerarRandomTokenNuevo(tokens);
        //    token.Confirmado = true;
        //    token.Token1 = token1;
        //    bool result = await EditToken(token);
        //    if (result) return token1;
        //    else return 0;
        //}
        public async Task<int> AsignarTokenABox(Token token)
        {
            List<Token> listaTokens = await GetTokensValidosByLocker(token.IdLocker);
            List<int?> boxesAsignados = listaTokens.Where(t => t.IdSize == token.IdSize && t.IdBox != null).Select(t => t.IdBoxNavigation.Box1).ToList();
            Box box;
            //el if de abajo te da el box del primero que esté con el mismo Size del mismo locker, el else chequea tambien que no esté asignado
            if (boxesAsignados.Count(x => x.HasValue) == 0) box = token.IdLockerNavigation.Boxes.Where(b => b.IdSize == token.IdBoxNavigation.IdSize).First();
            else box = token.IdLockerNavigation.Boxes.Where(b => b.IdSize == token.IdBoxNavigation.IdSize && !boxesAsignados.Contains(b.Box1)).First();

            token.FechaCreacion = DateTime.Now;     //aca dejo esto? elijamos si fecha creacion es la de confirmacion o la de creacion (no cambia xd)
            token.IdBox = box.Id;
            bool result = await EditToken(token);

            return (int)box.Box1;
        }

        //Funciones auxiliares
        //public int GenerarRandomTokenNuevo(List<Token> tokenList)
        //{
        //    //esta funcion asume que la tokenList es toda de los tokens validos (en fecha, confirmados y de un mismo locker)
        //    //(que pasa si hay dos lockers en un mismo local? usan la misma pantallita? usan distinta?)
        //    List<int?> listaTokens = tokenList.Select(token => token.Token1).ToList();
        //    int token = 0;
        //    while (token == 0 || listaTokens.Contains(token))
        //    {
        //        Random random = new Random();
        //        token = random.Next(100000, 1000000);
        //    }
        //    return token;
        //}
    }
}
