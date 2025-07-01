namespace GestionsInformesSSOAPI.Features.Controllers
{
    using global::GestionsInformesSSOAPI.Features.Services;
    using global::GestionsInformesSSOAPI.Features.Utility;
    using global::GestionsInformesSSOAPI.Infraestructure.Entities;
    using Microsoft.AspNetCore.Mvc;

    namespace GestionsInformesSSOAPI.Features.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ValoresProyectadosController : ControllerBase
        {
            private readonly ValoresProyectadosService _valoresService;

            public ValoresProyectadosController(ValoresProyectadosService valoresService)
            {
                _valoresService = valoresService;
            }

            [HttpPost]
            public async Task<ActionResult<ApiResponse>> GuardarValores([FromBody] ValoresProyectados_Calor dto)
            {
                if (dto == null)
                {
                    return BadRequest(ApiResponse.BadRequest("Datos inválidos para valores proyectados"));
                }

                var response = await _valoresService.CrearAsync(dto);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
        }
    }

}
