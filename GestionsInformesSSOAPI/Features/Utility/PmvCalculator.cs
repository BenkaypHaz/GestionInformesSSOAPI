// GestionsInformesSSOAPI/Utils/PmvCalculator.cs
// --- VERSIÓN FINAL REVISADA ---

using System;

public static class PmvCalculator
{
    /// <summary>
    /// Calcula el Voto Medio Estimado (PMV) según la norma ISO 7730. VERSIÓN FINAL.
    /// </summary>
    public static double CalculatePmv(double ta, double tr, double vel, double rh, double met, double clo, double wme = 0)
    {
        // Presión parcial de vapor de agua en Pascales (Pa).
        // Esta es la fórmula corregida. La anterior era incorrecta.
        double pa = rh * 10 * Math.Exp(16.6536 - 4030.183 / (ta + 235));

        double icl = 0.155 * clo; // Resistencia de la ropa en m²K/W
        double m = met;          // Tasa metabólica en W/m²
        double w = wme;          // Trabajo mecánico externo en W/m²
        double mw = m - w;       // Tasa metabólica efectiva

        double fcl = clo < 0.078 ? 1.0 + 1.29 * clo : 1.05 + 0.645 * clo;
        double hcf = 12.1 * Math.Sqrt(vel);
        double taa = ta + 273.15;
        double tra = tr + 273.15;

        // Cálculo iterativo de la temperatura superficial de la ropa (tcl)
        double tcla = taa + (35.5 - ta) / (3.5 * (icl + 0.1));
        double p1 = icl * fcl;
        double p2 = p1 * 3.96;
        double p3 = p1 * 100;
        double p4 = p1 * taa;
        double p5 = 308.7 - 0.028 * mw + p2 * Math.Pow(tra / 100, 4);

        int n = 0;
        double hc = 0;
        double xn = tcla / 100.0;
        double xf = xn;

        while (n <= 150)
        {
            xf = (xf + xn) / 2.0;
            hc = 2.38 * Math.Pow(Math.Abs(100.0 * xf - taa), 0.25);
            if (hcf > hc) hc = hcf;
            xn = (p5 + p4 * hc - p2 * Math.Pow(xf, 4)) / (100.0 + p3 * hc);
            n++;
            if (Math.Abs(xn - xf) < 0.00015) break;
        }

        double tcl = 100.0 * xn - 273.15;

        // Pérdida de calor
        double hl1 = 3.05 * 0.001 * (5733 - 6.99 * mw - pa);
        double hl2 = mw > 58.15 ? 0.42 * (mw - 58.15) : 0;
        double hl3 = 1.7E-05 * m * (5867 - pa);
        double hl4 = 0.0014 * m * (34 - ta);
        double hl5 = 3.96 * fcl * (Math.Pow(xn, 4) - Math.Pow(tra / 100.0, 4));
        double hl6 = fcl * hc * (tcl - ta);

        // Cálculo final
        double ts = 0.303 * Math.Exp(-0.036 * m) + 0.028;
        double pmv = ts * (mw - hl1 - hl2 - hl3 - hl4 - hl5 - hl6);

        return pmv;
    }

    /// <summary>
    /// Calcula el Porcentaje Previsto de Insatisfechos (PPD) a partir de un valor de PMV.
    /// </summary>
    /// <param name="pmv">El valor de PMV previamente calculado.</param>
    /// <returns>El valor de PPD en porcentaje (%).</returns>
    public static double CalculatePpd(double pmv)
    {
        double ppd = 100.0 - 95.0 * Math.Exp(-0.03353 * Math.Pow(pmv, 4) - 0.2179 * Math.Pow(pmv, 2));
        return ppd;
    }

}