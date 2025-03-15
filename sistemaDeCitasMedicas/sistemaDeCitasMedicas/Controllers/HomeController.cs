using Microsoft.AspNetCore.Mvc;
using sistemaDeCitasMedicas.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace sistemaDeCitasMedicas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // Página principal que depende del estado de autenticación
        public IActionResult Index()
        {
            var usuarioAutenticado = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (usuarioAutenticado)
            {
                return View("Index"); // Si el usuario está autenticado, mostrar su panel
            }

            return View("Home"); // Si no, mostrar la página de inicio con login y registro
        }

        // Página de privacidad (no cambia, solo para referencia)
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
