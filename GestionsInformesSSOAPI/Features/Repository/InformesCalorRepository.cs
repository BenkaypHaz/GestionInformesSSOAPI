using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

public class InformesCalorRepository
{
    private readonly GestionInformesSSO _context;

    public InformesCalorRepository(GestionInformesSSO context)
    {
        _context = context;
    }

    public async Task<int> GuardarInformeAsync(InformesCalor informe)
    {
        _context.InformesCalor.Add(informe);
        await _context.SaveChangesAsync();
        return informe.IdInfo; // Retorna el ID generado del informe
    }

    public async Task GuardarEquiposAsync(List<EquipoUtilizadoInforme> equipos)
    {
        _context.EquipoUtilizadoInforme.AddRange(equipos);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarClimaAsync(List<TablasClimaInforme> tablasClima)
    {
        // Validar opciones de nubosidad
        var opcionesNubosidadValidas = new[] { "Despejado", "Parcialmente nublado", "Nublado", "Muy nublado" };

        // Regex para validar formato de hora 12h (ej: "8:30 AM", "11:45 PM")
        var regexHora12h = new System.Text.RegularExpressions.Regex(
            @"^(1[0-2]|0?[1-9]):[0-5][0-9]\s?(AM|PM)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        foreach (var tablaClima in tablasClima)
        {
            // Validar nubosidad
            if (!string.IsNullOrEmpty(tablaClima.Nubosidad) &&
                !opcionesNubosidadValidas.Contains(tablaClima.Nubosidad))
            {
                throw new ArgumentException($"Opción de nubosidad '{tablaClima.Nubosidad}' no es válida.");
            }

            // Validar formato de hora de inicio
            if (!string.IsNullOrEmpty(tablaClima.HoraInicio) &&
                !regexHora12h.IsMatch(tablaClima.HoraInicio))
            {
                throw new ArgumentException($"Formato de hora de inicio '{tablaClima.HoraInicio}' no es válido. " +
                                          "Use formato: HH:MM AM/PM (ej: 8:30 AM)");
            }

            // Validar formato de hora de finalización
            if (!string.IsNullOrEmpty(tablaClima.HoraFinalizacion) &&
                !regexHora12h.IsMatch(tablaClima.HoraFinalizacion))
            {
                throw new ArgumentException($"Formato de hora de finalización '{tablaClima.HoraFinalizacion}' no es válido. " +
                                          "Use formato: HH:MM AM/PM (ej: 5:45 PM)");
            }

            // Validar que la hora de fin sea posterior a la de inicio (opcional)
            if (!string.IsNullOrEmpty(tablaClima.HoraInicio) &&
                !string.IsNullOrEmpty(tablaClima.HoraFinalizacion))
            {
                if (!EsHoraPosterior(tablaClima.HoraInicio, tablaClima.HoraFinalizacion))
                {
                    throw new ArgumentException("La hora de finalización debe ser posterior a la hora de inicio.");
                }
            }
        }

        _context.TablasClimaInforme.AddRange(tablasClima);
        await _context.SaveChangesAsync();
    }

    // Método auxiliar para comparar horas en formato 12h
    private bool EsHoraPosterior(string horaInicio, string horaFin)
    {
        try
        {
            var inicio = DateTime.ParseExact(horaInicio, "h:mm tt",
                                           System.Globalization.CultureInfo.InvariantCulture);
            var fin = DateTime.ParseExact(horaFin, "h:mm tt",
                                        System.Globalization.CultureInfo.InvariantCulture);

            return fin > inicio;
        }
        catch
        {
            return false; // Si no se puede parsear, retornar false
        }
    }

    public async Task GuardarTitulosAsync(List<TitulosGraficos> titulos)
    {
        _context.TitulosGraficos.AddRange(titulos);
        await _context.SaveChangesAsync();
    }

    public async Task<InformesCalor> ObtenerPorIdAsync(int id)
    {
        return await _context.InformesCalor
            .FirstOrDefaultAsync(i => i.IdInfo == id);
    }

    public async Task<bool> ActualizarConclusionesAsync(int informeId, string conclusiones)
    {
        var informe = await _context.InformesCalor
            .FirstOrDefaultAsync(i => i.IdInfo == informeId);

        if (informe == null)
            return false;

        informe.Conclusiones = conclusiones;
        await _context.SaveChangesAsync();
        return true;
    }
}
