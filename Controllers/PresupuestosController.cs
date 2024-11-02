using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

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
        var presupuestos = _presupuestosRepository.GetPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult ListarDetalles(int id)
    {
        var listaDetalle = _presupuestosRepository.ObtenerDetalle(id);
        return View(listaDetalle);
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

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        var presupuesto = _presupuestosRepository.ObtenerDetalle(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
    {
        if (ModelState.IsValid)
        {
            _presupuestosRepository.ModificarPresupuesto(presupuesto);
            return RedirectToAction(nameof(Index));
        }
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult EliminarPresupuestoConfirmado(int id)
    {
        _presupuestosRepository.EliminarPresupuestoPorId(id);
        return RedirectToAction(nameof(Index));
    }

     public IActionResult Index()
    {
        return View(_presupuestosRepository.GetPresupuestos());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}