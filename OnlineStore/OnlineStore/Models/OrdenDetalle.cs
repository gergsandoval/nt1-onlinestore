using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("OrdenDetalles")]
    public class OrdenDetalle
    {
        private int ordenDetalleId;
        private string nombreProducto;
        private int precioProducto;
        private int cantidad;
        private int ordenId;
        private Orden orden;

        public int OrdenDetalleId { get => ordenDetalleId; set => ordenDetalleId = value; }
        [Display(Name = "Nombre")]
        public string NombreProducto { get => nombreProducto; set => nombreProducto = value; }
        [Display(Name = "Precio")]
        public int PrecioProducto { get => precioProducto; set => precioProducto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        [NotMapped]
        public int Subtotal { get => obtenerSubtotal(); }
        public int OrdenId { get => ordenId; set => ordenId = value; }
        public virtual Orden Orden { get => orden; set => orden = value; }


        private int obtenerSubtotal()
        {
            return PrecioProducto * Cantidad;
        }
    }
}