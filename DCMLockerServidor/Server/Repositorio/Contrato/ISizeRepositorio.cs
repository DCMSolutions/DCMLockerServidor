using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;
using DCMLockerServidor.Shared.Models;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface ISizeRepositorio
    {
        //CRUD Sizes
        Task<List<Size>> GetSizes();
        Task<Size> GetSizeById(int id);
        Task<bool> AddSize(Size Size);
        Task<bool> EditSize(Size Size);
        Task<bool> DeleteSize(int idSize);
        Task<bool> AddListSizes(List<Size> Sizes);
    }
}
