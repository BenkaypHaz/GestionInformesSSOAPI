using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
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


}




