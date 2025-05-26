namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    public class TasaMetabolica
    {
        public int Id { get; set; }
        public string Actividad { get; set; } = string.Empty;
        public int TasaMetabolicaMin { get; set; }
        public int TasaMetabolicaMax { get; set; }
        public double EficienciaMecanica { get; set; }
    }

}
