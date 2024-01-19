using DCMLockerServidor.Client.Pages;
using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Controllers;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;

namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class LockerRepositorio : ILockerRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        public LockerRepositorio(DcmlockerContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Locker CRUD
        public async Task<List<Locker>> GetLockers()
        {

            try
            {
                return await _dbContext.Lockers
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.Boxes)
                    .AsNoTracking()
                    .ToListAsync();

            }
            catch
            {
                throw;
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
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<Locker> GetLockerByNroSerie(string NroSerie)
        {
            try
            {
                return await _dbContext.Lockers
                    .Where(locker => locker.NroSerieLocker == NroSerie)
                    .Include(e => e.EmpresaNavigation)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
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
                throw;
            }
        }
        public async Task<bool> EditLocker(Locker Locker)
        {
            try
            {

                //_dbContext.Update(Locker);
                //await _dbContext.SaveChangesAsync();
                //return true;

                var existingLocker = await _dbContext.Lockers.FindAsync(Locker.Id);

                if (existingLocker == null)
                {
                    // Locker with the given ID not found
                    return false;
                }

                // Update properties of the existingLocker entity
                _dbContext.Entry(existingLocker).CurrentValues.SetValues(Locker);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> SaveLockersLista(List<Locker> LockerList)
        {
            try
            {
                // Identify existing lockers in the database
                var existingLockers = await _dbContext.Lockers.ToDictionaryAsync(l => l.Id);

                // Iterate over the new list and update or add entities
                foreach (var newLocker in LockerList)
                {
                    if (existingLockers.TryGetValue(newLocker.Id, out var existingLocker))
                    {
                        // Update existing locker properties
                        _dbContext.Entry(existingLocker).CurrentValues.SetValues(newLocker);
                    }
                    else
                    {
                        // Add new locker to the database
                        _dbContext.Lockers.Add(newLocker);
                    }
                }

                // Identify and remove old lockers that are not in the new list
                var lockerIdsToRemove = existingLockers.Keys.Except(LockerList.Select(l => l.Id));
                foreach (var lockerId in lockerIdsToRemove)
                {
                    var lockerToRemove = existingLockers[lockerId];
                    _dbContext.Lockers.Remove(lockerToRemove);
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteLocker(Locker Locker)
        {
            try
            {
                _dbContext.Lockers.Remove(Locker);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }



        //Boxes CRUD
        public async Task<ICollection<Box>> SaveBoxes(ServerStatus status)
        {
            try
            {

                List<TLockerMapDTO>? boxes = status.Locker;
                var locker = await GetLockerByNroSerie(status.NroSerie);

                //int IdLocker;
                if (locker != null)
                {
                    int IdLocker = locker.Id;
                    var oldBoxes = await GetBoxesByIdLocker(IdLocker);
                    foreach (var box in oldBoxes)
                    {
                        if (box.Enable == true && !boxes.Any(x => x.Id == box.Id))
                        {
                            box.Enable = false;
                            await EditBox(box);
                        }
                    }

                    foreach (var box in boxes)
                    {
                        Console.WriteLine(box.Id);
                        Box newBox = new Box();

                        newBox.Enable = box.Enable;
                        newBox.IdFisico = box.Id;
                        newBox.IdLocker = IdLocker;
                        newBox.Puerta = box.Puerta;
                        newBox.Ocupacion = box.Ocupacion;
                        newBox.Libre = box.Libre;
                        newBox.LastUpdateTime = status.LastUpdateTime;
                        //newBox.IdSize = box.Size;
                        await AddBox(newBox);
                    }
                }


                return await GetBoxes();
            }
            catch
            {
                throw;
            }
        }
        public async Task<ICollection<Box>> GetBoxes()
        {
            try
            {
                return await _dbContext.Boxes
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .ToListAsync();
            }
            catch
            {
                throw;
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
                throw;
            }
        }
        public async Task<List<Box>> GetBoxesByIdLocker(int IdLocker)
        {
            try
            {
                var locker = await _dbContext.Lockers
                    .Where(locker => locker.Id == IdLocker)
                    .Include(b => b.Boxes)
                    .FirstOrDefaultAsync();
                var result = locker.Boxes.ToList();
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddBox(Box Box)
        {
            try
            {
                var boxes = await GetBoxes();
                if (boxes.Any(x => x.IdFisico == Box.IdFisico))
                {
                    foreach (var item in boxes)
                    {
                        if (item.IdFisico == Box.IdFisico)
                        {
                            Box.Id = item.Id;
                            await EditBox(Box);
                        }
                    }
                }
                else
                {

                    _dbContext.Set<Box>().Add(Box);
                    await _dbContext.SaveChangesAsync();

                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<bool> AddBoxesLista(List<Box> BoxList)
        {
            try
            {
                foreach (Box Box in BoxList)
                {
                    _dbContext.Set<Box>().Add(Box);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> EditBox(Box Box)
        {
            try
            {
                var existingBox = await _dbContext.Boxes.FindAsync(Box.Id);

                if (existingBox == null)
                {
                    // Locker with the given ID not found
                    return false;
                }

                // Detach the existing entity from the DbContext
                _dbContext.Entry(existingBox).State = EntityState.Detached;

                // Attach the updated entity and set its state to Modified
                _dbContext.Attach(Box);
                _dbContext.Entry(Box).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> EditBoxesLista(List<Box> BoxList)
        {
            try
            {
                List<Box> BoxListVieja = BoxList.FirstOrDefault().IdLockerNavigation.Boxes.ToList();

                foreach (Box Box in BoxList)
                {
                    //if (!Box.FastEsIgualA(BoxListVieja.Where(BoxViejo => BoxViejo.Id == Box.Id).FirstOrDefault()))
                    //{
                    //    var existingBox = _dbContext.Boxes.SingleOrDefault(b => b.Id == Box.Id);
                    //    Box.LastUpdateTime = DateTime.Now;
                    //    if (existingBox != null)
                    //    {
                    //        _dbContext.Update(Box);
                    //    }
                    //    else
                    //    {
                    //        _dbContext.Add(Box);
                    //    }
                    //}
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteBox(Box Box)
        {
            try
            {
                _dbContext.Boxes.Remove(Box);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        //Sizes CRUD
        public async Task<List<Size>> GetSizes()
        {
            try
            {
                return await _dbContext.Sizes
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddSize(Size Size)
        {
            try
            {
                _dbContext.Set<Size>().Add(Size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddSizesLista(List<Size> SizeList)
        {
            try
            {
                foreach (Size Size in SizeList)
                {
                    _dbContext.Set<Size>().Add(Size);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> EditSize(Size Size)
        {
            try
            {
                var existingSize = _dbContext.Sizes.SingleOrDefault(b => b.Id == Size.Id);
                if (existingSize != null)
                {
                    _dbContext.Update(Size);
                }
                else
                {
                    _dbContext.Add(Size);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteSize(Size Size)
        {
            try
            {
                _dbContext.Sizes.Remove(Size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        //Relacionar locker y empresa
        public async Task<bool> AddEmpresaALocker(int idLocker, Empresa empresa)
        {
            Locker locker = await GetLockerById(idLocker);
            locker.Empresa = empresa.Id;
            bool result = await EditLocker(locker);
            return result;
        }

    }
}
