using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
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

        //------------------------------------------//
        //El CRUD
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
        public async Task<Token> GetTokenById(int idToken)
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdBoxNavigation)
                    .Include(e => e.IdLockerNavigation)
                    .ThenInclude(e => e.Boxes)
                    .Where(tok => tok.Id == idToken)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Token>> GetTokensByLocker(int idLocker)
        {
            try
            {
                return await _dbContext.Tokens
                    //.Include(e => e.IdBoxNavigation)
                    //.Include(e => e.IdLockerNavigation)
                    //.ThenInclude(e => e.Boxes)
                    .Where(tok => tok.IdLocker == idLocker)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> AddToken(Token token)
        {
            try
            {
                token.FechaCreacion = DateTime.Now;
                _dbContext.Add(token);
                await _dbContext.SaveChangesAsync();
                return token.Id;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> EditToken(Token token)
        {
            try
            {

                var existingToken = await _dbContext.Tokens.FindAsync(token.Id);

                if (existingToken == null)
                {
                    // Locker with the given ID not found
                    return 0;
                }

                _dbContext.Update(existingToken).CurrentValues.SetValues(token);
                await _dbContext.SaveChangesAsync();
                return token.Id;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteToken(int idToken)
        {
            try
            {
                var token = await GetTokenById(idToken);
                _dbContext.Tokens.Remove(token);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        //------------------------------------------//
        //Funciones utiles
        public async Task<ServerToken> VerifyToken(ServerToken token, Locker locker)
        {

            ServerToken response = new();

            var tokens = await GetTokensValidosByLockerFechas(locker.Id, DateTime.Now, DateTime.Now);
            if (tokens.Any(x => x.Token1 == token.Token))
                response.Box = tokens.Where(x => x.Token1 == token.Token).First().IdBoxNavigation.IdFisico;

            return response;

        }
        public async Task<int> Reservar(Token token)
        {
            if (token.IdLocker == null || token.FechaInicio == null || token.FechaFin == null || token.Modo == null || token.IdSize == null)
            {
                return 0;  //aca va que algo estuvo mal pasado
            }

            int disp = await CantDisponibleByLockerTamañoFechas(token.IdLockerNavigation, token.IdSize.Value, token.FechaInicio.Value, token.FechaFin.Value);
            if (disp == 0) return 0;
            //listo, todo chequeado, ahora se puede reservar
            var result = await AddToken(token);
            return result;
        }
        public async Task<int> ConfirmarCompraToken(int idToken)
        {
            Token token = await GetTokenById(idToken);
            if (token != null && token.Confirmado != true)      //que hago si ya esta confirmado? 
            {
                List<Token> tokens = await GetTokensValidosByLockerFechas(token.IdLocker.Value, DateTime.Now, DateTime.Now);
                int token1 = GenerarRandomTokenNuevo(tokens);
                token.Confirmado = true;
                token.Token1 = token1.ToString();
                var result = await EditToken(token);
                if (result != 0) return token1;
                else return 0;
            }
            else
            {
                return 0;
            }
        }

        //public async Task<int?> AsignarTokenABox(int idToken)
        //{
        //    Token token = await GetTokenById(idToken);
        //    Locker locker = token.IdLockerNavigation;
        //    List<Token> listaTokens = await GetTokensValidosByLocker(token.IdLocker);
        //    List<Box> boxesAsignados = listaTokens.Where(t => t.IdSize == token.IdSize && t.IdBox != null).Select(t => t.IdBoxNavigation).ToList();

        //    Box box;
        //    //el if de abajo te da el box del primero que esté con el mismo Size del mismo locker, el else chequea tambien que no esté asignado
        //    if (boxesAsignados.Count == 0)
        //    {
        //        box = locker.Boxes.Where(b => b.IdSize == token.IdSize).First();
        //    }
        //    else
        //    {
        //        box = locker.Boxes.Where(b => b.IdSize == token.IdBoxNavigation.IdSize && !boxesAsignados.Contains(b.Box1)).First();
        //    }

        //    token.FechaCreacion = DateTime.Now;     //aca dejo esto? elijamos si fecha creacion es la de confirmacion o la de creacion (no cambia xd)
        //    token.IdBox = box.Id;
        //    bool result = await EditToken(token);

        //    return (int?)box.Box1;
        //}




        //Funciones auxiliares
        public async Task<int> CantDisponibleByLockerTamañoFechas(Locker locker, int idSize, DateTime inicio, DateTime fin)
        {
            int cantBoxesDisponiblesByTamaño = locker.Boxes.Count(box => box.IdSize == idSize);
            int maxTokensEnUnDia = 0;

            for (DateTime date = inicio; date <= fin; date = date.AddDays(1))
            {
                List<Token> tokens = await GetTokensValidosByLockerFechas(locker.Id, date, date);
                if (tokens.Count() > maxTokensEnUnDia) maxTokensEnUnDia = tokens.Count();
            }

            return cantBoxesDisponiblesByTamaño - maxTokensEnUnDia;
        }

        public async Task<List<Token>> GetTokensValidosByLockerFechas(int idLocker, DateTime inicio, DateTime fin)
        {
            //tener en cuenta que si inicio y fin son Datetime.Now lo unico que chequea es si la fecha de hoy esta entre el inicio y fin del locker
            List<Token> listaTokens = await GetTokensByLocker(idLocker);
            List<Token> result = listaTokens.Where(tok => CheckIntersection(inicio, fin, tok.FechaInicio.Value, tok.FechaFin.Value)).ToList();
            return result;
        }

        public bool CheckIntersection(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (end1 < start2 || start1 > end2)
            {
                return false;
            }
            return true;
        }

        public int GenerarRandomTokenNuevo(List<Token> tokenList)
        {
            int token = 0;
            //esta funcion asume que la tokenList es toda de los tokens validos (en fecha, confirmados y de un mismo locker)
            //(que pasa si hay dos lockers en un mismo local? usan la misma pantallita? usan distinta?)
            List<int> listaTokens = tokenList
                .Where(token => !string.IsNullOrEmpty(token.Token1))
                .Select(token => int.Parse(token.Token1)).ToList();

            while (token == 0 || listaTokens.Contains(token))
            {
                Random random = new Random();
                token = random.Next(100000, 1000000);
            }
            return token;
        }
    }
}
