namespace ControlInventario.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int Stock { get; set; }

        public decimal Precio { get; set; }

        public int ProveedorId { get; set; }

        public Proveedor? Proveedor { get; set; }
    }
}