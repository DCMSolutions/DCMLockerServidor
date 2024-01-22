//using DCMLockerServidor.Server.Repositorio.Contrato;
//using DCMLockerServidor.Shared;
//using Microsoft.AspNetCore.Http.HttpResults;
//using System.Collections.Generic;
//using System.Linq.Dynamic.Core;
//using System.Linq.Expressions;
//using System.Text.Json;

//namespace DCMLockerServidor.Server.Repositorio.Implementacion
//{
//    public class ExternalRepositorio : IExternalRepositorio
//    {
//        public async Task<ServerStatus> GetLocker(string nroSerieLocker)
//        {
//            try
//            {
//                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");

//                if (System.IO.File.Exists(sf))
//                {
//                    string content = System.IO.File.ReadAllText(sf);
//                    List<ServerStatus> lockersList = JsonSerializer.Deserialize<List<ServerStatus>>(content);
//                    ServerStatus locker = lockersList.Where(x => x.NroSerie == nroSerieLocker).FirstOrDefault();
//                    return locker;
//                }
//                else
//                {
//                    ServerStatus emptyLocker = new ServerStatus();
//                    return emptyLocker; // devolver locker vacio si el archivo no existe.
//                }
//            }
//            catch
//            {
//                ServerStatus emptyLocker = new ServerStatus();
//                return emptyLocker; // devolver locker vacio si el archivo no existe.
//            }
//        }
//        public async Task<Size> GetSize(int idSize)
//        {
//            try
//            {

//                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Sizes.ans");

//                if (System.IO.File.Exists(sf))
//                {
//                    string content = System.IO.File.ReadAllText(sf);
//                    List<Size> SizesList = JsonSerializer.Deserialize<List<Size>>(content);
//                    Size Size = SizesList.Where(x => x.Id == idSize).FirstOrDefault();
//                    return Size;
//                }
//                else
//                {
//                    Size emptySize = new Size();
//                    return emptySize; // devolver Size vacio si el archivo no existe.
//                }
//            }
//            catch
//            {
//                Size emptySize = new Size();
//                return emptySize; // devolver Size vacio si el archivo no existe.
//            }
//        }

//        public async Task<List<LockerToken>> GetTokens()
//        {
//            try
//            {
//                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

//                if (System.IO.File.Exists(sf))
//                {
//                    string content = System.IO.File.ReadAllText(sf);
//                    List<LockerToken>? lockerTokenList = JsonSerializer.Deserialize<List<LockerToken>>(content);
//                    return lockerTokenList;
//                }
//                else
//                {
//                    List<LockerToken> emptyList = new List<LockerToken>();
//                    return emptyList; // devolver lista vacia si el archivo no existe.
//                }
//            }
//            catch
//            {
//                List<LockerToken> emptyList = new List<LockerToken>();
//                return emptyList; // devolver lista vacia si el archivo no existe.
//            }
//        }
//        public async Task<LockerToken> GetTokenByCodigo(int code)
//        {
//            List<LockerToken> lockerTokenList = await GetTokens();
//            LockerToken result = lockerTokenList.Where(x => x.Token == code).FirstOrDefault();
//            return result;
//        }
//        public int CantidadByLockerSize(ServerStatus status, Size Size)
//        {
//            return status.Locker.Where(x => x.Size == Size.Name).Count();
//        }
//        public int Disponibilidad(ServerStatus status, DateTime desde, DateTime hasta, List<LockerToken> tokensDelLocker, Size Size)
//        {
//            tokensDelLocker = tokensDelLocker.Where(x => x.Size.Id == Size.Id && x.Locker.NroSerie == status.NroSerie).ToList();
//            int disponibilidad = 999; //para poner minimo y que se quede con datos
//            int cantidadTotal = CantidadByLockerSize(status, Size);
//            for (var date = desde.Date; date <= hasta.Date; date = date.AddDays(1))
//            {
//                int usados = tokensDelLocker.Where(lockerToken => date >= lockerToken.FechaInicio.Date && date <= lockerToken.FechaFin.Date).ToList().Count;
//                if (disponibilidad > cantidadTotal - usados) disponibilidad = cantidadTotal - usados;
//            }
//            return disponibilidad;
//        }
//    }
//}