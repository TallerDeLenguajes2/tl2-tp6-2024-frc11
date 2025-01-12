using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly IUsuariosRepository _repositorioUsuarios;
    private readonly ILogger<LoginController> _log;

    public LoginController(IUsuariosRepository repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public IActionResult Index()
    {
        var modelo = new LoginViewModel
        {
            Autenticado = HttpContext.Session.GetString("Autenticado") == "true"
        };
        return View(modelo);
    }

    public IActionResult IniciarSesion(LoginViewModel modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Usuario) || string.IsNullOrWhiteSpace(modelo.Contraseña))
        {
            modelo.Error = "Por favor, ingrese usuario y contraseña.";
            return View("Index", modelo);
        }

        var usuarioEncontrado = _repositorioUsuarios.GetUsuarios(modelo.Usuario, modelo.Contraseña);
        if (usuarioEncontrado != null)
        {
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetString("Usuario", usuarioEncontrado.Usuario);
            HttpContext.Session.SetString("Rol", usuarioEncontrado.Rol.ToString());
            return RedirectToAction("Index", "Presupuestos");
        }

        modelo.Error = "Usuario o contraseña incorrectos.";
        modelo.Autenticado = false;
        return View("Index", modelo);
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
