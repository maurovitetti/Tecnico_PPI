using System.ComponentModel.DataAnnotations;

namespace PPI.DTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// Nombre del usuario
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El campo {0} no debe tener más de 20 caracteres.")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Contraseña usuario
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El campo {0} no debe tener más de 20 caracteres.")]
        public string Password { get; set; } = null!;
    }

}
