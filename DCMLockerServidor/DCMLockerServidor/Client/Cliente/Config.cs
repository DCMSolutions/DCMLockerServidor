using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DCMLockerServidor.Shared;

namespace DCMLockerServidor.Client.Cliente
{
    public class Config
    {
        private readonly HttpClient _cliente;

        public Config(HttpClient cliente)
        {
            _cliente = cliente;
        }
        //crud lista de lockers, num correspondiente y idserver
        public async Task<List<Locker>> GetListaDeLockers()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Locker>>("api/locker");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarLocker(Locker locker)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/locker/addLocker", locker);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> EnviarListaDeLockers(List<Locker> listaDeLockers)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/locker", listaDeLockers);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        //crud lista de locker token

        public async Task<List<LockerToken>> GetListaDeLockersToken()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<LockerToken>>("api/locker/Token");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AgregarLockerToken(LockerToken lockerToken)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/locker/addLockerToken", lockerToken);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
