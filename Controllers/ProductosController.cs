using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private readonly ProductosRepository _productosRepository;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        _productosRepository = new ProductosRepository();
    }

    [HttpGet]
    public IActionResult ListarProductos()
    {
        var productos = _productosRepository.ObtenerProductos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearProducto(Producto producto)
    {
        if (ModelState.IsValid) // se utiliza para verificar si los datos enviados en un formulario cumplen con todas las reglas de validación definidas en el modelo de datos.
        {
            _productosRepository.CrearProducto(producto);
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

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
    [ValidateAntiForgeryToken]
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
    [ValidateAntiForgeryToken] //Es una buena práctica proteger las acciones POST con tokens antifalsificación para prevenir ataques Cross-Site Request Forgery (CSRF).
    public IActionResult EliminarProductoConfirmado(int id)
    {
        //En este caso no es necesario el ModelState.IsValid porque solo recibo un dato simple(id)
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
}