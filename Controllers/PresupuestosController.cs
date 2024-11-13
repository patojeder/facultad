using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private readonly PresupuestosRepository _presupuestosRepository;
    private readonly ClientesRepository _clientesRepository;
    private readonly ProductosRepository _productosRepository;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _presupuestosRepository = new PresupuestosRepository();
        _clientesRepository = new ClientesRepository();
        _productosRepository = new ProductosRepository();
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
        var viewModel = new PresupuestoViewModel
        {
            Productos = _productosRepository.ObtenerProductos(),
            Clientes = _clientesRepository.ObtenerClientes()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearPresupuesto(PresupuestoViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (viewModel.ClienteIdSeleccionado == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un cliente antes de agregar productos.");
                viewModel.Clientes = _clientesRepository.ObtenerClientes();
                viewModel.Productos = _productosRepository.ObtenerProductos();
                return View(viewModel);
            }

            var cliente = _clientesRepository.ObtenerCliente(viewModel.ClienteIdSeleccionado);
            var nuevoPresupuesto = new Presupuesto
            {
                Cliente = cliente,
                FechaCreacion = DateTime.Now,
                Detalle = new List<PresupuestoDetalle>()
            };

            // Agrega cada producto con su cantidad al detalle del presupuesto
            foreach (var productoSeleccionado in viewModel.ProductosSeleccionados)
            {
                if (productoSeleccionado.ProductoId > 0 && productoSeleccionado.Cantidad > 0)
                {
                    var producto = _productosRepository.ObtenerProducto(productoSeleccionado.ProductoId);
                    nuevoPresupuesto.Detalle.Add(new PresupuestoDetalle
                    {
                        Producto = producto,
                        Cantidad = productoSeleccionado.Cantidad
                    });
                }
            }

            _presupuestosRepository.CrearPresupuesto(nuevoPresupuesto);
            return RedirectToAction(nameof(Index));
        }

        // Recargo los productos y los clientes para que se muestren si hay errores
        viewModel.Clientes = _clientesRepository.ObtenerClientes();
        viewModel.Productos = _productosRepository.ObtenerProductos();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        // Cargo el presupuesto y los clientes desde la base de datos
        var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
        var clientes = _clientesRepository.ObtenerClientes();

        var viewModel = new ModificarPresupuestoViewModel
        {
            Presupuesto = presupuesto,
            Clientes = clientes,
            ClienteIdSeleccionado = presupuesto.Cliente.Id  // ID del cliente actual
        };

        return View(viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel viewModel)
    {

        if (viewModel.ClienteIdSeleccionado == 0)
        {
            ModelState.AddModelError("ClienteIdSeleccionado", "Debe seleccionar un cliente válido");
            viewModel.Clientes = _clientesRepository.ObtenerClientes();
            return View(viewModel);
        }

        var presupuestoExistente = _presupuestosRepository.ObtenerPresupuestoPorId(viewModel.Presupuesto.IdPresupuesto);
        if (presupuestoExistente == null)
        {
            return NotFound();
        }

        // Actualizo los datos del presupuesto
        var presupuestoActualizado = new Presupuesto
        {
            IdPresupuesto = viewModel.Presupuesto.IdPresupuesto,
            FechaCreacion = presupuestoExistente.FechaCreacion, // Mantener la fecha original
            Cliente = new Cliente { Id = viewModel.ClienteIdSeleccionado },
            Detalle = viewModel.Presupuesto.Detalle.Select(d => new PresupuestoDetalle
            {
                Producto = d.Producto,
                Cantidad = d.Cantidad
            }).ToList() ?? new List<PresupuestoDetalle>()
        };

        _presupuestosRepository.ModificarPresupuestoQ(presupuestoActualizado);

        return RedirectToAction(nameof(Index));

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
    [ValidateAntiForgeryToken]
    public IActionResult EliminarPresupuestoConfirmado(int id)
    {
        //No hace falta el ModelState.IsValid
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