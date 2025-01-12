using Microsoft.AspNetCore.Mvc;

namespace Controllers;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private readonly IProductosRepository _repositorioProductos;

    public ProductosController(ILogger<ProductosController> logger, IProductosRepository repositorioProductos)
    {
        _logger = logger;
        _repositorioProductos = repositorioProductos;
    }

    public IActionResult Index()
    {
        var productos = _repositorioProductos.ListarProductosRegistrados();
        return View(productos);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ConfirmarCreacionProducto(AltaProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "El modelo del producto no es válido.";
            return View("CrearProducto", productoVM);
        }

        var producto = new Productos(productoVM);
        _repositorioProductos.CrearNuevoProducto(producto);
        TempData["Exito"] = "Producto creado exitosamente.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarProducto(int id)
    {
        var producto = _repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction("Index");
        }

        var productoVM = new ModificarProductoViewModel(producto);
        return View(productoVM);
    }

    [HttpPost]
    public IActionResult ConfirmarEdicionProducto(ModificarProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "El modelo del producto no es válido.";
            return View("EditarProducto", productoVM);
        }

        var producto = new Productos(productoVM);
        _repositorioProductos.ModificarProducto(producto);
        TempData["Exito"] = "Producto modificado exitosamente.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DetalleEliminarProducto(int id)
    {
        var producto = _repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction("Index");
        }

        return View(producto);
    }

    [HttpPost]
    public IActionResult ConfirmarEliminacionProducto(int id)
    {
        var producto = _repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction("Index");
        }

        _repositorioProductos.EliminarProductoPorId(id);
        TempData["Exito"] = "Producto eliminado exitosamente.";
        return RedirectToAction("Index");
    }
}
