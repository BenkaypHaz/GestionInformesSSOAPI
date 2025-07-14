namespace GestionsInformesSSOAPI.Infraestructure.Modelos
{
    public class PanelDatosRequest
    {
        public int IdInforme { get; set; }
        public int CantidadAreas { get; set; }
        public bool UsarMismaDatosParaTodasAreas { get; set; }
        public List<PanelDatosAreaDto> DatosPorArea { get; set; }
    }
}
 