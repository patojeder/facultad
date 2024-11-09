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

            // Crea el presupuesto con el cliente seleccionado
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

        // Recargar productos y clientes para que se muestren correctamente si hay errores
        viewModel.Clientes = _clientesRepository.ObtenerClientes();
        viewModel.Productos = _productosRepository.ObtenerProductos();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        // Cargar el presupuesto y la lista de clientes desde la base de datos
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
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Actualizar el cliente del presupuesto antes de guardar
            var presupuesto = viewModel.Presupuesto;
            presupuesto.Cliente.Id = viewModel.ClienteIdSeleccionado;

            // Llama al método que guarda el presupuesto en la base de datos
            _presupuestosRepository.ModificarPresupuestoQ(presupuesto);

            return RedirectToAction("Index");
        }

        // Si hay errores en el modelo, vuelve a cargar la lista de clientes
        viewModel.Clientes = _clientesRepository.ObtenerClientes();
        return View(viewModel);
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