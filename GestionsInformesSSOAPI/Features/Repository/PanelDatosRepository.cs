using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class PanelDatosRepository
    {
        private readonly GestionInformesSSO _context;
        private readonly ExcelRepository _excelRepository;

        public PanelDatosRepository(GestionInformesSSO context, ExcelRepository excelRepository)
        {
            _context = context;
            _excelRepository = excelRepository;
        }

        public async Task<bool> GuardarPanelDatosAsync(List<PanelDatos_Calor> panelDatos)
        {
            try
            {
                await _context.Set<PanelDatos_Calor>().AddRangeAsync(panelDatos);
                await _context.SaveChangesAsync();

                // Después de guardar, calcular PMV/PPD para las áreas que lo requieran
                var informeId = panelDatos.First().Id_informe;
                var areasConGraficoCampana = panelDatos
                    .Where(p => p.GenerarGraficoCampana)
                    .Select(p => p.id_area)
                    .ToList();

                if (areasConGraficoCampana.Any())
                {
                    // Llamar al método de cálculo solo para las áreas con GenerarGraficoCampana = true
                    _excelRepository.CalcularYGuardarPMVPorArea(informeId);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<PanelDatos_Calor>> ObtenerPorInformeAsync(int informeId)
        {
            return await _context.Set<PanelDatos_Calor>()
                .Where(p => p.Id_informe == informeId)
                .OrderBy(p => p.id_area)
                .ToListAsync();
        }

        public async Task<bool> EliminarPorInformeAsync(int informeId)
        {
            try
            {
                var datos = await _context.Set<PanelDatos_Calor>()
                    .Where(p => p.Id_informe == informeId)
                    .ToListAsync();

                if (datos.Any())
                {
                    _context.Set<PanelDatos_Calor>().RemoveRange(datos);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
