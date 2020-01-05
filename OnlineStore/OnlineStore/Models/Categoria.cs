using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class Categoria
    {
        private int categoriaId;
        private string nombre;

        public int CategoriaId { get => categoriaId; set => categoriaId = value; }
        [Required(ErrorMessage ="El {0} es obligatorio")]
        [StringLength(20,ErrorMessage = "El {0} debe tener como maximo {1} caracteres")]
        [Display(Name = "Categoria")]
        public string Nombre { get => nombre; set => nombre = value; }
    }
}