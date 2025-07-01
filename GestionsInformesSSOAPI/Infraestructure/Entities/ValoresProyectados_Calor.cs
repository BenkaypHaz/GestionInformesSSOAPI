namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    public class ValoresProyectados_Calor
    {
        public int Id { get; set; }
        public int IdInforme { get; set; }
        public decimal TemperaturaRectalFinal { get; set; }
        public bool ExcesoTempCorporal38C { get; set; }
        public bool ExcesoDeshidratacion2Porciento { get; set; }
        public decimal TasaMediaSudoracion { get; set; }
        public decimal PerdidaTotalAgua { get; set; }
    }
}
