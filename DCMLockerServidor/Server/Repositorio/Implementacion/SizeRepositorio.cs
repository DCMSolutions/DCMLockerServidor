using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;



namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class SizeRepositorio : ISizeRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        public SizeRepositorio(DcmlockerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Size>> GetSizes()
        {
            try
            {
                var result = await _dbContext.Sizes
                   .AsNoTracking()
                   .OrderBy(x => x.Alto * x.Ancho)
                   .ToListAsync();
                return result;
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los tamaños");
            }
        }

        public async Task<Size> GetSizeById(int id)
        {
            try
            {
                return await _dbContext.Sizes
                    .Where(Size => Size.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception("No se pudo obtener el tamaño");
            }
        }

        public async Task<bool> AddSize(Size Size)
        {
            try
            {
                _dbContext.Add(Size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo agregar el tamaño");
            }
        }

        public async Task<bool> EditSize(Size Size)
        {
            try
            {

                var existingSize = await _dbContext.Sizes.FindAsync(Size.Id);

                if (existingSize == null)
                {
                    // Locker with the given ID not found
                    return false;
                }

                _dbContext.Update(existingSize).CurrentValues.SetValues(Size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo editar el tamaño");
            }
        }

        public async Task<bool> DeleteSize(int idSize)
        {
            try
            {
                var boxes = _dbContext.Boxes.Where(b => b.IdSize == idSize);
                foreach (var item in boxes)
                {
                    item.IdSize = null;
                }

                var tokens = _dbContext.Tokens.Where(t => t.IdSize == idSize);
                foreach (var token in tokens)
                {
                    _dbContext.Tokens.Remove(token);
                }

                var size = await GetSizeById(idSize);
                _dbContext.Sizes.Remove(size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo eliminar el tamaño");
            }
        }

        public async Task<bool> AddListSizes(List<Size> Sizes)
        {
            try
            {
                foreach (var size in Sizes) _dbContext.Add(size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudieron resetear los tamaños");
            }
        }

    }
}
