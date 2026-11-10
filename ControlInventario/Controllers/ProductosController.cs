
using ControlInventario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ControlInventario.Controllers
{
    public class ProductosController : Controller
    {
        private readonly InventarioContext _context;

        public ProductosController(InventarioContext context)
        {
            _context = context;
        }

        // Mostrar productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Proveedor)
                .ToListAsync();

            return View(productos);
        }

        // Mostrar formulario de registro
        public IActionResult Create()
        {
            ViewBag.ProveedorId = new SelectList(
                _context.Proveedores, "Id", "Nombre");

            return View();
        }

        // Guardar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Stock,Precio,ProveedorId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProveedorId = new SelectList(
                _context.Proveedores, "Id", "Nombre",
                producto.ProveedorId);

            return View(producto);
        }

        // Mostrar formulario de edición
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null) return NotFound();

            ViewBag.ProveedorId = new SelectList(
                _context.Proveedores, "Id", "Nombre",
                producto.ProveedorId);

            return View(producto);
        }

        // Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombre,Stock,Precio,ProveedorId")] Producto producto)
        {
            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProveedorId = new SelectList(
                _context.Proveedores, "Id", "Nombre",
                producto.ProveedorId);

            return View(producto);
        }

        // Mostrar confirmación de eliminación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // Eliminar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
