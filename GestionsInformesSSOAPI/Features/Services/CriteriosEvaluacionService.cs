// GestionsInformesSSOAPI/Features/Services/CriteriosEvaluacionService.cs
using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class CriteriosEvaluacionService
    {
        private readonly CriteriosEvaluacionRepository _repository;

        public CriteriosEvaluacionService(CriteriosEvaluacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GuardarCriteriosAsync(CriteriosEvaluacionDto dto)
        {
            try
            {
                // Validaciones
                if (dto.IdInforme <= 0)
                    return ApiResponse.BadRequest("ID de informe inválido");

                if (dto.TiempoExposicionHoras <= 0)
                    return ApiResponse.BadRequest("El tiempo de exposición debe ser mayor a 0");

                if (dto.PesoPromedioTrabajadores <= 0)
                    return ApiResponse.BadRequest("El peso promedio debe ser mayor a 0");

                if (dto.TasaMetabolicaValor <= 0)
                    return ApiResponse.BadRequest("La tasa metabólica debe ser mayor a 0");

                var criterios = new CriteriosEvaluacion
                {
                    IdInforme = dto.IdInforme,
                    TiempoExposicionHoras = dto.TiempoExposicionHoras,
                    PesoPromedioTrabajadores = dto.PesoPromedioTrabajadores,
                    TasaMetabolicaValor = dto.TasaMetabolicaValor,
                    TasaMetabolicaDescripcion = dto.TasaMetabolicaDescripcion,
                    AdaptacionFisiologicaTexto = dto.AdaptacionFisiologicaTexto, // NUEVO
                    AjusteRopaId = dto.AjusteRopaId,
                    AjusteRopaValorClo = dto.AjusteRopaValorClo
                };

                await _repository.GuardarAsync(criterios);
                return ApiResponse.Ok("Criterios guardados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Error al guardar criterios: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ObtenerCriteriosPorInformeAsync(int informeId)
        {
            try
            {
                var criterios = await _repository.ObtenerPorInformeAsync(informeId);
                if (criterios == null)
                    return ApiResponse.Ok("No se encontraron criterios para este informe", null);

                var dto = new CriteriosEvaluacionDto
                {
                    IdInforme = criterios.IdInforme,
                    TiempoExposicionHoras = criterios.TiempoExposicionHoras,
                    PesoPromedioTrabajadores = criterios.PesoPromedioTrabajadores,
                    TasaMetabolicaValor = criterios.TasaMetabolicaValor,
                    TasaMetabolicaDescripcion = criterios.TasaMetabolicaDescripcion,
                    AjusteRopaId = criterios.AjusteRopaId,
                    AdaptacionFisiologicaTexto = criterios.AdaptacionFisiologicaTexto, // NUEVO
                    AjusteRopaValorClo = criterios.AjusteRopaValorClo
                };

                return ApiResponse.Ok("Criterios obtenidos exitosamente", dto);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Error al obtener criterios: {ex.Message}");
            }
        }
    }
}