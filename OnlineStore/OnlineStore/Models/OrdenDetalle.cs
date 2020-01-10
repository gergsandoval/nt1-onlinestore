using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("OrdenDetalles")]
    public class OrdenDetalle
    {
        private int ordenDetalleId;
        private int productoId;
        private Producto producto;
        private int cantidad;
        private int ordenId;
        private Orden orden;

        public int OrdenDetalleId { get => ordenDetalleId; set => ordenDetalleId = value; }
        public int ProductoId { get => productoId; set => productoId = value; }
        public virtual Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        [NotMapped]
        public int Subtotal { get => obtenerSubtotal(); }
        public int OrdenId { get => ordenId; set => ordenId = value; }
        public virtual Orden Orden { get => orden; set => orden = value; }

        private int obtenerSubtotal()
        {
            return Producto.Precio * Cantidad;
        }
    }
}