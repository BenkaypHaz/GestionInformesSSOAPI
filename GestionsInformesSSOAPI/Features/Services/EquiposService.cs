using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class EquiposService
    {
        private readonly EquiposRepository _repository;

        public EquiposService(EquiposRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var equipos = await _repository.GetAllAsync();
                return ApiResponse.Ok("Equipos fetched successfully", equipos);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var equipo = await _repository.GetByIdAsync(id);
                if (equipo == null) return ApiResponse.NotFound("Equipo not found");
                return ApiResponse.Ok("Equipo fetched successfully", equipo);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> AddAsync(Equipos equipo)
        {
            try
            {
                var addedEquipo = await _repository.AddAsync(equipo);
                return ApiResponse.Ok("Equipo added successfully", addedEquipo);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, Equipos equipo)
        {
            try
            {
                if (id != equipo.IdEquipo)
                    return ApiResponse.BadRequest("Id mismatch");

                var updatedEquipo = await _repository.UpdateAsync(equipo);
                return ApiResponse.Ok("Equipo updated successfully", updatedEquipo);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return ApiResponse.Ok("Equipo deleted successfully", null);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
