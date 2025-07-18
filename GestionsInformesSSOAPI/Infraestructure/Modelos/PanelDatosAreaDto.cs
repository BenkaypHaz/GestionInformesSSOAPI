namespace GestionsInformesSSOAPI.Infraestructure.Modelos
{
    public class PanelDatosAreaDto
    {
        public int IdArea { get; set; }
        public decimal WBGT { get; set; }
        public decimal BulboSeco { get; set; }
        public decimal BulboHumedo { get; set; }
        public decimal CuerpoNegro { get; set; }
        public decimal IndiceTermico { get; set; }
        public decimal HumedadPromedio { get; set; }
        public bool GenerarGraficoCampana { get; set; }
        public decimal TasaEstimada { get; set; } // NUEVO CAMPO
        public int? IdRopaUtilizada { get; set; } // NUEVO - ID de la ropa
        public string Postura { get; set; } // NUEVO
        public string Ambiente { get; set; } // NUEVO
    }
}
