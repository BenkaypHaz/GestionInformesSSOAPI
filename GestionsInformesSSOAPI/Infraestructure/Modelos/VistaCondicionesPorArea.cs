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
        public decimal cy_f { get; set; }
        public string Postura { get; set; }
        public string Ambiente { get; set; }
        public bool Aclimatacion { get; set; }
        public bool Hidratacion { get; set; }
    }
}
