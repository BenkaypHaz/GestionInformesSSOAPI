using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RopaUtilizadaController : ControllerBase
    {
        private readonly RopaUtilizadaService _service;

        public RopaUtilizadaController(RopaUtilizadaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var response = await _service.GetAllAsync();
            return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status404NotFound, response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Add([FromBody] RopaUtilizada ropa)
        {
            var response = await _service.AddAsync(ropa);
            return StatusCode(response.Success ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] RopaUtilizada ropa)
        {
            var response = await _service.UpdateAsync(id, ropa);
            return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }
    }
}
