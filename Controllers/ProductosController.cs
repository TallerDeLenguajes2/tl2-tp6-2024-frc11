using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : Controller 
    {
        private readonly ILogger<ProductosController> _logger;
        private ProductosRepository repositorioProductos;

        public ProductosController(ILogger<ProductosController> logger)
        {
            _logger = logger;
            repositorioProductos = new ProductosRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var productos = repositorioProductos.ListarProductosRegistrados();
            return View(productos);  
        }

        [HttpGet("ModificarProducto/{id}")]
        public IActionResult ModificarProducto(int id)
        {
            var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
            return View(producto);  
        }

        [HttpPost("ModificarProductoPorId")]
        public IActionResult ModificarProductoPorId(Productos producto)
        {
            repositorioProductos.ModificarProducto(producto);
            return RedirectToAction("Index");
        }

        [HttpGet("EliminarProducto/{id}")]
        public IActionResult EliminarProducto(int id)
        {
            var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
            return View(producto);  
        }

        [HttpPost("EliminarProductoPorId")]
        public IActionResult EliminarProductoPorId(int id)
        {
            repositorioProductos.EliminarProductoPorId(id);
            return RedirectToAction("Index");
        }
    }
}