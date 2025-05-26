using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.Extensions.Logging;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class ImagenesService 
    {
        private readonly ImagenesRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImagenesService> _logger;

        public ImagenesService(ImagenesRepository repo, IWebHostEnvironment env, ILogger<ImagenesService> logger)
        {
            _repo = repo;
            _env = env;
            _logger = logger;

        }

        public async Task<ApiResponse> GuardarImagen(ImagenUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return new ApiResponse(false, "Archivo no válido");

            var folder = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{model.InformeId}_{model.Tipo}{Path.GetExtension(model.File.FileName)}";
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Guardar metadata en BD si aplica
            await _repo.RegistrarRuta(model.InformeId, model.Tipo, $"/Images/{fileName}");

            return new ApiResponse(true, "Imagen guardada correctamente");
        }

        public async Task<ApiResponse> ObtenerImagenPorNombre(string nombreImagen)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreImagen))
                    return new ApiResponse(false, "Nombre de imagen requerido");

                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var imagesFolder = Path.Combine(webRootPath, "Images");
                var rutaCompleta = Path.Combine(imagesFolder, nombreImagen);

                _logger.LogInformation("Buscando imagen en: {Ruta}", rutaCompleta);

                if (!File.Exists(rutaCompleta))
                {
                    _logger.LogWarning("Archivo no encontrado: {Ruta}", rutaCompleta);
                    return new ApiResponse(false, "Imagen no encontrada");
                }

                var contentType = ObtenerContentType(nombreImagen);
                var fileInfo = new FileInfo(rutaCompleta);

                var resultado = new
                {
                    NombreArchivo = nombreImagen,
                    RutaCompleta = rutaCompleta,
                    ContentType = contentType,
                    Tamaño = fileInfo.Length,
                    FechaModificacion = fileInfo.LastWriteTime
                };

                return new ApiResponse(true, "Imagen encontrada", resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener imagen por nombre: {NombreImagen}", nombreImagen);
                return new ApiResponse(false, $"Error interno: {ex.Message}");
            }
        }

       
       

       

        // Método helper para obtener Content-Type
        private string ObtenerContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }

    }
}
