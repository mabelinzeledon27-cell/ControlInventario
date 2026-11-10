using ControlInventario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlInventario.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly InventarioContext _context;

        public ProveedoresController(InventarioContext context)
        {
            _context = context;
        }

        // Mostrar proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return View(proveedores);
        }

        // Mostrar formulario de registro
        public IActionResult Create()
        {
            return View();
        }

        // Guardar proveedor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Telefono")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        // Mostrar formulario de edición
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombre,Telefono")] Proveedor proveedor)
        {
            if (id != proveedor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        // Mostrar confirmación de eliminación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // Eliminar proveedor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.Productos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null) return NotFound();

            if (proveedor.Productos.Any())
            {
                TempData["Error"] =
                    "No se puede eliminar porque tiene productos asociados.";

                return RedirectToAction(nameof(Index));
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}