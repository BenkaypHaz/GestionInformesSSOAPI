using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EstrésTérmicoController : ControllerBase
{
    private readonly ICalculoEstrésTérmicoService _service;

    public EstrésTérmicoController(ICalculoEstrésTérmicoService service)
    {
        _service = service;
    }

    [HttpGet("evaluar-riesgo")]
    public IActionResult EvaluarRiesgo()
    {
        var resultado = _service.EvaluarRiesgo();
        return Ok(resultado);
    }
}
