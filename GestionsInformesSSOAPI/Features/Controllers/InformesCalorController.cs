using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InformesCalorController : ControllerBase
{
    private readonly InformesCalorService _service;

    public InformesCalorController(InformesCalorService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CrearInforme([FromBody] InformeRequest request)
    {
        var response = await _service.CrearInformeAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut("{id}/conclusiones")]
    public async Task<ActionResult<ApiResponse>> ActualizarConclusiones(int id, [FromBody] ActualizarConclusionesDTO dto)
    {
        dto.IdInforme = id;
        var response = await _service.ActualizarConclusionesAsync(dto);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

}




