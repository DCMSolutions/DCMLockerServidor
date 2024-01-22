using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace DCMLockerServidor.Client.Cliente
{
    public class Config2
    {
        private readonly HttpClient _cliente;
        private readonly NavigationManager _nav;
        public Config2(HttpClient cliente, NavigationManager nav)
        {
            _cliente = cliente;
            _nav = nav; 
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
                await _cliente.PostAsJsonAsync("api/locker/addLocker", locker);
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
                var response = await _cliente.PutAsJsonAsync("api/locker/editLocker", locker);
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
                await _cliente.PostAsJsonAsync("api/token/addToken", token);
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
                await _cliente.PutAsJsonAsync("api/token/editToken", token);
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

        //crud empresas
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

        /// <summary>---------------------------------------------------------------------
        ///  Configuracion de Sizes
        /// </summary>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<List<Size>> GetSizes()
        {
            try
            {
                var oRta = await _cliente.GetFromJsonAsync<List<Size>>("api/size");
                return oRta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Size> GetSizeById(int idSize)
        {
            try
            {

                var oRta = await _cliente.GetFromJsonAsync<Size>($"/api/Size/{idSize}");
                return oRta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> AddSize(Size Size)
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
        public async Task<bool> EditarSize(Size Size)
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
