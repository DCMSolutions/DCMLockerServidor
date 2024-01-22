using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;
using DCMLockerServidor.Shared.Models;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface ILockerRepositorio
    {
        //Lockers
        Task<List<Locker>> GetLockers();
        Task<Locker> GetLockerById(int Id);
        Task<Locker> GetLockerByNroSerie(string NroSerie);
        Task<bool> AddLocker(Locker Locker);
        Task<bool> EditLocker(Locker Locker);
        Task<bool> DeleteLocker(Locker Locker);
        //Boxes
        Task<List<Box>> SaveBoxes(ServerStatus status);
        Task<ICollection<Box>> GetBoxes();
        Task<Box> GetBoxById(int IdBox);
        Task<List<Box>> GetBoxesByIdLocker(int IdLocker);
        Task<bool> AddBox(Box Box);
        Task<bool> EditBox(Box Box);
        Task<bool> DeleteBox(Box Box);
        //Sizes
        Task<List<Size>> GetSizes();
        Task<bool> AddSize(Size Size);
        Task<bool> AddSizesLista(List<Size> SizeList);
        Task<bool> EditSize(Size Size);
        Task<bool> DeleteSize(Size Size);
        //Agergar empresa a locker
        Task<bool> AddEmpresaALocker(int idLocker, Empresa empresa);
    }
}
