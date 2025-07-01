namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    public class TitulosGraficos
    {
        public int Id { get; set; }              // Clave primaria, autoincrementable en la BD
        public int IdInforme { get; set; }       // ID del informe relacionado
        public string Titulo { get; set; }       // Título del gráfico
        public int tipo_grafico { get; set; } 
    }

}
