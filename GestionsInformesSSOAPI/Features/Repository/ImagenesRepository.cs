namespace GestionsInformesSSOAPI.Features.Repository
{
    public class ImagenesRepository 
    {
        // Inyección de contexto si vas a guardar en DB
        public Task RegistrarRuta(int informeId, string tipo, string path)
        {
            // Simulación: log o guardar en BD
            Console.WriteLine($"📂 Imagen registrada: {tipo} - {path}");
            return Task.CompletedTask;
        }
    }
}
