// GestionsInformesSSOAPI/Features/Controllers/CriteriosEvaluacionController.cs
using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriteriosEvaluacionController : ControllerBase
    {
        private readonly CriteriosEvaluacionService _service;

        public CriteriosEvaluacionController(CriteriosEvaluacionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> GuardarCriterios([FromBody] CriteriosEvaluacionDto dto)
        {
            var response = await _service.GuardarCriteriosAsync(dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{informeId}")]
        public async Task<ActionResult<ApiResponse>> ObtenerCriterios(int informeId)
        {
            var response = await _service.ObtenerCriteriosPorInformeAsync(informeId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}