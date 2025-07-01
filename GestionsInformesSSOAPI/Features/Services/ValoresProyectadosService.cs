using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class ValoresProyectadosService 
    {
        private readonly ValoresProyectadosRepository _repo;

        public ValoresProyectadosService(ValoresProyectadosRepository repo  )
        {
            _repo = repo;
        }

        public async Task<ApiResponse> CrearAsync(ValoresProyectados_Calor dto)
        {
            try
            {
                await _repo.GuardarAsync(dto);
                return new ApiResponse(true, "Valores proyectados guardados correctamente.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, $"Error al guardar: {ex.Message}");
            }
        }
    }

}
