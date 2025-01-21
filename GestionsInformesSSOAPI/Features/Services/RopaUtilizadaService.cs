using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using System;
using System.Threading.Tasks;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class RopaUtilizadaService
    {
        private readonly RopaUtilizadaRepository _repository;

        public RopaUtilizadaService(RopaUtilizadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var ropaList = await _repository.GetAllAsync();
                return ApiResponse.Ok("Lista de ropa obtenida exitosamente", ropaList);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Error al obtener la lista de ropa: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var ropa = await _repository.GetByIdAsync(id);
                if (ropa == null) return ApiResponse.NotFound("Ropa utilizada not found");
                return ApiResponse.Ok("Ropa utilizada fetched successfully", ropa);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> AddAsync(RopaUtilizada ropa)
        {
            try
            {
                var addedRopa = await _repository.AddAsync(ropa);
                return ApiResponse.Ok("Ropa utilizada added successfully", addedRopa);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, RopaUtilizada ropa)
        {
            try
            {
                if (id != ropa.IdRopa)
                    return ApiResponse.BadRequest("Id mismatch");

                var updatedRopa = await _repository.UpdateAsync(ropa);
                return ApiResponse.Ok("Ropa utilizada updated successfully", updatedRopa);
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
                return ApiResponse.Ok("Ropa utilizada deleted successfully", null);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
