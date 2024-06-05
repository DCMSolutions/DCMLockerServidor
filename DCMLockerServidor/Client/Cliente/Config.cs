using System.Net.Http.Json;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Components;

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
        public async Task<Locker> GetLockerById(int idLocker)
        {
            try
            {

                var oRta = await _cliente.GetFromJsonAsync<Locker>($"/api/locker/{idLocker}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<Locker> GetLockerByNroSerie(string NroSerie)
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<Locker>($"/api/locker/{NroSerie}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> AgregarLocker(Locker locker)
        {
            try
            {
                var result = await _cliente.PostAsJsonAsync("api/locker/addLocker", locker);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarLocker(Locker locker)
        {
            try
            {
                locker.Boxes = null;
                var response = await _cliente.PutAsJsonAsync("api/locker", locker);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteLocker(int idLocker)
        {
            try
            {
                Console.WriteLine(idLocker);
                await _cliente.DeleteAsync($"api/locker/{idLocker}");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //crud boxes
        public async Task<Box> GetBoxById(int idBox)
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<Box>($"/api/locker/box/{idBox}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //crud lista de token
        public async Task<List<Token>> GetListaDeToken()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Token>>("api/token");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarToken(Token token)
        {
            try
            {
                await _cliente.PostAsJsonAsync("api/token", token);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarToken(Token token)
        {
            try
            {
                await _cliente.PutAsJsonAsync("api/token", token);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteToken(int id)
        {
            try
            {
                await _cliente.DeleteAsync($"api/token/{id}");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        //funciones token
        public async Task<HttpResponseMessage> ConfirmarToken(int idToken)
        {
            try
            {
                var result = await _cliente.PostAsJsonAsync($"api/token/confirmar", idToken);
                //var content = await result.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error de conexion");
            }
        }

        public async Task<int> GetTimeDeleter()
        {
            try
            {
                var response = await _cliente.GetAsync("api/token/GetTimeDeleter");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    int intervalInMinutes;
                    if (int.TryParse(content, out intervalInMinutes))
                    {
                        return intervalInMinutes;
                    }
                    else
                    {
                        throw new FormatException("El tiempo guardado no es un entero ¿?.");
                    }
                }
                else
                {
                    throw new HttpRequestException($"Hubo un error de conexion");
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Hubo un error de conexion");
            }
        }


        public async Task<HttpResponseMessage> UpdateTokenDeleterTime(int timeDeleter)
        {
            try
            {
                var result = await _cliente.PostAsJsonAsync($"api/token/TimeTokenDeleter", timeDeleter);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar");
            }
        }

        //crud empresas
        public class LockerEmpresa
        {
            public string NroSerieLocker { get; set; }
            public int IdEmpresa { get; set; }
        }
        public async Task<List<Empresa>> GetListaDeEmpresas()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Empresa>>("api/Empresa");
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
                var oRta = await _cliente.GetFromJsonAsync<Empresa>($"api/Empresa/{id}");
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

                await _cliente.PostAsJsonAsync("api/Empresa", empresa);
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
                await _cliente.PutAsJsonAsync("api/Empresa", empresa);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> RegenerarToken(int idEmpresa)
        {
            try
            {
                await _cliente.PutAsJsonAsync($"api/Empresa/UpdateTokenEmpresa", idEmpresa);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteEmpresa(int id)
        {
            try
            {
                await _cliente.DeleteAsync($"api/Empresa/{id}");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //funciones empresas
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

        /// <summary>---------------------------------------------------------------------
        ///  Configuracion de Sizes
        /// </summary>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<List<SizeDTO>> GetSizes()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<SizeDTO>>("api/size");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<SizeDTO> GetSizeById(int idSize)
        {
            try
            {

                var oRta = await _cliente.GetFromJsonAsync<SizeDTO>($"/api/Size/{idSize}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> AddSize(SizeDTO Size)
        {
            try
            {
                Size.Boxes = null;
                Size.Tokens = null;
                await _cliente.PostAsJsonAsync("api/Size", Size);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarSize(SizeDTO Size)
        {
            try
            {
                var response = await _cliente.PutAsJsonAsync("api/size", Size);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSize(int idSize)
        {
            try
            {
                await _cliente.DeleteAsync($"api/Size/{idSize}");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
