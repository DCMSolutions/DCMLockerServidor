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
        public async Task<List<ServerStatus>> GetListaDeLockers()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<ServerStatus>>("api/locker");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarLocker(ServerStatus locker)
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
        public async Task<bool> DeleteLocker(ServerStatus locker)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/locker/deleteLocker", locker);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EnviarListaDeLockers(List<ServerStatus> listaDeLockers)
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
        public async Task<bool> DeleteLockerToken(LockerToken lockerToken)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/locker/deleteLockerToken", lockerToken);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //crud empresas
        public async Task<List<Empresa>> GetListaDeEmpresas()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Empresa>>("api/Empresas");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Empresa> GetEmpresaPorId(int id)
        {
            try
            {
                var oRtaList = await _cliente.GetFromJsonAsync<List<Empresa>>("api/Empresas");
                var oRta = oRtaList.Where(x => x.Id == id).ToList().First();
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarEmpresa(Empresa empresa)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/addEmpresa", empresa);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarEmpresa(Empresa empresa)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/editEmpresa", empresa);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteEmpresa(Empresa empresa)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/deleteEmpresa", empresa);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //crud locales
        public async Task<List<Local>> GetListaDeLocales()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Local>>("api/Empresas/Locales");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Local>> GetLocalesPorIdEmpresa(int idEmpresa)
        {
            try
            {
                var oRtaList = await _cliente.GetFromJsonAsync<List<Local>>("api/Empresas/Locales");
                var oRta = oRtaList.Where(x => x.IdEmpresa == idEmpresa).ToList();
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Local> GetLocalPorIds(int id, int idEmpresa)
        {
            try
            {
                var oRtaList = await _cliente.GetFromJsonAsync<List<Local>>("api/Empresas/Locales");
                var oRta = oRtaList.Where(x => x.IdEmpresa == idEmpresa && x.Id == id).ToList().First();
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarLocal(Local local)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/addLocal", local);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarLocal(Local local)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/editLocal", local);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteLocal(Local local)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/deleteLocal", local);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Lockers a empresa falta


    }
}
