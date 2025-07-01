namespace GestionsInformesSSOAPI.Infraestructure.Modelos
{
    public class VistaCondicionesPorArea
    {
        public int id_informe { get; set; }
        public int id_area { get; set; }
        public decimal Tasa_Estimada { get; set; }
        public decimal Bulbo_Humedo { get; set; }
        public decimal Temp_aire { get; set; }
        public decimal Ropa { get; set; }
        public decimal Velocidad_Aire { get; set; }
        public decimal HumedadRelativa { get; set; }
    }
}
