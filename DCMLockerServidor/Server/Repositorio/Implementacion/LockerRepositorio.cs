using AutoMapper;
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
        private readonly IMapper _mapper;
        public LockerRepositorio(DcmlockerContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

                var existingLocker = await _dbContext.Lockers.FindAsync(Locker.Id);

                if (existingLocker == null)
                {
                    // Locker with the given ID not found
                    return false;
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
        public async Task<List<Box>> SaveBoxes(ServerStatus status)
        {
            try
            {
                var locker = await GetLockerByNroSerie(status.NroSerie);

                List<Box> listaBox = _mapper.Map<List<TLockerMapDTO>, List<Box>>(status.Locker);
                List<Box> Boxes = new();
                if (locker != null)
                {
                    Boxes = await GetBoxesByIdLocker(locker.Id);

                    foreach (var item in listaBox)
                    {
                        item.IdLocker = locker.Id;
                        if (Boxes != null && Boxes.Where(x => x.IdFisico == item.IdFisico).ToList().Count > 0)
                        {
                            var Box = Boxes.FirstOrDefault(x => x.IdFisico == item.IdFisico);

                            if (Box == null)
                            {
                                Boxes.Add(item);
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

                            }
                        }
                        else
                        {
                            Boxes.Add(item);

                        }


                    }

                }
                else
                {
                    foreach (var item in listaBox)
                    {
                        Boxes.Add(item);
                    }
                }

                return Boxes;
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
                var response = await _dbContext.Boxes
                    .Include(e => e.IdLockerNavigation)
                    .Include(e => e.IdSizeNavigation)
                    .ToListAsync();

                return response;
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

                _dbContext.Add<Box>(Box);
                await _dbContext.SaveChangesAsync();



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> EditBox(Box Box)
        {
            try
            {
                //var existingBox = await _dbContext.Boxes.FindAsync(Box.Id);

                //if (existingBox == null)
                //{
                //    // Box with the given ID not found
                //    return false;
                //}

                //// Update the properties of the existingBox with the values from updatedBox
                //_dbContext.Entry(existingBox).CurrentValues.SetValues(Box);

                //await _dbContext.SaveChangesAsync();

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
