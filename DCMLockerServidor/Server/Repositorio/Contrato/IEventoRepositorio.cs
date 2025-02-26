using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DCMLockerServidor.Shared;
using System.Linq.Expressions;
using DCMLockerServidor.Shared.Models;

namespace DCMLockerServidor.Server.Repositorio.Contrato
{
    public interface IEventoRepositorio
    {
        //CRUD evento
        Task<List<Evento>> GetEventos();
        Task<List<Evento>> GetEventosByIdLocker(int idLocker);
        Task<Evento> GetEventoById(int idEvento);
        Task<bool> AddEvento(Evento evento);
        Task<bool> EditEvento(Evento evento);
        Task<bool> DeleteEvento(int idEvento);

    }
}
