using DCMLockerServidor.Server;
using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;



namespace DCMLockerServidor.Server.Repositorio.Implementacion
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        private readonly DcmlockerContext _dbContext;
        public EmpresaRepositorio(DcmlockerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            try
            {
                return await _dbContext.Empresas
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<Empresa> GetEmpresaById(int id)
        {
            try
            {
                return await _dbContext.Empresas
                    .Where(empresa => empresa.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddEmpresa(Empresa empresa)
        {
            try
            {
                _dbContext.Set<Empresa>().Add(empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> EditEmpresa(Empresa empresa)
        {
            try
            {
                var existingEmpresa = _dbContext.Empresas.SingleOrDefault(b => b.Id == empresa.Id);
                if (existingEmpresa != null)
                {
                    _dbContext.Update(empresa);
                }
                else
                {
                    _dbContext.Add(empresa);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteEmpresa(Empresa empresa)
        {
            try
            {
                _dbContext.Empresas.Remove(empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

    }
}
