using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("Productos")]
    public class Producto
    {
        private int productoId;
        private string nombre;
        private string descripcion;
        private int stock;
        private int precio;
        private int categoriaId;
        private Categoria categoria;

        public int ProductoId { get => productoId; set => productoId = value; }
        [Required(ErrorMessage ="El {0} es requerido")]
        [StringLength(20, ErrorMessage = "El {0} debe tener como maximo {1} caracteres")]
        public string Nombre { get => nombre; set => nombre = value; }
        [StringLength(50, ErrorMessage = "El {0} debe tener como maximo {1} caracteres")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Required(ErrorMessage = "El {0} es requerido")]
        [Range(0, 99999, ErrorMessage = "El {0} debe estar entre {1} y {2}")]
        public int Stock { get => stock; set => stock = value; }
        [Required(ErrorMessage = "El {0} es requerido")]
        [Range(0, 99999, ErrorMessage = "El {0} debe estar entre {1} y {2}")]
        public int Precio { get => precio; set => precio = value; }
        
        [Required(ErrorMessage = "La {0} es requerida")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get => categoriaId; set => categoriaId = value; }
        public virtual Categoria Categoria { get => categoria; set => categoria = value; }
    }
}