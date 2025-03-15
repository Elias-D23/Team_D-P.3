using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la conexión a la base de datos SQLite
builder.Services.AddDbContext<CitaMedicaContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionSQL"));
});

// Añadir servicios de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CitaMedicaContext>();

    try
    {
        // Crear la base de datos si no existe
        db.Database.EnsureCreated();
        Console.WriteLine("Base de datos SQLite creada exitosamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al crear la base de datos SQLite: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Solo usar redirección HTTPS en producción
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

// Usar sesiones antes de la autorización
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
