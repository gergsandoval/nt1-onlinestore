using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class Orden
    {
        private int ordenId;
        private string usuarioEmail;
        private DateTime fechaCompra;
        private ICollection<OrdenDetalle> detalles;

        public int OrdenId { get => ordenId; set => ordenId = value; }
        [Display(Name ="Email del Usuario")]
        public string UsuarioEmail { get => usuarioEmail; set => usuarioEmail = value; }
        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra { get => fechaCompra; set => fechaCompra = value; }
        public virtual ICollection<OrdenDetalle> Detalles { get => detalles; set => detalles = value; }
        [NotMapped]
        public int Total { get => obtenerTotal(); }

        private int obtenerTotal()
        {
            int total = 0;
            foreach (var item in Detalles)
            {
                total += item.Subtotal;
            }
            return total;
        }
    }
}