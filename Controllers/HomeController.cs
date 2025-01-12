using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_frc11.Models;

namespace tl2_tp6_2024_frc11.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _log;

    public HomeController(ILogger<HomeController> log)
    {
        _log = log;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult PoliticaPrivacidad()
    {
        return View("Privacy");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult MostrarError()
    {
        var modeloError = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        return View("Error", modeloError);
    }
}
