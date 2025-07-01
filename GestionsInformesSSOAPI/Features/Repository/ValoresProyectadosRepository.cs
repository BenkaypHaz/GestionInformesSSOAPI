using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class ValoresProyectadosRepository 
    {
        private readonly GestionInformesSSO _context;

        public ValoresProyectadosRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task GuardarAsync(ValoresProyectados_Calor entidad)
        {
            _context.ValoresProyectados_Calor.Add(entidad);
            await _context.SaveChangesAsync();
        }
    }

}
