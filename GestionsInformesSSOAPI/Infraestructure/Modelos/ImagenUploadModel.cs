using Microsoft.AspNetCore.Http;

namespace GestionsInformesSSOAPI.Infraestructure.Modelos
{
    public class ImagenUploadModel
    {
        public IFormFile File { get; set; }
        public int InformeId { get; set; }
        public string Tipo { get; set; } // Ej: "Rectal", "Piel", "Agua"
    }
}
