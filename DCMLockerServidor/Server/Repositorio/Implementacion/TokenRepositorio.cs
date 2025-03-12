using Blazored.Modal;
using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;





namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class TokenRepositorio : ITokenRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILockerRepositorio _locker;


        public TokenRepositorio(DcmlockerContext dbContext, IConfiguration configuration, ILockerRepositorio locker)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _locker = locker;
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

        public async Task<List<Token>> GetTokensModoFecha()
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdBoxNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .Include(e => e.IdLockerNavigation)
                    .AsNoTracking()
                    .Where(tok => tok.Modo == "Por fecha")
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tokens");
            }
        }

        public async Task<List<Token>> GetTokensForDelete()
        {
            int _intervalInMinutes = _configuration.GetValue<int>("TokenDeleterConfigTime");
            DateTime thresholdTime = DateTime.Now.AddMinutes(-_intervalInMinutes);
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
                    .Include(e => e.IdSizeNavigation)
                    .Include(e => e.IdLockerNavigation)
                    .ThenInclude(e => e.EmpresaNavigation)
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
                List<Token> resultList = await _dbContext.Tokens
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdBoxNavigation)
                    .Where(tok => tok.Token1 == token && tok.IdLockerNavigation.NroSerieLocker == nroSerieLocker).ToListAsync();

                if (resultList.Count == 0) throw new Exception("No se encontró ningun token con ese código");
                Token result = resultList.Where(tok => CheckIntersection(tok.FechaInicio.Value, tok.FechaFin.Value, DateTime.Now, DateTime.Now)).FirstOrDefault();
                return result;
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
                    .Include(e => e.IdBoxNavigation)
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

        public async Task<List<Token>> GetTokensByBox(int idBox)
        {
            try
            {
                return await _dbContext.Tokens
                    .Where(tok => tok.IdBox == idBox)
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tokens del locker");
            }
        }

        public async Task<List<Token>> GetTokensByEmpresa(string tokenEmpresa)
        {
            try
            {
                return await _dbContext.Tokens
                    .Include(e => e.IdLockerNavigation)
                    .ThenInclude(e => e.EmpresaNavigation)
                    .Where(tok => tok.IdLockerNavigation.EmpresaNavigation.TokenEmpresa == tokenEmpresa)
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tokens de la empresa");
            }
        }


        public async Task<int> AddToken(Token token)
        {
            try
            {
                token.FechaCreacion = DateTime.Now;
                token.Contador = 0;
                _dbContext.Add(token);
                await _dbContext.SaveChangesAsync();
                return token.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("No se pudo agregar el token");
            }
        }

        public async Task<int> EditToken(Token token)
        {
            try
            {
                // Obtener el token existente desde la base de datos
                var existingToken = await _dbContext.Tokens.FindAsync(token.Id);

                if (existingToken == null)
                {
                    throw new Exception("No se encontró token con ese ID");
                }


                _dbContext.Entry(existingToken).CurrentValues.SetValues(token);

                await _dbContext.SaveChangesAsync();
                return existingToken.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo editar el token", ex);
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
        public async Task<bool> VerifyToken(Token? token)
        {
            if (token == null) return false;
            if (token.Modo == "Por fecha" && !CheckIntersection(token.FechaInicio.Value, token.FechaFin.Value, DateTime.Now, DateTime.Now)) return false;
            if (token.Modo == "Por cantidad" && token.Cantidad <= token.Contador) return false;
            return true;
        }

        public async Task<int> Reservar(Token token)
        {
            if (token.IdLocker == null || token.FechaInicio == null || token.FechaFin == null || token.Modo == null || token.IdSize == null)
            {
                throw new Exception("Un parámetro requerido está en null");
            }
            if (token.Modo != "Por cantidad" && token.Modo != "Por fecha") throw new Exception("El modo no está permitido");
            if (token.IdLockerNavigation.Status != "connected") throw new Exception("El locker está desconectado");
            int disp = await CantDisponibleByLockerTamañoFechas(token.IdLockerNavigation, token.IdSize.Value, token.FechaInicio.Value, token.FechaFin.Value, true);
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
                List<Token> tokens = await GetTokensValidosByLockerFechas(token.IdLocker.Value, DateTime.Now, DateTime.Now, "Por fecha");
                int token1 = GenerarRandomTokenNuevo(tokens);
                token.Confirmado = true;
                if (token.Token1 == null) token.Token1 = token1.ToString();
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


            Locker locker = token.IdLockerNavigation;
            List<Token> listaTokens = await GetTokensValidosByLockerFechas(token.IdLocker.Value, DateTime.Now, DateTime.Now, token.Modo);

            //quiero los tokens confirmados o recien creados (si hay un token no confirmado pero creado hace menos de 5 min cuenta)
            listaTokens = listaTokens.Where(tok => tok.Confirmado == true && tok.IdBox != null && tok.IdSize == token.IdSize).ToList();

            //tiene que ser List<int?> para que no llore, pero por la linea de arriba se que ninguno es null
            List<int?> boxesAsignados = listaTokens.Select(t => t.IdBox).ToList();
            List<Box> allBoxesBySize = locker.Boxes.Where(box => box.IdSize == token.IdSize && box.Enable == true && box.Ocupacion == false).ToList();
            Box? box;

            //el if de abajo te da el box del primero que esté con el mismo Size del mismo locker, el else chequea tambien que no esté asignado
            if (boxesAsignados.Count == 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(allBoxesBySize.Count);
                box = allBoxesBySize[randomIndex];
            }
            else
            {
                Random random = new Random();
                int randomIndex = random.Next(allBoxesBySize.Where(b => !boxesAsignados.Contains(b.Id)).ToList().Count);
                box = allBoxesBySize.Where(b => !boxesAsignados.Contains(b.Id)).ToList()[randomIndex];
            }
            if (box == null) throw new Exception("No hay disponibilidad");
            token.IdBox = box.Id;
            //aca se le pone el mismo box a todas las extensiones de esa reserva (es decir las que tengan el mismo token)
            await CompletarBox(token.Token1, locker.NroSerieLocker, box.Id);
            token.Contador++;
            await EditToken(token);

            return box.IdFisico.Value;      //devuelve el numero de box (osea el sticker) para que el front lo muestre ez
        }

        public async Task<int> ExtenderToken(int idToken, DateTime fin)
        {
            Token? token = await GetTokenById(idToken);

            if (token == null) throw new Exception("El id no pertenece a un token");
            if (token.FechaInicio >= fin) throw new Exception("La fecha no es mayor a la de inicio");
            if (token.IdLockerNavigation.Status != "connected") throw new Exception("El locker está desconectado");

            //separo en casos donde la reserva sea valida hoy o no, en el primero no tengo complicaciones de que ya se haya reservado
            if (DateTime.Now < token.FechaFin)
            {
                Token newToken = new();
                newToken.FechaFin = fin;
                newToken.FechaInicio = token.FechaFin.Value.AddSeconds(1);
                newToken.Confirmado = false;
                newToken.IdBox = token.IdBox;
                newToken.IdLocker = token.IdLocker;
                newToken.Modo = token.Modo;
                newToken.Token1 = token.Token1;
                newToken.Cantidad = token.Cantidad;
                newToken.IdSize = token.IdSize;
                int id = await AddToken(newToken);
                return id;
            }
            else
            {
                //chequea que haya disponibilidad (si no hay, aunque su box no esté en otra reserva igual va a asignarse) y que su box no haya sido asignado
                DateTime finViejoMasUno = token.FechaFin.Value.AddSeconds(1);
                int cantDisp = await CantDisponibleByLockerTamañoFechas(token.IdLockerNavigation, token.IdSize.Value, finViejoMasUno, fin, false);     //asumo que la fecha fin es a las 23:59

                List<Token> tokensByBox = new();
                if (token.IdBox != null) tokensByBox = await GetTokensByBox(token.IdBox.Value);
                int tokensParaBoxFechas = tokensByBox.Count(tok => (tok.Id != idToken) && CheckIntersection(finViejoMasUno, fin, tok.FechaInicio.Value, tok.FechaFin.Value));

                if (cantDisp < 1 || tokensParaBoxFechas > 0) throw new Exception("Ya fue reservado");

                Token newToken = new();
                newToken.FechaFin = fin;
                newToken.FechaInicio = finViejoMasUno;
                newToken.Confirmado = false;
                newToken.IdBox = token.IdBox;
                newToken.IdLocker = token.IdLocker;
                newToken.Modo = token.Modo;
                newToken.Token1 = token.Token1;
                newToken.Cantidad = token.Cantidad;
                newToken.IdSize = token.IdSize;
                int id = await AddToken(newToken);
                return id;
            }
        }

        //Funciones auxiliares
        public async Task<int> CantDisponibleByLockerTamañoFechas(Locker locker, int idSize, DateTime inicio, DateTime fin, bool veoOcup)
        {
            int minBoxesLibresEnUnDia = locker.Boxes.Count(box => box.IdSize == idSize && box.Enable == true);  //este es el maximo que puede haber disponibles

            for (DateTime date = inicio; date <= fin; date = date.AddDays(1))
            {
                int cantBoxesDisp = await GetCantBoxLibresByLockerFechasSize(locker.Id, idSize, date, date, veoOcup);
                if (cantBoxesDisp < minBoxesLibresEnUnDia) minBoxesLibresEnUnDia = cantBoxesDisp;
            }

            return minBoxesLibresEnUnDia;
        }

        public async Task<List<Token>> GetTokensValidosByLockerFechas(int idLocker, DateTime inicio, DateTime fin, string modo)
        {
            //tener en cuenta que si inicio y fin son Datetime.Now lo unico que chequea es si la fecha de hoy esta entre el inicio y fin del locker
            List<Token> listaTokens = await GetTokensByLocker(idLocker);
            List<Token> result = new();
            listaTokens = listaTokens.Where(token => (DateTime.Now - token.FechaCreacion).Value.TotalMinutes < 5 || token.Confirmado == true).ToList();
            if (modo == "Por fecha") result = listaTokens.Where(tok => tok.Modo == "Por fecha" && CheckIntersection(inicio, fin, tok.FechaInicio.Value, tok.FechaFin.Value)).ToList();
            if (modo == "Por cantidad") result = listaTokens.Where(tok => tok.Modo == "Por cantidad" && tok.Cantidad > tok.Contador).ToList();
            return result;
        }

        public async Task<int> GetCantBoxLibresByLockerFechasSize(int idLocker, int idSize, DateTime inicio, DateTime fin, bool veoOcup)
        {
            //tener en cuenta que si inicio y fin son Datetime.Now lo unico que chequea es si la fecha de hoy esta entre el inicio y fin del locker
            List<Box> boxes = await _locker.GetBoxesByIdLocker(idLocker);

            List<Token> listaTokens = await GetTokensByLocker(idLocker);
            listaTokens = listaTokens.Where(token => token.IdSize == idSize && ((DateTime.Now - token.FechaCreacion).Value.TotalMinutes < 5 || token.Confirmado == true)).ToList();

            int result = 0;

            foreach (var box in boxes)
            {
                bool tieneTokenAsignado = listaTokens.Where(tok => tok.IdBox == box.Id
                                                && (tok.Modo == "Por fecha" || (tok.Modo == "Por cantidad" && tok.Cantidad > tok.Contador))
                                                && CheckIntersection(inicio, fin, tok.FechaInicio.Value, tok.FechaFin.Value)).Count() > 0;
                bool boxLibre = !(veoOcup && box.Ocupacion == true);
                if (boxLibre && box.IdSize == idSize && box.Enable == true && !tieneTokenAsignado)
                {
                    result++;
                }
            }
            result -= listaTokens.Where(tok => tok.IdBox == null && tok.Modo == "Por fecha" && CheckIntersection(inicio, fin, tok.FechaInicio.Value, tok.FechaFin.Value)).Count();

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

        public async Task CompletarBox(string token1, string nroSerieLocker, int boxId)
        {
            List<Token> reservasAsociadas = await _dbContext.Tokens
                    .Where(tok => tok.Token1 == token1 && tok.IdLockerNavigation.NroSerieLocker == nroSerieLocker).ToListAsync();

            foreach (Token token in reservasAsociadas)
            {
                token.IdBox = boxId;
                await EditToken(token);
            }
        }

    }
}
