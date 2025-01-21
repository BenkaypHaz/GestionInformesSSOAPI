using GestionsInformesSSOAPI.Features.Utility;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExcelController : ControllerBase
{
    private readonly ExcelService _service;

    public ExcelController(ExcelService service)
    {
        _service = service;
    }

    [HttpPost("LeerExcel")]
    public IActionResult LeerExcel(IFormFile file, [FromForm] int informeId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse.BadRequest("No se ha proporcionado ningún archivo."));
        }

        if (informeId <= 0)
        {
            return BadRequest(ApiResponse.BadRequest("InformeId inválido."));
        }

        try
        {
            using var stream = file.OpenReadStream();
            var response = _service.LeerArchivoExcel(stream, file.ContentType, informeId);
            return StatusCode(response.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.BadRequest($"Error interno: {ex.Message}"));
        }
    }


}
