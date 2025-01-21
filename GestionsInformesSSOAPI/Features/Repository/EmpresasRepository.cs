using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Features.Repositories
{
    public class EmpresasRepository
    {
        private readonly GestionInformesSSO _context;

        public EmpresasRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task<List<Empresas>> GetAllEmpresasAsync()
        {
            return await _context.Empresas.ToListAsync();
        }

        public async Task<Empresas> GetEmpresaByIdAsync(int id)
        {
            return await _context.Empresas.FirstOrDefaultAsync(e => e.IdEmpresa == id);
        }

        public async Task<Empresas> AddEmpresaAsync(Empresas empresa)
        {
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            return empresa;
        }

        public async Task<Empresas> UpdateEmpresaAsync(Empresas empresa)
        {
            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();
            return empresa;
        }

        public async Task DeleteEmpresaAsync(int id)
        {
            var empresa = await GetEmpresaByIdAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
