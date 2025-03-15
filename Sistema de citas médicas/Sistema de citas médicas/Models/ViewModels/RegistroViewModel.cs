using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema_de_citas_médicas_.Models.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [Display(Name = "Cédula")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "El formato de cédula debe ser XXX-XXXXXXX-X")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Formato de teléfono no válido")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "El formato de teléfono debe ser XXX-XXX-XXXX")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido")]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; }

        [Display(Name = "Acepto los términos y condiciones")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Debe aceptar los términos y condiciones")]
        public bool AceptaTerminos { get; set; }
    }
}