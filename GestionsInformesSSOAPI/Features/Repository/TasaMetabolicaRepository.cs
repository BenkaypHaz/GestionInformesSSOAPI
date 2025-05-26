using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class TasaMetabolicaRepository
    {
        private readonly GestionInformesSSO _dbContext;

        public TasaMetabolicaRepository(GestionInformesSSO dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TasaMetabolica>> ObtenerTodasAsync()
        {
            return await _dbContext.Set<TasaMetabolica>().ToListAsync();
        }
    }

}
