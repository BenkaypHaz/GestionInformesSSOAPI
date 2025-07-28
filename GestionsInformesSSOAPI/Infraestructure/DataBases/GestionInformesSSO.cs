using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Infraestructure.DataBases
{
    public class GestionInformesSSO : DbContext
    {
        public GestionInformesSSO(DbContextOptions<GestionInformesSSO> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VistaCondicionesPorArea>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VistaCondicionesPorArea"); 
            });
        }


        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RolesUsuario> RolesUsuario { get; set; }
        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Equipos> Equipos { get; set; }
        public DbSet<DatosTiempo_Calor> DatosTiempo_Calor { get; set; }
        public DbSet<GraficoDatos_Calor> GraficoDatos_Calor { get; set; }
        public DbSet<InformesCalor> InformesCalor { get; set; }
        public DbSet<PanelDatos_Calor> PanelDatos_Calor { get; set; }
        public DbSet<TablasClimaInforme> TablasClimaInforme { get; set; }
        public DbSet<EquipoUtilizadoInforme> EquipoUtilizadoInforme { get; set; }
        public DbSet<RopaUtilizada> RopaUtilizada { get; set; }
        public DbSet<TasaMetabolica> TasaMetabolica { get; set; }
        public DbSet<TitulosGraficos> TitulosGraficos { get; set; }
        public DbSet<ValoresProyectados_Calor> ValoresProyectados_Calor { get; set; }
        public DbSet<VistaCondicionesPorArea> VistaCondicionesPorArea { get; set; }
        public DbSet<CriteriosEvaluacion> CriteriosEvaluacion { get; set; }

    }
}
