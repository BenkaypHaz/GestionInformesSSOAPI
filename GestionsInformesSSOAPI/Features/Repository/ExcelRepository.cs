using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.DataBases;

public class ExcelRepository
{
    private readonly GestionInformesSSO _dbContext;

    public ExcelRepository(GestionInformesSSO dbContext)
    {
        _dbContext = dbContext;
    }

    public Dictionary<string, List<Dictionary<string, object>>> LeerTodasLasHojas(Stream excelStream, string contentType)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        IExcelDataReader reader;

        if (contentType == "application/vnd.ms-excel") // .xls
        {
            reader = ExcelReaderFactory.CreateBinaryReader(excelStream);
        }
        else if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") // .xlsx
        {
            reader = ExcelReaderFactory.CreateOpenXmlReader(excelStream);
        }
        else
        {
            throw new Exception("Solo se admiten archivos Excel en formato .xls o .xlsx.");
        }

        var dataSet = reader.AsDataSet(); // Convertimos a DataSet para acceder a todas las hojas
        var resultado = new Dictionary<string, List<Dictionary<string, object>>>();

        foreach (DataTable table in dataSet.Tables)
        {
            var filas = new List<Dictionary<string, object>>();
            var encabezados = new List<string>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var fila = new Dictionary<string, object>();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (i == 0) // Obtener encabezados
                    {
                        var columnName = table.Rows[i][j]?.ToString() ?? $"Columna{j + 1}";
                        encabezados.Add(columnName);
                    }
                    else // Obtener datos
                    {
                        fila[encabezados[j]] = table.Rows[i][j] ?? "N/A";
                    }
                }
                if (i > 0) // Agregar solo filas de datos, no encabezados
                {
                    filas.Add(fila);
                }
            }

            resultado[table.TableName] = filas; // Guardar los datos de la hoja
        }

        reader.Close();
        return resultado;
    }

    public void GuardarDatos(Dictionary<string, List<Dictionary<string, object>>> datosProcesados, int informeId)
    {
        try
        {
            // --- REMOVIDO: Panel de Datos ---
            // Ya no procesamos "Panel de datos de resumen" porque ahora se captura desde el frontend

            // --- Gráfica de Datos ---
            if (datosProcesados.ContainsKey("Gráfica de datos de registro"))
            {
                var filas = datosProcesados["Gráfica de datos de registro"];
                var graficoDatos = new List<GraficoDatos_Calor>();
                int graficoAreaId = 1;

                foreach (var fila in filas)
                {
                    bool filaVacia = fila.Values.All(v => string.IsNullOrWhiteSpace(v?.ToString()));
                    if (filaVacia)
                    {
                        graficoAreaId++;
                        continue;
                    }

                    if (!DateTime.TryParse(fila["Timestamp"]?.ToString(), out DateTime fecha))
                        continue;

                    graficoDatos.Add(new GraficoDatos_Calor
                    {
                        Fecha = fecha,
                        DryBulb = ParseDecimal(fila, "DryBulb"),
                        Humidity = ParseDecimal(fila, "Humidity"),
                        Id_informe = informeId,
                        id_area = graficoAreaId
                    });
                }

                if (graficoDatos.Any())
                    _dbContext.Set<GraficoDatos_Calor>().AddRange(graficoDatos);
            }

            // --- MS (DatosTiempo_Calor) ---
            if (datosProcesados.ContainsKey("MS"))
            {
                var filas = datosProcesados["MS"];
                var datosTiempo = new List<DatosTiempo_Calor>();
                int msAreaId = 1;

                foreach (var fila in filas)
                {
                    bool filaVacia = fila.Values.All(v => string.IsNullOrWhiteSpace(v?.ToString()));
                    if (filaVacia)
                    {
                        msAreaId++;
                        continue;
                    }

                    if (!DateTime.TryParse(fila["Tiempo"]?.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime fechaHora))
                    {
                        Console.WriteLine($"No se pudo parsear el tiempo: {fila["Tiempo"]}");
                        continue;
                    }

                    TimeSpan tiempo = fechaHora.TimeOfDay;
                    decimal velocidad = ParseDecimal(fila, "m/s");

                    datosTiempo.Add(new DatosTiempo_Calor
                    {
                        Tiempo = tiempo,
                        MS = velocidad,
                        Id_informe = informeId,
                        id_area = msAreaId
                    });
                }

                if (datosTiempo.Any())
                    _dbContext.Set<DatosTiempo_Calor>().AddRange(datosTiempo);
            }

            _dbContext.SaveChanges();

            // NO llamamos a CalcularYGuardarPMVPorArea aquí porque ahora se hace desde el controller

        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar los datos desde el archivo Excel.", ex);
        }
    }


    public void CalcularYGuardarPMVPorArea(int informeId)
    {
        try
        {
            var condicionesPorArea = _dbContext.VistaCondicionesPorArea
                                               .Where(v => v.id_informe == informeId)
                                               .ToList();

            if (!condicionesPorArea.Any())
            {
                Console.WriteLine($"[DEBUG] Advertencia: No se encontraron datos en VistaCondicionesPorArea para el informeId: {informeId}");
                return;
            }

            // Bucle a través de cada área para calcular y actualizar el PMV
            foreach (var condicion in condicionesPorArea)
            {
                // --- INICIO DE DEPURACIÓN PARA UN ÁREA ---
                Console.WriteLine($"\n--- Depurando Área ID: {condicion.id_area} para Informe ID: {informeId} ---");

                // <-- DEBUG 1: Muestra los datos tal como vienen de la base de datos (en formato decimal)
                Console.WriteLine($"[DEBUG 1] Valores CRUDOS leídos de la DB:");
                Console.WriteLine($"  -> TempAire: {condicion.Temp_aire}");
                Console.WriteLine($"  -> VelocidadAire: {condicion.Velocidad_Aire}");
                Console.WriteLine($"  -> HumedadRelativa: {condicion.HumedadRelativa}"); // Sospechoso #1
                Console.WriteLine($"  -> TasaEstimada: {condicion.Tasa_Estimada}");
                Console.WriteLine($"  -> Ropa: {condicion.Ropa}");

                // --- Preparación de parámetros ---
                double ta = (double)condicion.Temp_aire;
                double tr = ta; // Asunción: Temperatura Radiante = Temperatura del Aire
                double vel = (double)condicion.Velocidad_Aire;
                double rh = (double)condicion.HumedadRelativa * 100; // Sospechoso #2: ¿Se está haciendo esta multiplicación?
                double met = (double)condicion.Tasa_Estimada;
                double clo = (double)condicion.Ropa;

                // <-- DEBUG 2: Muestra los valores JUSTO ANTES de enviarlos a la función de cálculo
                Console.WriteLine($"[DEBUG 2] Parámetros FINALES enviados a PmvCalculator.CalculatePmv:");
                Console.WriteLine($"  -> ta (Temp. Aire °C): {ta}");
                Console.WriteLine($"  -> tr (Temp. Radiante °C): {tr}");
                Console.WriteLine($"  -> vel (Velocidad Aire m/s): {vel}");
                Console.WriteLine($"  -> rh (Humedad Relativa %): {rh}"); // ¡Este es el valor más importante a revisar!
                Console.WriteLine($"  -> met (Tasa Metabólica W/m²): {met}");
                Console.WriteLine($"  -> clo (Aislamiento Ropa clo): {clo}");

                // --- Cálculo ---
                double pmvCalculado = PmvCalculator.CalculatePmv(ta, tr, vel, rh, met, clo);

                double ppdCalculado = PmvCalculator.CalculatePpd(pmvCalculado);


                // <-- DEBUG 3: Muestra el resultado del cálculo
                Console.WriteLine($"[DEBUG 3] Resultado del PMV calculado: {pmvCalculado}");

                // --- Actualización de la DB ---
                var panelDato = _dbContext.Set<PanelDatos_Calor>()
                                          .FirstOrDefault(p => p.Id_informe == informeId && p.id_area == condicion.id_area);

                if (panelDato != null)
                {
                    panelDato.PMV = (decimal)pmvCalculado;
                    panelDato.PPD = (decimal)ppdCalculado; // Asumiendo que la columna existe

                    Console.WriteLine($"[DEBUG 4] Actualizando PMV en PanelDatos_Calor para Area ID: {condicion.id_area}");
                }
                else
                {
                    Console.WriteLine($"[DEBUG 4] ERROR: No se encontró PanelDatos_Calor para Area ID: {condicion.id_area}");
                }
                Console.WriteLine("--- Fin Depuración Área ---");
            }

            _dbContext.SaveChanges();
            Console.WriteLine("\nCambios guardados en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error catastrófico en CalcularYGuardarPMVPorArea: {ex.ToString()}");
            throw new Exception($"Error al calcular y guardar el PMV para el informeId {informeId}.", ex);
        }
    }


    private decimal ObtenerValorDeFila(Dictionary<string, object> fila)
    {
        // Obtener y convertir el valor de la fila
        var valorString = fila["Valor"]?.ToString()
            .Replace("°C", "")
            .Replace("%", "")
            .Trim();

        if (decimal.TryParse(valorString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor))
        {
            return valor;
        }
        return 0;
    }

    private decimal ParseDecimal(Dictionary<string, object> fila, string clave, bool isPercentage = false)
    {
        if (fila.ContainsKey(clave) && fila[clave] != null)
        {
            var stringValue = fila[clave].ToString()
                .Replace("°C", "")
                .Replace("%", "")
                .Trim();

            if (decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
            {
                return isPercentage ? value / 100 : value; // Convertir porcentajes si es necesario
            }
        }
        return 0;
    }




}
