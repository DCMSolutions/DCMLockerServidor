using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;



namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class EventoRepositorio : IEventoRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        public EventoRepositorio(DcmlockerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Evento>> GetEventos()
        {
            try
            {
                var result = await _dbContext.Eventos
                   .AsNoTracking()
                   .ToListAsync();
                return result;
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los eventos");
            }
        }

        public async Task<List<Evento>> GetEventosByIdLocker(int idLocker)
        {
            try
            {
                return await _dbContext.Eventos
                    .Where(Evento => Evento.IdLocker == idLocker)
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("No se pudieron obtener los eventos del locker");
            }
        }

        public async Task<Evento> GetEventoById(int idEvento)
        {
            try
            {
                Evento evento = await _dbContext.Eventos.FindAsync(idEvento);
                return evento;
            }
            catch
            {
                throw new Exception("No se encontró el evento");
            }
        }

        public async Task<bool> AddEvento(Evento Evento)
        {
            try
            {
                _dbContext.Add(Evento);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo agregar el evento");
            }
        }

        public async Task<bool> EditEvento(Evento Evento)
        {
            try
            {

                var existingEvento = await _dbContext.Eventos.FindAsync(Evento.Id);

                if (existingEvento == null)
                {
                    // evento with the given ID not found
                    return false;
                }

                _dbContext.Update(existingEvento).CurrentValues.SetValues(Evento);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo editar el evento");
            }
        }

        public async Task<bool> DeleteEvento(int idEvento)
        {
            try
            {
                var evento = await GetEventoById(idEvento);
                _dbContext.Eventos.Remove(evento);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo eliminar el evento");
            }
        }
    }
}
