using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPI.DTOs
{
    public class NewFCIDTO
    {
        /// <summary>
        /// Código que sirve para identificar de forma abreviaba los FCI 
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(10, ErrorMessage = "El campo {0} no debe tener más de 10 caracteres.")]
        [DefaultValue("")]
        public string Ticker { get; set; } = null!;

        /// <summary>
        /// Nombre del FCI
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Validacion del modelo de la clase NewFCIDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(Ticker))
                modelState.AddModelError("Ticker", "El campo Ticker es obligatorio.");

            if (string.IsNullOrEmpty(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre es obligatorio.");

            return modelState.IsValid;
        }
    }
    public class UpdateFCIDTO
    {
        /// <summary>
        /// Código que sirve para identificar de forma abreviaba un FCI <br /> Parámetro opcional
        /// </summary>
        [MaxLength(10, ErrorMessage = "El campo {0} no debe tener más de 10 caracteres.")]
        [DefaultValue("")]
        public string? Ticker { get; set; }

        /// <summary>
        /// Nombre del FCI <br /> Parámetro opcional
        /// </summary>
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string? Nombre { get; set; }

        /// <summary>
        /// Validacion del modelo de la clase UpdateFCIDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (Nombre != null && string.IsNullOrWhiteSpace(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre no puede estar vacío ni contener solo espacios en blanco.");

            if (Ticker != null && string.IsNullOrWhiteSpace(Ticker))
                modelState.AddModelError("Ticker", "El campo Ticker no puede estar vacío ni contener solo espacios en blanco.");

            if (Ticker == null && Nombre == null)
                modelState.AddModelError("Parametros", "Debe proporcionar al menos un campo para actualizar.");

            return modelState.IsValid;
        }
    }
}
