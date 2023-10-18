using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPI.DTOs
{
    public class NewAccionDTO
    {
        /// <summary>
        /// Código que sirve para identificar de forma abreviaba las acciones de una determinada empresa
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(10, ErrorMessage = "El campo {0} no debe tener más de 10 caracteres.")]
        [DefaultValue("")]
        public string Ticker { get; set; } = null!;

        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Precio unitario de la acción
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(1)]
        public decimal PrecioUnitario { get; set; }


        /// <summary>
        /// Validacion del modelo de la clase NewAccionDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(Ticker))
                modelState.AddModelError("Ticker", "El campo Ticker es obligatorio.");

            if (string.IsNullOrEmpty(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre es obligatorio.");

            if (PrecioUnitario <= 0)
                modelState.AddModelError("PrecioUnitario", "El campo PrecioUnitario debe ser mayor que cero.");

            return modelState.IsValid;
        }
    }

    public class UpdateAccionDTO
    {
        /// <summary>
        /// Código que sirve para identificar de forma abreviaba las acciones de una determinada empresa <br /> Parámetro opcional
        /// </summary>
        [MaxLength(10, ErrorMessage = "El campo {0} no debe tener más de 10 caracteres.")]
        [DefaultValue("")]
        public string? Ticker { get; set; }

        /// <summary>
        /// Nombre de la empresa <br /> Parámetro opcional
        /// </summary>
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string? Nombre { get; set; }

        /// <summary>
        /// Precio unitario de la acción <br /> Parámetro opcional
        /// </summary>
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(1)]
        public decimal? PrecioUnitario { get; set; }

        /// <summary>
        /// Validacion del modelo de la clase UpdateAccionDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (Nombre != null && string.IsNullOrWhiteSpace(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre no puede estar vacío ni contener solo espacios en blanco.");

            if (Ticker != null && string.IsNullOrWhiteSpace(Ticker))
                modelState.AddModelError("Ticker", "El campo Ticker no puede estar vacío ni contener solo espacios en blanco.");

            if (PrecioUnitario <= 0)
                modelState.AddModelError("PrecioUnitario", "El campo PrecioUnitario debe ser mayor que cero.");

            if (Ticker == null && Nombre == null && PrecioUnitario == null)
                modelState.AddModelError("Parametros", "Debe proporcionar al menos un campo para actualizar.");

            return modelState.IsValid;
        }
    }
}
