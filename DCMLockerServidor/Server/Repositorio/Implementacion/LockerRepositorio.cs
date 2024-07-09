using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using AutoMapper;

namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class LockerRepositorio : ILockerRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISizeRepositorio _size;
        private readonly IEmpresaRepositorio _empresa;
        public LockerRepositorio(DcmlockerContext dbContext, IMapper mapper, ISizeRepositorio size, IEmpresaRepositorio empresa)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _size = size;
            _empresa = empresa;
        }

        //Locker CRUD
        public async Task<List<Locker>> GetLockers()
        {

            try
            {
                var response = await _dbContext.Lockers
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.Boxes)
                    .ThenInclude(e => e.IdSizeNavigation)
                    .AsNoTracking()
                    .ToListAsync();

                return response;
            }
            catch
            {
                throw new Exception("Hubo un error al buscar los lockers");
            }
        }

        public async Task<Locker> GetLockerById(int Id)
        {
            try
            {
                return await _dbContext.Lockers
                    .Where(locker => locker.Id == Id)
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.Boxes)
                    .ThenInclude(e => e.IdSizeNavigation)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception("No se pudo obtener el locker");
            }
        }

        public async Task<Locker> GetLockerByNroSerie(string NroSerie)
        {
            try
            {
                return await _dbContext.Lockers
                    .Where(locker => locker.NroSerieLocker == NroSerie)
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.Boxes)
                    .ThenInclude(e => e.IdSizeNavigation)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception("No se pudo obtener el locker");
            }
        }

        public async Task<List<Locker>> GetLockersByTokenEmpresa(string tokenEmpresa)
        {

            try
            {
                var isDCM = await _empresa.IsDcmToken(tokenEmpresa);

                if (isDCM)
                {
                    var response = await _dbContext.Lockers
                        .Include(e => e.EmpresaNavigation)
                        .Include(e => e.Boxes)
                        .ThenInclude(e => e.IdSizeNavigation)
                        .Include(e => e.Tokens)
                        .AsNoTracking()
                        .ToListAsync();
                    return response;
                }
                else
                {
                    var response = await _dbContext.Lockers
                        .Include(e => e.EmpresaNavigation)
                        .Include(e => e.Boxes)
                        .ThenInclude(e => e.IdSizeNavigation)
                        .Include(e => e.Tokens)
                        .Where(loc => loc.EmpresaNavigation.TokenEmpresa == tokenEmpresa)
                        .AsNoTracking()
                        .ToListAsync();
                    return response;
                }
            }
            catch
            {
                throw new Exception("Hubo un error al obtener los lockers");
            }

        }

        public async Task<bool> AddLocker(Locker Locker)
        {
            try
            {
                _dbContext.Set<Locker>().Add(Locker);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo agregar el locker");
            }
        }

        public async Task<bool> EditLocker(Locker Locker)
        {
            try
            {
                var existingLocker = await _dbContext.Lockers.FindAsync(Locker.Id);

                if (existingLocker == null)
                {
                    // Locker with the given ID not found
                    throw new Exception("No se encontro el locker");
                }

                // Update properties of the existingLocker entity

                _dbContext.Entry(existingLocker).CurrentValues.SetValues(new
                {
                    Locker.LastUpdateTime,
                    Locker.Status,
                    Locker.Empresa,
                });

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new Exception("No se pudo editar el locker");
            }
        }

        public async Task<bool> DeleteLocker(int idLocker)
        {
            try
            {
                var boxes = _dbContext.Boxes.Where(b => b.IdLocker == idLocker);
                foreach (var box in boxes)
                {
                    _dbContext.Boxes.Remove(box);
                }

                var locker = await GetLockerById(idLocker);
                _dbContext.Lockers.Remove(locker);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo eliminar el locker");
            }
        }

        //Boxes CRUD
        public async Task<List<Box>> SaveBoxes(ServerStatus status)
        {
            try
            {
                var locker = await GetLockerByNroSerie(status.NroSerie);

                List<Box> boxesNew = _mapper.Map<List<TLockerMapDTO>, List<Box>>(status.Locker);
                List<Box> boxesOld = new();
                var sizes = await _size.GetSizes();
                if (locker != null)
                {
                    boxesOld = await GetBoxesByIdLocker(locker.Id);
                    foreach (var item in boxesOld)
                    {
                        if (!boxesNew.Any(x => x.IdFisico == item.IdFisico))
                        {

                            await DeleteBox(item);
                        }
                    }
                    foreach (var item in boxesNew)
                    {
                        if (!sizes.Any(x => x.Id == item.IdSize)) item.IdSize = null;
                        item.IdLocker = locker.Id;
                        if (boxesOld != null && boxesOld.Where(x => x.IdFisico == item.IdFisico).ToList().Count > 0)
                        {
                            var Box = boxesOld.FirstOrDefault(x => x.IdFisico == item.IdFisico);

                            if (Box == null)
                            {
                                boxesOld.Add(item);
                            }
                            else
                            {

                                Box.Enable = item.Enable;
                                Box.Ocupacion = item.Ocupacion;
                                Box.Status = item.Status;
                                Box.Box1 = item.Box1;
                                Box.LastUpdateTime = DateTime.Now;
                                Box.Libre = item.Libre;
                                Box.Puerta = item.Puerta;
                                Box.IdSize = item.IdSize;
                            }
                        }
                        else
                        {
                            boxesOld.Add(item);

                        }


                    }

                }
                else
                {
                    foreach (var item in boxesNew)
                    {
                        if (!sizes.Any(x => x.Id == item.IdSize)) item.IdSize = null;
                        boxesOld.Add(item);
                    }
                }

                return boxesOld;
            }
            catch
            {
                throw new Exception("Hubo un error al guardar los boxes");
            }
        }

        public async Task<ICollection<Box>> GetBoxes()
        {
            try
            {
                var response = await _dbContext.Boxes
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .ToListAsync();

                return response;
            }
            catch
            {
                throw new Exception("Hubo un error al obtener los boxes");
            }
        }

        public async Task<Box> GetBoxById(int IdBox)
        {
            try
            {
                return await _dbContext.Boxes
                    .Where(box => box.Id == IdBox)
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw new Exception("No se pudo obtener el box");
            }
        }

        public async Task<List<Box>> GetBoxesByIdLocker(int IdLocker)
        {
            try
            {
                var locker = await _dbContext.Lockers
                    .Where(locker => locker.Id == IdLocker)
                    .Include(b => b.Boxes)
                    .Include(t => t.Tokens)
                    .FirstOrDefaultAsync();

                var result = locker.Boxes.ToList();
                return result;
            }
            catch
            {
                throw new Exception("Hubo un error al obtener los boxes");
            }
        }

        public async Task<bool> AddBox(Box Box)
        {
            try
            {
                _dbContext.Add<Box>(Box);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo agregar el box");
            }
        }

        public async Task<bool> EditBox(Box Box)
        {
            try
            {
                var existingBox = await _dbContext.Boxes.FindAsync(Box.Id);

                if (existingBox == null)
                {
                    throw new Exception("No se encontró el box");
                }

                _dbContext.Entry(existingBox).CurrentValues.SetValues(Box);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new Exception("No se pudo editar el box");
            }
        }

        public async Task<bool> DeleteBox(Box Box)
        {
            try
            {
                _dbContext.Tokens.RemoveRange(Box.Tokens);
                _dbContext.Boxes.Remove(Box);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo eliminar el box");
            }
        }

    }
}
