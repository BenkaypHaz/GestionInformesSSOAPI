using GestionsInformesSSOAPI.Features.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasaMetabolicaController : ControllerBase
    {
        private readonly TasaMetabolicaService _servicio;

        public TasaMetabolicaController(TasaMetabolicaService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _servicio.ObtenerTodas();
            return Ok(response);
        }
    }
}
