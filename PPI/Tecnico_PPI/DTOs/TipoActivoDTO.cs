using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPI.DTOs
{
    public class NewTipoActivoDTO
    {
        /// <summary>
        /// Nombre del tipo de activo
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string? Nombre { get; set; } = null!;

        /// <summary>
        /// Comision del tipo de activo
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser un valor positivo.")]
        [DefaultValue(0)]
        public decimal? Comision { get; set; }

        /// <summary>
        /// Impuesto del tipo de activo
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser un valor positivo.")]
        [DefaultValue(0)]
        public decimal? Impuesto { get; set; }

        /// <summary>
        /// Validacion del modelo de la clase NewTipoActivoDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre es obligatorio.");

            if (Comision < 0)
                modelState.AddModelError("Comision", "El campo Comision debe ser un valor positivo.");

            if (Impuesto < 0)
                modelState.AddModelError("Impuesto", "El campo Impuesto debe ser un valor positivo.");

            return modelState.IsValid;
        }
    }

    public class UpdateTipoActivoDTO
    {
        /// <summary>
        /// Nombre del tipo de activo
        /// </summary>
        [MaxLength(50, ErrorMessage = "El campo {0} no debe tener más de 50 caracteres.")]
        [DefaultValue("")]
        public string? Nombre { get; set; } = null!;

        /// <summary>
        /// Comision del tipo de activo
        /// </summary>
        [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser un valor positivo.")]
        [DefaultValue(0)]
        public decimal? Comision { get; set; }

        /// <summary>
        /// Impuesto del tipo de activo
        /// </summary>
        [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "El campo {0} debe ser un valor positivo.")]
        [DefaultValue(0)]
        public decimal? Impuesto { get; set; }

        /// <summary>
        /// Validacion del modelo de la clase UpdateTipoActivoDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (Nombre != null && string.IsNullOrWhiteSpace(Nombre))
                modelState.AddModelError("Nombre", "El campo Nombre no puede estar vacío ni contener solo espacios en blanco.");

            if (Comision < 0)
                modelState.AddModelError("Comision", "El campo Comision debe ser un valor positivo.");

            if (Impuesto < 0)
                modelState.AddModelError("Impuesto", "El campo Impuesto debe ser un valor positivo.");

            if (Nombre == null && Comision == null && Impuesto == null)
                modelState.AddModelError("Parámetros","Debe proporcionar al menos un campo para actualizar.");

            return modelState.IsValid;
        }
    }
}
