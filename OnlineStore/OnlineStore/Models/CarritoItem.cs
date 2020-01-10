using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("CarritoItems")]
    public class CarritoItem
    {
        private int carritoItemId;
        private int productoId;
        private Producto producto;
        private int cantidad;
        private string usuarioEmail;

        public int CarritoItemId { get => carritoItemId; set => carritoItemId = value; }
        public int ProductoId { get => productoId; set => productoId = value; }
        public virtual Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        [NotMapped]
        public int Subtotal { get => obtenerSubtotal(); }
        public string UsuarioEmail { get => usuarioEmail; set => usuarioEmail = value; }

        private int obtenerSubtotal()
        {
            return Producto.Precio * Cantidad;
        } 
    }
}