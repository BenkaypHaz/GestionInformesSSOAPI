using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class EquiposRepository
    {
        private readonly GestionInformesSSO _context;

        public EquiposRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task<List<Equipos>> GetAllAsync()
        {
            return await _context.Equipos.ToListAsync();
        }

        public async Task<Equipos> GetByIdAsync(int id)
        {
            return await _context.Equipos.FindAsync(id);
        }

        public async Task<Equipos> AddAsync(Equipos equipo)
        {
            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync();
            return equipo;
        }

        public async Task<Equipos> UpdateAsync(Equipos equipo)
        {
            _context.Equipos.Update(equipo);
            await _context.SaveChangesAsync();
            return equipo;
        }

        public async Task DeleteAsync(int id)
        {
            var equipo = await GetByIdAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
