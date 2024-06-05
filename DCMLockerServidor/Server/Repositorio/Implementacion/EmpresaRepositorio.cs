using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;



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
                   .AsNoTracking()
                   .ToListAsync();
            }
            catch
            {
                throw new Exception("Hubo un error al buscar las empresas");
            }
        }

        public async Task<Empresa> GetEmpresaById(int idEmpresa)
        {
            try
            {
                Empresa empresa = await _dbContext.Empresas.FindAsync(idEmpresa);
                return empresa;
            }
            catch
            {
                throw new Exception("No se encontró la empresa");
            }
        }

        public async Task<Empresa> GetEmpresaByToken(string tokenEmpresa)
        {
            try
            {
                var empresa = await _dbContext.Empresas.Where(empresa => empresa.TokenEmpresa == tokenEmpresa).FirstOrDefaultAsync();
                return empresa;
            }
            catch
            {
                throw new Exception("No se encontró la empresa");
            }
        }

        public async Task<bool> AddEmpresa(Empresa empresa)
        {
            try
            {
                empresa.TokenEmpresa = GenerarCodigoAlfanumerico();
                _dbContext.Set<Empresa>().Add(empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo agregar la empresa");
            }
        }

        public async Task<bool> EditEmpresa(Empresa empresa)
        {
            try
            {
                var existingEmpresa = await _dbContext.Empresas.FindAsync(empresa.Id);

                if (existingEmpresa == null)
                {
                    throw new Exception("No se encontro la empresa");
                }
                _dbContext.Update(existingEmpresa).CurrentValues.SetValues(empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo editar la empresa");
            }
        }

        public async Task<bool> UpdateTokenEmpresa(int idEmpresa)
        {
            try
            {
                Empresa empresa = await GetEmpresaById(idEmpresa);
                empresa.TokenEmpresa = GenerarCodigoAlfanumerico();
                await EditEmpresa(empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo actualizar el token de la empresa");
            }
        }

        public async Task<bool> DeleteEmpresa(int idEmpresa)
        {
            try
            {
                var lockers = _dbContext.Lockers.Where(b => b.Empresa == idEmpresa);
                foreach (var item in lockers)
                {
                    item.Empresa = null;
                }
                var Empresa = await GetEmpresaById(idEmpresa);
                _dbContext.Empresas.Remove(Empresa);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("No se pudo eliminar la empresa");
            }
        }

        public async Task<bool> IsDcmToken(string tokenEmpresa)
        {
            try
            {
                var empresa = await _dbContext.Empresas.Where(empresa => empresa.TokenEmpresa == tokenEmpresa).FirstOrDefaultAsync();
                var isDCM = empresa.Id == 1;
                return isDCM;
            }
            catch
            {
                throw new Exception("No se");
            }
        }

        //funciones utiles
        static string GenerarCodigoAlfanumerico()
        {
            const string caracteres = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789";
            StringBuilder codigo = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < 21; i++)
            {
                int indice = random.Next(caracteres.Length);
                codigo.Append(caracteres[indice]);
            }

            return codigo.ToString();
        }

    }
}
