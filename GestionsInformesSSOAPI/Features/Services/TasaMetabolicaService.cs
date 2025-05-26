using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Features.Utility;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class TasaMetabolicaService
    {
        private readonly TasaMetabolicaRepository _repositorio;

        public TasaMetabolicaService(TasaMetabolicaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<ApiResponse> ObtenerTodas()
        {
            var datos = await _repositorio.ObtenerTodasAsync();

            return ApiResponse.Ok("Tasas metabolicas obtenidas", datos);

        }
    }

}
