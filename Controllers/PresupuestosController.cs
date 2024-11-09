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
    public IActionResult CrearPresupuesto(PresupuestoViewModel viewModel, int ProductoId, int Cantidad)
    {
        // Si el modelo es válido
        if (ModelState.IsValid)
        {
            // Si un cliente no ha sido seleccionado, no dejamos agregar productos
            if (viewModel.ClienteIdSeleccionado == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un cliente antes de agregar productos.");
                viewModel.Clientes = _clientesRepository.ObtenerClientes();
                viewModel.Productos = _productosRepository.ObtenerProductos();
                return View(viewModel);
            }

            // Si se selecciona un producto válido
            if (ProductoId > 0 && Cantidad > 0)
            {
                // Verificar si el producto ya fue seleccionado
                if (!viewModel.ProductosSeleccionados.Contains(ProductoId))
                {
                    viewModel.ProductosSeleccionados.Add(ProductoId); // Agregar el producto a la lista
                }

                // Actualizar la cantidad si el producto ya está en la lista
                if (viewModel.CantidadProductosSeleccionados.ContainsKey(ProductoId))
                {
                    viewModel.CantidadProductosSeleccionados[ProductoId] += Cantidad; // Sumar la cantidad
                }
                else
                {
                    viewModel.CantidadProductosSeleccionados.Add(ProductoId, Cantidad); // Agregar nuevo producto con cantidad
                }
            }

            // Recargar productos y clientes para que se muestren correctamente
            viewModel.Clientes = _clientesRepository.ObtenerClientes();
            viewModel.Productos = _productosRepository.ObtenerProductos();

            return View(viewModel);
        }

        // Si el modelo no es válido, recargamos los datos
        viewModel.Clientes = _clientesRepository.ObtenerClientes();
        viewModel.Productos = _productosRepository.ObtenerProductos();
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ConfirmarPresupuesto(PresupuestoViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var cliente = _clientesRepository.ObtenerCliente(viewModel.ClienteIdSeleccionado);

            var nuevoPresupuesto = new Presupuesto
            {
                Cliente = cliente,
                FechaCreacion = DateTime.Now,
                Detalle = new List<PresupuestoDetalle>()
            };

            foreach (var productoId in viewModel.ProductosSeleccionados)
            {
                var producto = _productosRepository.ObtenerProducto(productoId);
                var cantidad = viewModel.CantidadProductosSeleccionados[productoId];

                nuevoPresupuesto.Detalle.Add(new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = cantidad
                });
            }

            _presupuestosRepository.CrearPresupuesto(nuevoPresupuesto);
            return RedirectToAction(nameof(Index)); // Redirigir a la lista de presupuestos
        }

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        var presupuesto = _presupuestosRepository.ObtenerPresupuestoPorId(id);
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
            _presupuestosRepository.ModificarPresupuestoQ(presupuesto);
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