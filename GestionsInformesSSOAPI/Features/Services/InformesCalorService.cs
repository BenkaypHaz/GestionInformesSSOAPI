using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
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
                Tasa_Estimada = request.Tasa_Estimada,
                Peso_Promedio = request.Peso_Promedio,
                idRopaUtilizada = request.idRopaUtilizada
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
                TemperaturaMinima = Convert.ToDecimal(dia.TemperaturaMinima)
            }).ToList();
            await _repository.GuardarClimaAsync(tablasClima);

            // Retornar respuesta con el ID del informe creado
            return ApiResponse.Ok($"Informe creado correctamente. ID: {informeId}", informeId);
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error al crear el informe: {ex.Message}");
        }
    }


    public async Task<ApiResponse> Prueba(IFormFile file,String xd)
    {
        return ApiResponse.Ok("xd");
    }

}
