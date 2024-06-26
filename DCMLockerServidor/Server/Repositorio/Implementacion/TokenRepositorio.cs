using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;





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
        //CRUD
        public async Task<List<Token>> GetTokens()
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdBoxNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .Include(e => e.IdLockerNavigation)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tokens");
            }
        }
        
        public async Task<List<Token>> GetTokensForDelete()
        {
            DateTime thresholdTime = DateTime.Now.AddMinutes(-30);                  //asssssssssssssssssssssssssssssssssssssssssssssssaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            try
            {
                return await _dbContext.Tokens
                    .Where(tok => tok.FechaCreacion != null && tok.FechaCreacion.Value < thresholdTime && tok.Confirmado != true)
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tokens para eliminar");
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
                throw new Exception("No se encontró el token");
            }
        }

        public async Task<Token> GetTokenByTokenLocker(string token, string nroSerieLocker)
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdBoxNavigation)
                    .Where(tok => tok.Token1 == token && tok.IdLockerNavigation.NroSerieLocker == nroSerieLocker)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                if (_dbContext.Tokens.Include(e => e.IdLockerNavigation).Where(tok => tok.IdLockerNavigation.NroSerieLocker == nroSerieLocker).ToList().Count == 0)
                    throw new Exception("No se encontró ningun token del locker");
                else
                {
                    throw new Exception("No se encontró el token");
                }
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
                throw new Exception("Hubo un error al buscar los tokens del locker");
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
                throw new Exception("No se pudo agregar el token");
            }
        }

        public async Task<int> EditToken(Token token)
        {
            try
            {
                var existingToken = await _dbContext.Tokens.FindAsync(token.Id);

                if (existingToken == null)
                {
                    throw new Exception("No se encontro token con ese id");
                }

                _dbContext.Update(existingToken).CurrentValues.SetValues(token);
                await _dbContext.SaveChangesAsync();
                return token.Id;
            }
            catch
            {
                throw new Exception("No se pudo editar el token");
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
                throw new Exception("No se pudo eliminar el token");
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
                throw new Exception("Un parámetro requerido está en null");
            }
            if (token.Modo != "Por cantidad" && token.Modo != "Por fecha") throw new Exception("El modo no esta permitido");
            int disp = await CantDisponibleByLockerTamañoFechas(token.IdLockerNavigation, token.IdSize.Value, token.FechaInicio.Value, token.FechaFin.Value);
            if (disp <= 0) throw new Exception("No hay disponibilidad");
            //listo, todo chequeado, ahora se puede reservar
            var result = await AddToken(token);
            return result;
        }

        public async Task<int> ConfirmarCompraToken(int idToken)
        {
            Token? token = await GetTokenById(idToken);

            if (token == null) throw new Exception("El id no pertenece a un token");

            if (token.Confirmado != true)
            {
                List<Token> tokens = await GetTokensValidosByLockerFechasSize(token.IdLocker.Value, token.IdSize.Value, DateTime.Now, DateTime.Now);
                int token1 = GenerarRandomTokenNuevo(tokens);
                token.Confirmado = true;
                token.Token1 = token1.ToString();
                await EditToken(token);
                return token1;
            }
            else
            {
                throw new Exception("Ya esta confirmado");
            }
        }

        public async Task<int> AsignarTokenABox(int idToken)
        {
            Token token = await GetTokenById(idToken);
            if (!CheckIntersection(token.FechaInicio.Value, token.FechaFin.Value, DateTime.Now, DateTime.Now)) throw new Exception("No está en fecha");
            Locker locker = token.IdLockerNavigation;
            List<Token> listaTokens = await GetTokensValidosByLockerFechas(token.IdLocker.Value, DateTime.Now, DateTime.Now);

            //quiero los tokens confirmados o recien creados (si hay un token no confirmado pero creado hace menos de 5 min cuenta)
            listaTokens = listaTokens.Where(tok => tok.Confirmado == true && tok.IdBox != null && tok.IdSize == token.IdSize).ToList();

            //tiene que ser List<int?> para que no llore, pero por la linea de arriba se que ninguno es null
            List<int?> boxesAsignados = listaTokens.Select(t => t.IdBox).ToList();
            List<Box> allBoxesBySize = locker.Boxes.Where(box => box.IdSize == token.IdSize && box.Enable == true && box.Ocupacion == false).ToList();
            Box? box;

            //el if de abajo te da el box del primero que esté con el mismo Size del mismo locker, el else chequea tambien que no esté asignado
            if (boxesAsignados.Count == 0)
            {
                box = allBoxesBySize.FirstOrDefault();
            }
            else
            {
                box = allBoxesBySize.Where(b => !boxesAsignados.Contains(b.Id)).FirstOrDefault();
            }
            if (box == null) throw new Exception("No hay disponibilidad");
            token.IdBox = box.Id;
            await EditToken(token);

            return box.IdFisico.Value;      //devuelve el numero de box (osea el sticker) para que el front lo muestre ez
        }

        //Funciones auxiliares
        public async Task<int> CantDisponibleByLockerTamañoFechas(Locker locker, int idSize, DateTime inicio, DateTime fin)
        {
            int cantBoxesDisponiblesByTamaño = locker.Boxes.Count(box => box.IdSize == idSize && box.Enable == true && box.Ocupacion == false);
            int maxTokensEnUnDia = 0;

            for (DateTime date = inicio; date <= fin; date = date.AddDays(1))
            {
                List<Token> tokens = await GetTokensValidosByLockerFechasSize(locker.Id, idSize, date, date);
                if (tokens.Count() > maxTokensEnUnDia) maxTokensEnUnDia = tokens.Count();
            }
            return cantBoxesDisponiblesByTamaño - maxTokensEnUnDia;
        }

        public async Task<List<Token>> GetTokensValidosByLockerFechasSize(int idLocker, int idSize, DateTime inicio, DateTime fin)
        {
            //tener en cuenta que si inicio y fin son Datetime.Now lo unico que chequea es si la fecha de hoy esta entre el inicio y fin del locker
            List<Token> listaTokens = await GetTokensByLocker(idLocker);
            listaTokens = listaTokens.Where(token => token.IdSize == idSize && ((DateTime.Now - token.FechaCreacion).Value.TotalMinutes < 5 || token.Confirmado == true)).ToList();
            List<Token> result = listaTokens.Where(tok => CheckIntersection(inicio, fin, tok.FechaInicio.Value, tok.FechaFin.Value)).ToList();
            return result;
        }

        public async Task<List<Token>> GetTokensValidosByLockerFechas(int idLocker, DateTime inicio, DateTime fin)
        {
            //tener en cuenta que si inicio y fin son Datetime.Now lo unico que chequea es si la fecha de hoy esta entre el inicio y fin del locker
            List<Token> listaTokens = await GetTokensByLocker(idLocker);
            listaTokens = listaTokens.Where(token => (DateTime.Now - token.FechaCreacion).Value.TotalMinutes < 5 || token.Confirmado == true).ToList();
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
