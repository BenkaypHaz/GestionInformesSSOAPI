using System;

public class DatosTrabajador
{
    // Propiedades utilizadas en los cálculos
    public double M { get; set; }
    public double W { get; set; }
    public double Tair { get; set; }
    public double Tg { get; set; }
    public double Twb { get; set; }
    public double Vair { get; set; }
    public double Icl { get; set; }
    public double Emax { get; set; }
    public double Hr { get; set; }
    public double Tskin { get; set; }
}

public class CalculadorDeEstrésTérmico
{
    // Métodos para calcular cada paso como funciones separadas
    public static double CalcularWBGT(DatosTrabajador datos)
    {
        return 0.7 * datos.Twb + 0.2 * datos.Tg + 0.1 * datos.Tair;
    }

    public static double CalcularConveccion(DatosTrabajador datos)
    {
        double hc = 8.3 * Math.Pow(datos.Vair, 0.6);
        return hc * (datos.Tskin - datos.Tair);
    }

    public static double CalcularRadiacion(DatosTrabajador datos)
    {
        double hr = 4 * 5.67e-8 * Math.Pow((datos.Tskin + datos.Tg + 546.3) / 2, 3);
        return hr * (datos.Tskin - datos.Tg);
    }

    public static double CalcularPerdidaEvaporativaSudor(DatosTrabajador datos)
    {
        double ef = 1 / (1 + 0.6 * datos.Icl * datos.Hr);
        return datos.Emax * ef * (1 - datos.Icl / 0.5);
    }

    public static double CalcularPerdidaEvaporativaRespiracion(DatosTrabajador datos)
    {
        return 0.0014 * datos.M * (34 - datos.Tair);
    }

    public static string CalcularBalanceDeCalor(DatosTrabajador datos)
    {
        double c = CalcularConveccion(datos);
        double r = CalcularRadiacion(datos);
        double eres = CalcularPerdidaEvaporativaRespiracion(datos);
        double esw = CalcularPerdidaEvaporativaSudor(datos);

        double mw = datos.M - datos.W;  // M - W
        double sumatoria = c + r + eres + esw;  // C + R + Eres + Esw

        // Preparar el mensaje con los resultados
        string mensaje = $"M-W = {mw:0.00}, C+R+Eres+Esw = {sumatoria:0.00}";

        // Comparación de los resultados para determinar la condición de balance
        if (Math.Abs(mw - sumatoria) < 0.1) // Umbral de error pequeño para considerar igualdad
        {
            mensaje += " - El balance de calor es neutro. El riesgo de estrés térmico es bajo.";
        }
        else if (mw > sumatoria)
        {
            mensaje += " - El cuerpo está acumulando calor, lo que puede aumentar el riesgo de estrés térmico.";
        }
        else
        {
            mensaje += " - Como la disipación total de calor es negativa, el cuerpo no puede eliminar suficiente calor, lo que indica un alto riesgo de estrés térmico severo.";
        }

        return mensaje;
    }

    public static void Main()
    {
        DatosTrabajador datos = new DatosTrabajador
        {
            M = 300,
            W = 0,
            Tair = 32,
            Tg = 35,
            Twb = 31,
            Vair = 0.3,
            Icl = 0.5,
            Emax = 11.6,
            Hr = 0.45,
            Tskin = 35
        };

        Console.WriteLine("WBGT: " + CalcularWBGT(datos));
        Console.WriteLine("Balance de Calor: " + CalcularBalanceDeCalor(datos));
    }
}
