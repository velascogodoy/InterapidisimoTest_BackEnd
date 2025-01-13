using Servicios.CRUD;
using Servicios.InicioSesion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inyeccion de dependencias
builder.Services.AddSingleton<IIniciarSesionServicio, IniciarSesionServicio>();
builder.Services.AddSingleton<IRegistrosServicios, RegistrosServicios>();
builder.Services.AddSingleton<IConsultasServicios, ConsultasServicios>();

//configuracion de CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("nuevaPolitica", app =>
    {
        app.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("nuevaPolitica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
