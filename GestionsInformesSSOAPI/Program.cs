using GestionsInformesSSOAPI.Features.Repositories;
using GestionsInformesSSOAPI.Features.Repository;
using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{      
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://www.ahm-honduras.com") 
              .AllowAnyMethod() 
              .AllowAnyHeader() 
              .AllowCredentials(); 
    });
});

builder.Services.AddDbContext<GestionInformesSSO>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<EmpresasRepository>();
builder.Services.AddScoped<EquiposRepository>();
builder.Services.AddScoped<ExcelRepository>();
builder.Services.AddScoped<InformesCalorRepository>();
builder.Services.AddScoped<RopaUtilizadaRepository>();


builder.Services.AddScoped<UsuarioServices>();
builder.Services.AddScoped<EmpresasService>();
builder.Services.AddScoped<EquiposService>();
builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<InformesCalorService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<RopaUtilizadaService>();


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
