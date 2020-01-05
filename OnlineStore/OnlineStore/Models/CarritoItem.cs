using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class CarritoItem
    {
        private int carritoItemId;
        private int productoId;
        private Producto producto;
        private int cantidad;
        private int subtotal;
        private string userEmail;

        public int CarritoItemId { get => carritoItemId; set => carritoItemId = value; }
        public int ProductoId { get => productoId; set => productoId = value; }
        public virtual Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public int Subtotal { get => obtenerSubtotal(producto, cantidad); }
        public string UserEmail { get => userEmail; set => userEmail = value; }

        private int obtenerSubtotal(Producto producto, int cantidad)
        {
            return producto.Precio * cantidad;
        }
        
    }
}