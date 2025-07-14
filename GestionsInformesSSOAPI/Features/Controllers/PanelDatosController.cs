using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PanelDatosController : ControllerBase
    {
        private readonly PanelDatosService _service;

        public PanelDatosController(PanelDatosService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> GuardarPanelDatos([FromBody] PanelDatosRequest request)
        {
            var response = await _service.GuardarPanelDatosAsync(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{informeId}")]
        public async Task<ActionResult<ApiResponse>> ObtenerPanelDatos(int informeId)
        {
            var response = await _service.ObtenerPanelDatosPorInformeAsync(informeId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
