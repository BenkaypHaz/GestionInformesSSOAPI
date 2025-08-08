using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Http.HttpResults;

public class InformesCalorService
{
    private readonly InformesCalorRepository _repository;

    public InformesCalorService(InformesCalorRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> CrearInformeAsync(InformeRequest request)
    {
        try
        {
            var informe = new InformesCalor
            {
                IdEmpresa = request.IdEmpresa,
                EsAfiliada = request.EsAfiliada,
                FechaInicia = request.FechaInicia,
                FechaFinaliza = request.FechaFinaliza,
                IdTecnico = request.IdTecnico,
                Peso_Promedio = request.Peso_Promedio,
                Aclimatacion = request.Aclimatacion, // NUEVO
                Hidratacion = request.Hidratacion, // NUEVO
                Conclusiones = request.Conclusiones, // NUEVO
                GraficoTempRectal = request.GraficoTempRectal
            };

            // Guardar informe y obtener su ID generado
            var informeId = await _repository.GuardarInformeAsync(informe);

            // Crear y guardar equipos relacionados
            var equipos = request.EquiposUtilizados.Select(eq => new EquipoUtilizadoInforme
            {
                IdInforme = informeId,
                IdEquipo = eq.IdEquipo
            }).ToList();
            await _repository.GuardarEquiposAsync(equipos);

            // Crear y guardar datos de clima relacionados
            var tablasClima = request.DiasEvaluacion.Select(dia => new TablasClimaInforme
            {
                IdInforme = informeId,
                HumedadRelativa = Convert.ToDecimal(dia.HumedadRelativa),
                TemperaturaMaxima = Convert.ToDecimal(dia.TemperaturaMaxima),
                TemperaturaMinima = Convert.ToDecimal(dia.TemperaturaMinima),
                Nubosidad = dia.Nubosidad,
                HoraInicio = dia.HoraInicio,          
                HoraFinalizacion = dia.HoraFinalizacion 
            }).ToList();
            await _repository.GuardarClimaAsync(tablasClima);
            if (request.TitulosGraficos != null && request.TitulosGraficos.Any())
            {
                var titulos = request.TitulosGraficos.Select(t => new TitulosGraficos
                {
                    IdInforme = informeId,
                    Titulo = t.Titulo,
                    tipo_grafico = t.tipo_grafico
                }).ToList();

                await _repository.GuardarTitulosAsync(titulos);
            }

            // Retornar respuesta con el ID del informe creado
            return ApiResponse.Ok($"Informe creado correctamente. ID: {informeId}", informeId);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error al crear el informe: {ex.Message}");
        }
    }

    public async Task<ApiResponse> ActualizarConclusionesAsync(ActualizarConclusionesDTO dto)
    {
        try
        {
            var actualizado = await _repository.ActualizarConclusionesAsync(dto.IdInforme, dto.Conclusiones);

            if (!actualizado)
                return ApiResponse.BadRequest("Informe no encontrado");

            return ApiResponse.Ok("Conclusiones actualizadas exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error al actualizar conclusiones: {ex.Message}");
        }
    }

    public async Task<ApiResponse> Prueba(IFormFile file,String xd)
    {
        return ApiResponse.Ok("xd");
    }

}
