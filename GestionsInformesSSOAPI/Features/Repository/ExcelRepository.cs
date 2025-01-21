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
            if (datosProcesados.ContainsKey("Panel de datos de resumen"))
            {
                var filas = datosProcesados["Panel de datos de resumen"];
                var panelDatos = new List<PanelDatos_Calor>();
                PanelDatos_Calor cuadroActual = null;

                foreach (var fila in filas)
                {
                    if (!fila.ContainsKey("Descripción") || !fila.ContainsKey("Valor"))
                        continue;

                    string descripcion = fila["Descripción"].ToString();
                    decimal valor = ObtenerValorDeFila(fila);

                    if (descripcion == "WBGT de entrada promedio ")
                    {
                        if (cuadroActual != null)
                        {
                            cuadroActual.Id_informe = informeId; // Asociar el registro al informe
                            panelDatos.Add(cuadroActual);
                        }

                        cuadroActual = new PanelDatos_Calor { WBGT = valor, Id_informe = informeId };
                    }
                    else if (cuadroActual != null)
                    {
                        switch (descripcion)
                        {
                            case "Temperatura de bulbo seco promedio ":
                                cuadroActual.BulboSeco = valor;
                                break;
                            case "Temperatura de bulbo húmedo promedio":
                                cuadroActual.BulboHumedo = valor;
                                break;
                            case "Temperatura en cuerpo negro promedio ":
                                cuadroActual.CuerpoNegro = valor;
                                break;
                            case "Índice térmico promedio":
                                cuadroActual.IndiceTermico = valor;
                                break;
                            case "Humedad promedio":
                                cuadroActual.HumedadPromedio = valor / 100;
                                break;
                        }
                    }
                }

                if (cuadroActual != null)
                {
                    cuadroActual.Id_informe = informeId; // Asociar el último cuadro al informe
                    panelDatos.Add(cuadroActual);
                }

                if (panelDatos.Any())
                {
                    _dbContext.Set<PanelDatos_Calor>().AddRange(panelDatos);
                }
            }


            if (datosProcesados.ContainsKey("Gráfica de datos de registro"))
            {
                var graficoDatos = datosProcesados["Gráfica de datos de registro"]
                    .Where(fila => DateTime.TryParse(fila["Timestamp"]?.ToString(), out _))
                    .Select(fila => new GraficoDatos_Calor
                    {
                        Fecha = DateTime.Parse(fila["Timestamp"].ToString()),
                        DryBulb = ParseDecimal(fila, "DryBulb"),
                        Humidity = ParseDecimal(fila, "Humidity"),
                        Id_informe = informeId // Asociar el registro al informe
                    })
                    .ToList();

                if (graficoDatos.Any())
                {
                    _dbContext.Set<GraficoDatos_Calor>().AddRange(graficoDatos);
                }
            }


            if (datosProcesados.ContainsKey("MS"))
            {
                var filas = datosProcesados["MS"];
                var datosTiempo = new List<DatosTiempo_Calor>();
                int currentAreaId = 1; // Comienza con el área 1

                foreach (var fila in filas)
                {
                    try
                    {
                        // Verifica si todos los campos relevantes están vacíos
                        bool filaVacia = fila.Values.All(value => string.IsNullOrWhiteSpace(value?.ToString()));

                        if (filaVacia)
                        {
                            currentAreaId++; // Incrementa el área al encontrar una fila vacía
                            continue; // Salta esta fila
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
                            Id_informe = informeId, // Asociar el registro al informe
                            id_area = currentAreaId // Asigna el área actual
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error procesando fila: {ex.Message}");
                    }
                }

                if (datosTiempo.Any())
                {
                    _dbContext.Set<DatosTiempo_Calor>().AddRange(datosTiempo);
                }
            }



            // Guardar cambios en la base de datos
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // Manejo de excepciones: registrar errores o re-lanzar
            throw new Exception("Error al guardar los datos desde el archivo Excel.", ex);
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

    private bool EsFilaValida(Dictionary<string, object> fila)
    {
        // Validar que la fila contenga datos relevantes (no vacíos o nulos)
        return fila.ContainsKey("Descripción") && fila.ContainsKey("Valor") &&
               !string.IsNullOrWhiteSpace(fila["Descripción"]?.ToString()) &&
               !string.IsNullOrWhiteSpace(fila["Valor"]?.ToString());
    }

    private bool EsPanelValido(PanelDatos_Calor panel)
    {
        // Validar que al menos uno de los campos tenga datos significativos
        return panel.WBGT > 0 || panel.BulboSeco > 0 || panel.BulboHumedo > 0 ||
               panel.CuerpoNegro > 0 || panel.IndiceTermico > 0 || panel.HumedadPromedio > 0;
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
