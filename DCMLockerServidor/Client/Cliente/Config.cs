using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Shared.Models;
using System.Security.Principal;

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
        public async Task<List<Token>> GetListaDeLockersToken()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Token>>("api/Token");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarToken(Token Token)
        {

            try
            {
                await _cliente.PostAsJsonAsync("api/Token/addToken", Token);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteToken(Token Token)
        {
            try
            {
                
                Token.IdBoxNavigation = null;
                await _cliente.PostAsJsonAsync("api/Token/deleteToken", Token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //crud empresas
        public async Task<List<EmpresaOld>> GetListaDeEmpresas()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<EmpresaOld>>("api/Empresas");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<EmpresaOld> GetEmpresaPorId(int id)
        {
            try
            {
                return await _cliente.GetFromJsonAsync<EmpresaOld>($"api/Empresas/{id}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarEmpresa(EmpresaOld empresa)
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
        public async Task<bool> EditarEmpresa(EmpresaOld empresa)
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
        public async Task<bool> DeleteEmpresa(EmpresaOld empresa)
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
                List<Locker> allLockers = await GetListaDeLockers();
                List<string> allLockersList = allLockers.Select(locker => locker.NroSerieLocker).ToList();
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

        //tamaños
        public async Task<List<Tamaño>> GetTamaños()
        {
            try
            {
                var tamaños = await _cliente.GetFromJsonAsync<List<Tamaño>>("api/Locker/GetSizes");
                return tamaños;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
