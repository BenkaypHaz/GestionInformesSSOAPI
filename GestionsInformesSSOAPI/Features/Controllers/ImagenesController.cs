using Microsoft.AspNetCore.Mvc;
using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Infraestructure.Modelos;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly ImagenesService _imagenesService;
        private readonly ILogger<ImagenesController> _logger;

        public ImagenesController(ImagenesService imagenesService, ILogger<ImagenesController> logger)
        {
            _imagenesService = imagenesService;
            _logger = logger;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> SubirImagen([FromForm] ImagenUploadModel model)
        {
            var resultado = await _imagenesService.GuardarImagen(model);
            return Ok(resultado);
        }

        // Obtener imagen por nombre - Modificado para mostrar en navegador
        [HttpGet("GetByName/{nombreImagen}")]
        public async Task<IActionResult> ObtenerImagenPorNombre(string nombreImagen)
        {
            try
            {
                _logger.LogInformation("Solicitando imagen: {NombreImagen}", nombreImagen);
                var resultado = await _imagenesService.ObtenerImagenPorNombre(nombreImagen);

                if (!resultado.Success)
                {
                    _logger.LogWarning("Imagen no encontrada: {NombreImagen}", nombreImagen);
                    return NotFound(new { success = false, message = resultado.Message });
                }

                var imagenInfo = resultado.Data as dynamic;

                // Leer el archivo como bytes
                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagenInfo.RutaCompleta);

                // Determinar el Content-Type correcto basado en la extensión
                var contentType = DeterminarContentType(nombreImagen);

                // Establecer headers para mostrar inline (no descargar)
                Response.Headers.Add("Content-Disposition", "inline");

                // Devolver el archivo como stream para mostrar en navegador
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener imagen: {NombreImagen}", nombreImagen);
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        private string DeterminarContentType(string nombreArchivo)
        {
            var extension = Path.GetExtension(nombreArchivo).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                _ => "image/jpeg" // Default para imágenes
            };
        }
    }
}