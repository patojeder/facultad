using Microsoft.AspNetCore.Mvc;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private readonly PresupuestosRepository _presupuestosRepository;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _presupuestosRepository = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult ListarPresupuestos()
    {
        var presupuestos = _presupuestosRepository.ObtenerPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult CrearPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    {
        if (ModelState.IsValid) // se utiliza para verificar si los datos enviados en un formulario cumplen con todas las reglas de validación definidas en el modelo de datos.
        {
            _presupuestosRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction(nameof(Index));
        }
        return View(presupuesto);
    }

    /*

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto = _productosRepository.ObtenerProducto(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarProducto(int id, Producto producto)
    {
        if (ModelState.IsValid)
        {
            _productosRepository.ModificarProducto(id, producto);
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        var producto = _productosRepository.ObtenerProducto(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult EliminarProductoConfirmado(int id)
    {
        _productosRepository.EliminarProducto(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Index()
    {
        return View(_productosRepository.ObtenerProductos());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    */
}