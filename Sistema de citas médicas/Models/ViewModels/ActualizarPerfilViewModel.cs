using System.ComponentModel.DataAnnotations;

namespace Sistema_de_citas_médicas.Models.ViewModels
{
    public class ActualizarPerfilViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Formato de teléfono no válido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido")]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Contraseña actual")]
        [DataType(DataType.Password)]
        public string ContrasenaActual { get; set; }

        [Display(Name = "Nueva contraseña (dejar en blanco para mantener la actual)")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaContrasena { get; set; }

        [Display(Name = "Confirmar nueva contraseña")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; }
    }
}