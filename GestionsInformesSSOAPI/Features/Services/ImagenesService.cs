using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.Extensions.Logging;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class ImagenesService 
    {
        private readonly ImagenesRepository _repo;
        private readonly InformesCalorRepository _informesRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImagenesService> _logger;

        public ImagenesService(ImagenesRepository repo, InformesCalorRepository informesRepo, IWebHostEnvironment env, ILogger<ImagenesService> logger)
        {
            _repo = repo;
            _informesRepo = informesRepo;
            _env = env;
            _logger = logger;

        }

        public async Task<ApiResponse> GuardarImagen(ImagenUploadModel model)
        {
            try
            {
                if (model.Files == null || model.Files.Length == 0)
                    return new ApiResponse(false, "No se seleccionaron archivos válidos.");

                var folder = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Iniciar contador en función de cuántas imágenes ya existen
                int contadorFotosArea = 1;
                int contadorGraficosHumedad = 1;

                if (model.Tipo == "FotosAreas")
                {
                    var archivosExistentes = Directory.GetFiles(folder, $"{model.InformeId}_*")
                                                      .Where(f => int.TryParse(Path.GetFileNameWithoutExtension(f).Split('_').Last(), out _))
                                                      .ToList();
                    contadorFotosArea = archivosExistentes.Count + 1;
                    _logger.LogInformation($"[📷 FotosAreas] Imágenes existentes detectadas: {archivosExistentes.Count}. Iniciando desde: {contadorFotosArea}");
                }
                else if (model.Tipo == "GraficosHumedad")
                {
                    var archivosExistentes = Directory.GetFiles(folder, $"GraficoHumedad_{model.InformeId}_*")
                                                      .Where(f => {
                                                          var parts = Path.GetFileNameWithoutExtension(f).Split('_');
                                                          return parts.Length >= 3 && int.TryParse(parts.Last(), out _);
                                                      })
                                                      .ToList();
                    contadorGraficosHumedad = archivosExistentes.Count + 1;
                    _logger.LogInformation($"[📊 GraficosHumedad] Gráficos existentes detectados: {archivosExistentes.Count}. Iniciando desde: {contadorGraficosHumedad}");
                }

                foreach (var file in model.Files)
                {
                    string fileName;
                    if (model.Tipo == "FotosAreas")
                    {
                        fileName = $"{model.InformeId}_{contadorFotosArea}{Path.GetExtension(file.FileName)}";
                        contadorFotosArea++;
                    }
                    else if (model.Tipo == "GraficosHumedad")
                    {
                        fileName = $"GraficoHumedad_{model.InformeId}_{contadorGraficosHumedad}{Path.GetExtension(file.FileName)}";
                        contadorGraficosHumedad++;
                    }
                    else
                    {
                        fileName = $"{model.InformeId}_{model.Tipo}{Path.GetExtension(file .FileName)}";
                    }

                    var path = Path.Combine(folder, fileName);

                    // Log de depuración
                    _logger.LogInformation($"[💾 Guardando] Tipo: {model.Tipo}, Archivo: {fileName}");

                    using (var stream = new FileStream(path, FileMode.Create)) 
                    {
                        await file.CopyToAsync(stream);
                    }

                    await _repo.RegistrarRuta(model.InformeId, model.Tipo, $"/Images/{fileName}");
                }

                bool tieneImagenRectal = model.Tipo.Equals("Rectal", StringComparison.OrdinalIgnoreCase);
                if (tieneImagenRectal)
                {
                    await _informesRepo.ActualizarGraficoTempRectalAsync(model.InformeId, tieneImagenRectal);
                }

                return new ApiResponse(true, "Imágenes guardadas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar las imágenes");
                return new ApiResponse(false, "Hubo un error al guardar las imágenes. Intente nuevamente.");
            }
        }

        public class ImagenData
        {
            public string RutaCompleta { get; set; }
            public string ContentType { get; set; }
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

        // Devuelve un objeto con las propiedades definidas
        var resultado = new ApiResponse(true, "Imagen encontrada", new ImagenData
        {
            RutaCompleta = rutaCompleta,
            ContentType = contentType
        });

        return resultado;
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
