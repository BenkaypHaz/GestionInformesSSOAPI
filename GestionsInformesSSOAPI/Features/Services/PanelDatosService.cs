using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;

namespace GestionsInformesSSOAPI.Features.Services
{
    public class PanelDatosService
    {
        private readonly PanelDatosRepository _repository;

        public PanelDatosService(PanelDatosRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GuardarPanelDatosAsync(PanelDatosRequest request)
        {
            try
            {
                // Validaciones
                if (request.IdInforme <= 0)
                    return ApiResponse.BadRequest("ID de informe inválido");

                if (request.CantidadAreas <= 0)
                    return ApiResponse.BadRequest("La cantidad de áreas debe ser mayor a 0");

                // Eliminar datos anteriores si existen
                await _repository.EliminarPorInformeAsync(request.IdInforme);

                var panelDatosAGuardar = new List<PanelDatos_Calor>();

                if (request.UsarMismaDatosParaTodasAreas)
                {
                    // Usar los mismos datos para todas las áreas
                    if (request.DatosPorArea == null || !request.DatosPorArea.Any())
                        return ApiResponse.BadRequest("Debe proporcionar al menos un conjunto de datos");

                    var datosBase = request.DatosPorArea.First();

                    for (int i = 1; i <= request.CantidadAreas; i++)
                    {
                        panelDatosAGuardar.Add(new PanelDatos_Calor
                        {
                            Id_informe = request.IdInforme,
                            id_area = i,
                            WBGT = datosBase.WBGT,
                            BulboSeco = datosBase.BulboSeco,
                            BulboHumedo = datosBase.BulboHumedo,
                            CuerpoNegro = datosBase.CuerpoNegro,
                            IndiceTermico = datosBase.IndiceTermico,
                            HumedadPromedio = datosBase.HumedadPromedio,
                            GenerarGraficoCampana = datosBase.GenerarGraficoCampana,
                            PMV = 0, // Se calculará después si GenerarGraficoCampana = true
                            PPD = 0,  // Se calculará después si GenerarGraficoCampana = true
                            Tasa_Estimada = datosBase.TasaEstimada, // NUEVO CAMPO
                            IdRopaUtilizada = datosBase.IdRopaUtilizada, // NUEVO
                            Postura = datosBase.Postura, // NUEVO
                            Ambiente = datosBase.Ambiente // NUEVO
                        });
                    }
                }
                else
                {
                    // Usar datos diferentes para cada área
                    if (request.DatosPorArea == null || request.DatosPorArea.Count != request.CantidadAreas)
                        return ApiResponse.BadRequest($"Debe proporcionar datos para las {request.CantidadAreas} áreas");

                    foreach (var datosArea in request.DatosPorArea)
                    {
                        panelDatosAGuardar.Add(new PanelDatos_Calor
                        {
                            Id_informe = request.IdInforme,
                            id_area = datosArea.IdArea,
                            WBGT = datosArea.WBGT,
                            BulboSeco = datosArea.BulboSeco,
                            BulboHumedo = datosArea.BulboHumedo,
                            CuerpoNegro = datosArea.CuerpoNegro,
                            IndiceTermico = datosArea.IndiceTermico,
                            HumedadPromedio = datosArea.HumedadPromedio,
                            GenerarGraficoCampana = datosArea.GenerarGraficoCampana,
                            PMV = 0,
                            PPD = 0,
                            Tasa_Estimada = datosArea.TasaEstimada, // NUEVO CAMPO
                            IdRopaUtilizada = datosArea.IdRopaUtilizada, // NUEVO
                            Postura = datosArea.Postura, // NUEVO
                            Ambiente = datosArea.Ambiente // NUEVO
                        });
                    }
                }

                var resultado = await _repository.GuardarPanelDatosAsync(panelDatosAGuardar);

                if (resultado)
                    return ApiResponse.Ok("Panel de datos guardado exitosamente");
                else
                    return ApiResponse.BadRequest("Error al guardar el panel de datos");
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ObtenerPanelDatosPorInformeAsync(int informeId)
        {
            try
            {
                var datos = await _repository.ObtenerPorInformeAsync(informeId);
                return ApiResponse.Ok("Datos obtenidos exitosamente", datos);
            }
            catch (Exception ex)
            {
                return ApiResponse.BadRequest($"Error al obtener datos: {ex.Message}");
            }
        }
    }
}
