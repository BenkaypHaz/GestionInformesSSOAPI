using GestionsInformesSSOAPI.Features.Repositories;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class EmpresasService
    {
        private readonly EmpresasRepository _repository;

        public EmpresasService(EmpresasRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GetAllEmpresasAsync()
        {
            try
            {
                var empresas = await _repository.GetAllEmpresasAsync();
                return ApiResponse.Ok("Listado de empresas generado correctamente.", empresas);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Ocurrio un error: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetEmpresaByIdAsync(int id)
        {
            try
            {
                var empresa = await _repository.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return ApiResponse.NotFound("Empresa not found.");
                }
                return ApiResponse.Ok("Empresa retrieved successfully.", empresa);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> AddEmpresaAsync(Empresas empresa)
        {
            try
            {
                var createdEmpresa = await _repository.AddEmpresaAsync(empresa);
                return ApiResponse.Ok("Empresa added successfully.", createdEmpresa);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateEmpresaAsync(Empresas empresa)
        {
            try
            {
                var updatedEmpresa = await _repository.UpdateEmpresaAsync(empresa);
                return ApiResponse.Ok("Empresa updated successfully.", updatedEmpresa);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteEmpresaAsync(int id)
        {
            try
            {
                await _repository.DeleteEmpresaAsync(id);
                return ApiResponse.Ok("Empresa deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
