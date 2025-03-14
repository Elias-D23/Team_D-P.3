using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Data;

var builder = WebApplication.CreateBuilder(args);

//Connection DB.
builder.Services.AddDbContext<CitaMedicaContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionSQL"));
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
        // Crear la base de datos si no existe y aplicar migraciones
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
