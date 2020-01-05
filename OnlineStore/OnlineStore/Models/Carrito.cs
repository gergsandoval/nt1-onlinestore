using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class Carrito
    {
        private ICollection<CarritoItem> productos;
        public ICollection<CarritoItem> Productos { get => productos; set => productos = value; }
    }
}