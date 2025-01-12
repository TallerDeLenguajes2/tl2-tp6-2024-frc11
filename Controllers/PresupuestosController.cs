using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _log;
    private readonly IPresupuestosRepository _repositorioPresupuestos;
    private readonly IClientesRepository _repositorioClientes;
    private readonly IProductosRepository _repositorioProductos;

    public PresupuestosController(ILogger<PresupuestosController> log, IPresupuestosRepository presupuestosRepo, IClientesRepository clientesRepo, IProductosRepository productosRepo)
    {
        _log = log;
        _repositorioPresupuestos = presupuestosRepo;
        _repositorioClientes = clientesRepo;
        _repositorioProductos = productosRepo;
    }

    public IActionResult Index()
    {
        return View(_repositorioPresupuestos.ListarPresupuestosGuardados());
    }

    [HttpGet]
    public IActionResult NuevoPresupuesto()
    {
        var clientes = _repositorioClientes.ListarClientesGuardados();
        ViewData["Clientes"] = clientes.Select(c => new SelectListItem
        {
            Value = c.ClienteId.ToString(),
            Text = c.Nombre
        }).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CrearNuevoPresupuesto(AltaPresupuestoViewModel presupuestoVM)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");
        var presupuesto = new Presupuestos(presupuestoVM);
        _repositorioPresupuestos.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int idPresupuesto)
    {
        var productos = _repositorioProductos.ListarProductosRegistrados();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(),
            Text = p.Descripcion
        }).ToList();
        var modelo = new AgregarProductosAPresupuestoViewModel { IdPresupuesto = idPresupuesto };
        return View(modelo);
    }

    [HttpPost]
    public IActionResult ConfirmarAgregarProductos(AgregarProductosAPresupuestoViewModel productosVM)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");
        _repositorioPresupuestos.AgregarProducto(productosVM.IdPresupuesto, productosVM.IdProducto, productosVM.Cantidad);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult QuitarProductosDePresupuesto(int idPresupuesto)
    {
        var detalle = _repositorioPresupuestos.MostrarDetallePorId(idPresupuesto);
        ViewData["DetallePresupuesto"] = detalle;
        return View(idPresupuesto);
    }

    [HttpPost]
    public IActionResult ConfirmarEliminacionProductos(int idPresupuesto, List<int> idsProductos, List<int> cantidadesAnteriores, List<int> nuevasCantidades)
    {
        for (int i = 0; i < idsProductos.Count; i++)
        {
            if (cantidadesAnteriores[i] > nuevasCantidades[i])
            {
                _repositorioPresupuestos.EliminarProducto(idPresupuesto, idsProductos[i], cantidadesAnteriores[i], nuevasCantidades[i]);
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult VerPresupuesto(int idPresupuesto)
    {
        var presupuesto = _repositorioPresupuestos.ObtenerPresupuestoPorId(idPresupuesto);
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult EditarPresupuesto(int idPresupuesto)
    {
        var clientes = _repositorioClientes.ListarClientesGuardados();
        ViewData["Clientes"] = clientes.Select(c => new SelectListItem
        {
            Value = c.ClienteId.ToString(),
            Text = c.Nombre
        }).ToList();

        var presupuesto = _repositorioPresupuestos.ObtenerPresupuestoPorId(idPresupuesto);
        var modelo = new ModificarPresupuestoViewModel
        {
            IdPresupuesto = idPresupuesto,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(modelo);
    }

    [HttpPost]
    public IActionResult ConfirmarEdicionPresupuesto(ModificarPresupuestoViewModel presupuestoVM)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");
        var presupuesto = new Presupuestos(presupuestoVM);
        _repositorioPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int idPresupuesto)
    {
        var presupuesto = _repositorioPresupuestos.ObtenerPresupuestoPorId(idPresupuesto);
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult ConfirmarEliminacionPresupuesto(int idPresupuesto)
    {
        _repositorioPresupuestos.EliminarPresupuestoPorId(idPresupuesto);
        return RedirectToAction("Index");
    }
}
