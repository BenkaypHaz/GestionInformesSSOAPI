using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;

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
        _context.TablasClimaInforme.AddRange(tablasClima);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarTitulosAsync(List<TitulosGraficos> titulos)
    {
        _context.TitulosGraficos.AddRange(titulos);
        await _context.SaveChangesAsync();
    }

}
