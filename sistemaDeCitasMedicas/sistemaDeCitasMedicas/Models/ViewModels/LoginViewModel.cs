using System.ComponentModel.DataAnnotations;

namespace sistemaDeCitasMedicas.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido")]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}