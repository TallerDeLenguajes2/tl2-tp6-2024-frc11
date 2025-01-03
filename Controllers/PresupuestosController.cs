using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PresupuestosController : Controller
    {
        private readonly ILogger<PresupuestosController> _logger;
        private PresupuestosRepository repositorioPresupuestos;

        public PresupuestosController(ILogger<PresupuestosController> logger)
        {
            _logger = logger;
            repositorioPresupuestos = new PresupuestosRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var presupuestos = repositorioPresupuestos.ListarPresupuestosGuardados();
            return View(presupuestos);  
        }

        [HttpGet("AltaPresupuesto")]
        public IActionResult AltaPresupuesto()
        {
            Presupuestos presupuesto = new Presupuestos();
            presupuesto.IdPresupuesto = repositorioPresupuestos.BuscarIdMasGrande() + 1;
            presupuesto.FechaCreacion = DateTime.Now;
            return View(presupuesto);  
        }

        [HttpPost("CrearPresupuesto")]
        public IActionResult CrearPresupuesto(Presupuestos presupuesto)
        {
            repositorioPresupuestos.CrearPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }

        [HttpGet("AgregarProductosAPresupuesto/{id}")]
        public IActionResult AgregarProductosAPresupuesto(int id)
        {
            ProductosRepository repositorioProductos = new ProductosRepository();
            List<Productos> productos = repositorioProductos.ListarProductosRegistrados();
            ViewData["Productos"] = productos;
            return View(id);  
        }

        [HttpGet("EliminarProductosDePresupuesto/{id}")]
        public IActionResult EliminarProductosDePresupuesto(int id)
        {
            List<PresupuestosDetalle> detalles = repositorioPresupuestos.MostrarDetallePorId(id);
            ViewData["PresupuestosDetalle"] = detalles;
            return View(id);  
        }

        [HttpPost("EliminarProductos")]
        public IActionResult EliminarProductos([FromQuery] int idPresupuesto, [FromBody] EliminarProductosRequest request)
        {
            for (int i = 0; i < request.IdsProductos.Count; i++)
            {
                if (request.CantidadesVieja[i] > request.CantidadesNueva[i])
                {
                    repositorioPresupuestos.EliminarProducto(idPresupuesto, request.IdsProductos[i], request.CantidadesVieja[i], request.CantidadesNueva[i]);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet("MostrarPresupuesto/{id}")]
        public IActionResult MostrarPresupuesto(int id)
        {
            Presupuestos? presupuesto = repositorioPresupuestos.ObtenerPresupuestoPorId(id);
            return View(presupuesto);  
        }

        [HttpGet("ModificarPresupuesto/{id}")]
        public IActionResult ModificarPresupuesto(int id)
        {
            Presupuestos? presupuesto = repositorioPresupuestos.ObtenerPresupuestoPorId(id);
            return View(presupuesto);  
        }
    }

    public class EliminarProductosRequest
    {
        public List<int> IdsProductos { get; set; }
        public List<int> CantidadesVieja { get; set; }
        public List<int> CantidadesNueva { get; set; }
    }
}