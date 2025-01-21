using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class RopaUtilizadaRepository
    {
        private readonly GestionInformesSSO _context;

        public RopaUtilizadaRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task<List<RopaUtilizada>> GetAllAsync()
        {
            return await _context.RopaUtilizada.ToListAsync();
        }

        public async Task<RopaUtilizada> GetByIdAsync(int id)
        {
            return await _context.RopaUtilizada.FindAsync(id);
        }

        public async Task<RopaUtilizada> AddAsync(RopaUtilizada ropa)
        {
            await _context.RopaUtilizada.AddAsync(ropa);
            await _context.SaveChangesAsync();
            return ropa;
        }

        public async Task<RopaUtilizada> UpdateAsync(RopaUtilizada ropa)
        {
            _context.RopaUtilizada.Update(ropa);
            await _context.SaveChangesAsync();
            return ropa;
        }

        public async Task DeleteAsync(int id)
        {
            var ropa = await GetByIdAsync(id);
            if (ropa != null)
            {
                _context.RopaUtilizada.Remove(ropa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
