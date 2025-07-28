// GestionsInformesSSOAPI/Features/Repository/CriteriosEvaluacionRepository.cs
using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class CriteriosEvaluacionRepository
    {
        private readonly GestionInformesSSO _context;

        public CriteriosEvaluacionRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task<CriteriosEvaluacion> GuardarAsync(CriteriosEvaluacion criterios)
        {
            // Verificar si ya existen criterios para este informe
            var existente = await _context.Set<CriteriosEvaluacion>()
                .FirstOrDefaultAsync(c => c.IdInforme == criterios.IdInforme);

            if (existente != null)
            {
                // Actualizar
                existente.TiempoExposicionHoras = criterios.TiempoExposicionHoras;
                existente.PesoPromedioTrabajadores = criterios.PesoPromedioTrabajadores;
                existente.TasaMetabolicaValor = criterios.TasaMetabolicaValor;
                existente.TasaMetabolicaDescripcion = criterios.TasaMetabolicaDescripcion;
                existente.AjusteRopaId = criterios.AjusteRopaId;
                existente.AdaptacionFisiologicaTexto = criterios.AdaptacionFisiologicaTexto; 
                existente.AjusteRopaValorClo = criterios.AjusteRopaValorClo;

                _context.Update(existente);
            }
            else
            {
                // Crear nuevo
                await _context.Set<CriteriosEvaluacion>().AddAsync(criterios);
            }

            await _context.SaveChangesAsync();
            return criterios;
        }

        public async Task<CriteriosEvaluacion> ObtenerPorInformeAsync(int informeId)
        {
            return await _context.Set<CriteriosEvaluacion>()
                .FirstOrDefaultAsync(c => c.IdInforme == informeId);
        }
    }
}