using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Client.Pages;

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
        public async Task<ServerStatus> GetLocker(string NroSerie)
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<ServerStatus>($"/api/locker/Serie?NroSerie={NroSerie}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                return await _cliente.GetFromJsonAsync<Empresa>($"api/Empresas/{id}");
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

        //Lockers a empresa
        public class LockerEmpresa
        {
            public string NroSerieLocker { get; set; }
            public int IdEmpresa { get; set; }
        }
        public async Task<Dictionary<int, List<string>>> GetLockersDeEmpresas()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<Dictionary<int, List<string>>>("api/Empresas/LockersDeEmpresas");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<string>> GetLockersSinAsignar()
        {
            try
            {
                Dictionary<int, List<string>> lockersAsignadosDicc = await GetLockersDeEmpresas();
                List<string> lockersAsignadosList = lockersAsignadosDicc.Values.SelectMany(list => list).ToList();
                List<ServerStatus> allLockers = await GetListaDeLockers();
                List<string> allLockersList = allLockers.Select(locker => locker.NroSerie).ToList();
                List<string> lockersSinAsignar = allLockersList.Except(lockersAsignadosList).ToList();
                return lockersSinAsignar;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<string>> GetLockersDeEmpresaPorId(int idEmpresa)
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<Dictionary<int, List<string>>>("api/Empresas/LockersDeEmpresas");
                if (oRta.ContainsKey(idEmpresa))
                {

                    var oRtaList = oRta[idEmpresa];
                    return oRtaList;
                }
                else
                {
                    List<string> listaVacia = new();
                    return listaVacia;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AddLockerAId(string nroLocker, int idEmpresa)
        {
            LockerEmpresa lockEmpr = new LockerEmpresa { NroSerieLocker = nroLocker, IdEmpresa = idEmpresa };
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/AddLockerAId", lockEmpr);
                await _cliente.PostAsJsonAsync("api/Locker/Empresa", lockEmpr);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteLockerConIdEmpresa(string nroLocker, int idEmpresa)
        {
            LockerEmpresa lockEmpr = new LockerEmpresa { NroSerieLocker = nroLocker, IdEmpresa = idEmpresa };
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/deleteLockerConIdEmpresa", lockEmpr);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteLockerSinIdEmpresa(string nroLocker)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresas/deleteLockerSinIdEmpresa", nroLocker);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
