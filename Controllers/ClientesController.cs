using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class ClientesController : Controller
{
    private readonly ILogger<ClientesController> _logger;
    private readonly ClientesRepository _clientesRepository;

    public ClientesController(ILogger<ClientesController> logger)
    {
        _logger = logger;
        _clientesRepository = new ClientesRepository();
    }

    /*public IActionResult Index()
    {
        return View(_clientesRepository.ObtenerProductos());
    }*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}