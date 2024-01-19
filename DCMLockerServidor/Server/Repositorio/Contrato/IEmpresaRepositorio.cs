using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;
using DCMLockerServidor.Shared.Models;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface IEmpresaRepositorio
    {
        //CRUD empresas
        Task<List<Empresa>> GetEmpresas();
        Task<Empresa> GetEmpresaById(int id);
        Task<bool> AddEmpresa(Empresa empresa);
        Task<bool> EditEmpresa(Empresa empresa);
        Task<bool> DeleteEmpresa(Empresa empresa);
    }
}
