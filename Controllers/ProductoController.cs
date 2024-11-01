using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private readonly ProductoRepository _productoRepository;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        _productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult ListarProductos()
    {
        var productos = _productoRepository.ObtenerProductos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        if (ModelState.IsValid) // se utiliza para verificar si los datos enviados en un formulario cumplen con todas las reglas de validación definidas en el modelo de datos.
        {
            _productoRepository.CrearProducto(producto);
            return RedirectToAction(nameof(ListarProductos));
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto = _productoRepository.ObtenerProducto(id);
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
            _productoRepository.ModificarProducto(id, producto);
            return RedirectToAction(nameof(ListarProductos));
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        var producto = _productoRepository.ObtenerProducto(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult EliminarProductoConfirmado(int id)
    {
        _productoRepository.EliminarProducto(id);
        return RedirectToAction(nameof(ListarProductos));
    }

    public IActionResult Index()
    {
        return View(_productoRepository.ObtenerProductos());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}