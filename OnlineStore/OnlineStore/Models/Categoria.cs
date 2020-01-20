using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        private int categoriaId;
        private string nombre;

        public int CategoriaId { get => categoriaId; set => categoriaId = value; }
        [Required(ErrorMessage ="La {0} es obligatoria")]
        [StringLength(20,ErrorMessage = "La {0} debe tener como maximo {1} caracteres")]
        [Display(Name = "Categoria")]
        public string Nombre { get => nombre; set => nombre = value; }
    }
}