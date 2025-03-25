using System.Net.Http.Headers;
using System.Net.Http.Json;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DCMLockerServidor.Client.Cliente
{
    public class Config
    {
        private readonly HttpClient _clienteHttp;
        private readonly IAccessTokenProvider _tokenProvider;

        public Config(HttpClient cliente, IAccessTokenProvider tokenProvider)
        {
            _clienteHttp = cliente;
            _tokenProvider = tokenProvider;
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _clienteHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
            else
            {
                throw new UnauthorizedAccessException("Failed to acquire access token.");
            }

            return _clienteHttp;
        }

        public async Task<List<Locker>> GetListaDeLockers()
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {

                var oRta = await _cliente.GetFromJsonAsync<List<Locker>>($"/api/locker");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Locker> GetLockerById(int idLocker)
        {
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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

        //funciones locker
        public async Task<int> GetDisp(int idSize, string nroSerieLocker, DateTime inicio, DateTime fin)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetAsync($"api/token/disponibilidadLockerBySize/{idSize}/{nroSerieLocker}/{inicio:yyyy-MM-ddTHH:mm:ss}/{fin:yyyy-MM-ddTHH:mm:ss}");

                if (oRta.IsSuccessStatusCode)
                {
                    var content = await oRta.Content.ReadAsStringAsync();
                    if (int.TryParse(content, out int result))
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception("wtf");
                    }
                }
                else
                {
                    throw new Exception($"Request failed with status code: {oRta.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //crud boxes
        public async Task<Box> GetBoxById(int idBox)
        {
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Token>>("api/token");
                oRta.Reverse();
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Token> GetTokenById(int idToken)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<Token>($"/api/token/{idToken}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> AgregarToken(Token token)
        {
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var result = await _cliente.PostAsJsonAsync($"api/token/confirmar", idToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error de conexion");
            }
        }

        public async Task<int> GetTimeDeleter()
        {
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
        public async Task<List<EmpresaUrl>> GetUrlsByIdEmpresa(int idEmpresa)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<EmpresaUrl>>($"api/Empresa/urls/{idEmpresa}");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AgregarEmpresa(Empresa empresa)
        {
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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

        //urls empresas
        public async Task<bool> AddEmpresaUrl(EmpresaUrl empresaUrl)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                await _cliente.PostAsJsonAsync("api/Empresa/url", empresaUrl);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteEmpresaUrl(int idEmpresaUrl)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                await _cliente.DeleteAsync($"api/Empresa/url/{idEmpresaUrl}");
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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
            var _cliente = await GetAuthenticatedClientAsync();
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

        public async Task<bool> AddListSizes(List<Size> sizes)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                await _cliente.PostAsJsonAsync("api/Size/AddListSizes", sizes);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>---------------------------------------------------------------------
        ///  Configuracion de eventos
        /// </summary>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<List<Evento>> GetEventos()
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Evento>>("api/evento");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Evento>> GetEventosByIdLocker(int IdLocker)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Evento>>($"api/evento/{IdLocker}");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AddEvento(Evento Evento)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                await _cliente.PostAsJsonAsync("api/Evento", Evento);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> EditarEvento(Evento Evento)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                var response = await _cliente.PutAsJsonAsync("api/Evento", Evento);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteEvento(int idEvento)
        {
            var _cliente = await GetAuthenticatedClientAsync();
            try
            {
                await _cliente.DeleteAsync($"api/Evento/{idEvento}");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
