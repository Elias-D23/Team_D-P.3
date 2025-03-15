using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using sistemaDeCitasMedicas.Data;
using sistemaDeCitasMedicas.Services;


var builder = WebApplication.CreateBuilder(args);

//Connection DB.
builder.Services.AddDbContext<CitaMedicaContext>(options =>
{

    options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionSQL"));
});

// Configurar autenticación con cookies para el login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Paciente/Login";
        options.AccessDeniedPath = "/Paciente/AccesoDenegado";
    });

// Para inyectarlo en los controladores
builder.Services.AddTransient<CorreoService>(); // o AddScoped, dependiendo de la necesidad

// Add services to the container.
builder.Services.AddControllersWithViews();
// Para errores de raiz de proyecto incorrecta al clonarlo:
builder.WebHost.UseWebRoot("wwwroot");

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
