using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(50, ErrorMessage = "La {0} debe tener como maximo {1} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }

}
