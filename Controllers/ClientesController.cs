using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class ClientesController : Controller
{
    private readonly ILogger<ClientesController> _logger;
    private readonly IClientesRepository _clientesRepository;

    public ClientesController(ILogger<ClientesController> logger, IClientesRepository clientesRepository)
    {
        _logger = logger;
        _clientesRepository = clientesRepository;
    }

    [HttpGet]
    public IActionResult ListarClientes()
    {
        var clientes = _clientesRepository.ObtenerClientes();
        return View(clientes);
    }

    [HttpGet]
    public IActionResult CrearCliente()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearCliente(Cliente cliente)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if (ModelState.IsValid) // se utiliza para verificar si los datos enviados en un formulario cumplen con todas las reglas de validación definidas en el modelo de datos.
        {
            _clientesRepository.CrearCliente(cliente);
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var cliente = _clientesRepository.ObtenerCliente(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ModificarCliente(int id, Cliente cliente)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            _clientesRepository.ModificarCliente(id, cliente);
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var cliente = _clientesRepository.ObtenerCliente(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] //Es una buena práctica proteger las acciones POST con tokens antifalsificación para prevenir ataques Cross-Site Request Forgery (CSRF).
    public IActionResult EliminarClienteConfirmado(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        //En este caso no es necesario el ModelState.IsValid porque solo recibo un dato simple(id)
        _clientesRepository.EliminarCliente(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        ViewData["Admin"] = HttpContext.Session.GetString("Rol") == "Admin";
        return View(_clientesRepository.ObtenerClientes());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}