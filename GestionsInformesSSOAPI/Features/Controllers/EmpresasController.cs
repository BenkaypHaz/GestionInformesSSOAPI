using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly EmpresasService _service;

        public EmpresasController(EmpresasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllEmpresas()
        {
            var response = await _service.GetAllEmpresasAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEmpresaById(int id)
        {
            var response = await _service.GetEmpresaByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddEmpresa([FromBody] Empresas empresa)
        {
            var response = await _service.AddEmpresaAsync(empresa);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetEmpresaById), new { id = ((Empresas)response.Data).IdEmpresa }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateEmpresa(int id, [FromBody] Empresas empresa)
        {
            if (id != empresa.IdEmpresa)
            {
                return BadRequest(ApiResponse.BadRequest("ID mismatch."));
            }

            var response = await _service.UpdateEmpresaAsync(empresa);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteEmpresa(int id)
        {
            var response = await _service.DeleteEmpresaAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
