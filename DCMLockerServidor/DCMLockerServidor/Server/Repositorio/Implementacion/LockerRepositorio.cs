using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class LockerRepositorio : ILockerRepositorio
    {
        public List<LockerToken> GetTokens()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<LockerToken>? lockerTokenList = JsonSerializer.Deserialize<List<LockerToken>>(content);
                    return lockerTokenList;
                }
                else
                {
                    List<LockerToken> emptyList = new List<LockerToken>();
                    return emptyList; // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                List<LockerToken> emptyList = new List<LockerToken>();
                return emptyList; // devolver lista vacia si el archivo no existe.
            }
        }
        public int AgregarLockerToken(LockerToken lockerToken)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);
                    if (listaDeLockersToken.Count > 0) lockerToken.Id = listaDeLockersToken.Select(t => t.Id).Max() + 1;
                    else lockerToken.Id = 1;

                    listaDeLockersToken.Add(lockerToken);

                    string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return lockerToken.Id;
                }
                else
                {
                    lockerToken.Id = 1;
                    List<LockerToken> listaDeLockersToken = new List<LockerToken> { lockerToken };
                    string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                    return 1;
                }

            }
            catch
            {
                throw;
            }
        }
        public int ConfirmarLockerToken(int idToken)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");
                string content = System.IO.File.ReadAllText(sf);
                int token = 0;
                List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);
                foreach (var item in listaDeLockersToken.Where(x => x.Id == idToken))
                {
                    item.Confirmado = true;
                    token = item.Token;
                }
                string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);
                using (StreamWriter b = System.IO.File.CreateText(sf))
                {
                    b.Write(s);
                }
                return token;
            }
            catch
            {
                throw;
            }
        }
        public int GenerarRandomTokenNuevo(List<LockerToken> tokenList)
        {
            List<int> listaTokens = tokenList.Select(p => p.Token).ToList();
            int token = 0;
            while (token == 0 || listaTokens.Contains(token))
            {
                Random random = new Random();
                token = random.Next(100000, 1000000);
            }
            return token;
        }
        void Guardar(List<LockerToken> listaTokens)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");
                string s = JsonSerializer.Serialize<List<LockerToken>>(listaTokens);

                using (StreamWriter b = System.IO.File.CreateText(sf))
                {
                    b.Write(s);
                }

            }
            catch
            {
                throw;
            }
        }
        public int? Asignar(LockerToken token, ServerStatus locker)
        {
            List<LockerToken> listaTokens = GetTokens();
            List<int?> boxesOcupados = listaTokens.Where(x => x.Tamaño.Id == token.Tamaño.Id && x.Box != null && x.Locker.NroSerie == locker.NroSerie).Select(x => x.Box).ToList();
            int? box;
            if (boxesOcupados.Count == 0) box = locker.Locker.Where(x => x.Size == token.Tamaño.Name).First().Id;
            else box = locker.Locker.Where(x => x.Size == token.Tamaño.Name && !boxesOcupados.Contains(x.Id)).First().Id;

            //posteas que asignaste a ese token (escribe toda la lista pero hacer con put despues)
            listaTokens.Where(x => x.Token == token.Token).First().Box=box;
            Guardar(listaTokens);

            return box;
        }
    }

}