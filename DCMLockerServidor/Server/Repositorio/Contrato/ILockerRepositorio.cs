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
        Task<List<Locker>> GetLockersByTokenEmpresa(string tokenEmpresa);
        Task<bool> AddLocker(Locker Locker);
        Task<bool> EditLocker(Locker Locker);
        Task<bool> DeleteLocker(int idLocker);

        //Boxes
        Task<List<Box>> SaveBoxes(ServerStatus status);
        Task<ICollection<Box>> GetBoxes();
        Task<Box> GetBoxById(int IdBox);
        Task<List<Box>> GetBoxesByIdLocker(int IdLocker);
        Task<bool> AddBox(Box Box);
        Task<bool> EditBox(Box Box);
        Task<bool> DeleteBox(Box Box);
        
    }
}
