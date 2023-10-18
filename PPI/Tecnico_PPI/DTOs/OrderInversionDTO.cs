using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPI.DTOs
{
    public class NewOrdenInversionDTO
    {
        /// <summary>
        /// Obtiene o establece el ID del tipo de activo. En el caso de no saber que ID usar, puede consultar el método GET de TipoActivoController
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(2)]
        public int IdTipoActivo { get; set; }

        /// <summary>
        /// Obtiene o establece el ID de la cuenta. (Actualmente es obligatorio, pero no hay lógica creada que haga una conexión con una tabla que tenga información sobre la cuenta)
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(9405)]
        public int IdCuenta { get; set; }

        /// <summary>
        /// Obtiene o establece el ID del activo. En caso de no saber que ID usar, puede consultar el método GET de su respectivo controller
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(1)]
        public int IdActivo { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de activos.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que cero.")]
        [DefaultValue(1)]
        public int CantidadActivos { get; set; }

        /// <summary>
        /// Parámetro opcional. Se debe enviar cuando el tipo de activo no sea una Acción.
        /// </summary>
        [DefaultValue(0)]
        public decimal? PrecioUnitario { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de operación (Compra 'C' / Venta 'V').
        /// </summary>
        [Required]
        [RegularExpression("^[CV]$", ErrorMessage = "La operación debe ser 'C' o 'V")]
        [DefaultValue("C")]
        public char Operacion { get; set; }

        /// <summary>
        /// Validacion del modelo de la clase NewOrdenInversionDTO
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool IsValid(ModelStateDictionary modelState)
        {
            if (IdTipoActivo < 0)
                modelState.AddModelError("IdTipoActivo", "El campo IdTipoActivo debe ser un valor positivo.");

            if (IdActivo < 0)
                modelState.AddModelError("IdActivo", "El campo IdActivo debe ser un valor positivo.");

            if (IdCuenta < 0)
                modelState.AddModelError("IdCuenta", "El campo IdCuenta debe ser un valor positivo.");

            if (CantidadActivos < 0)
                modelState.AddModelError("CantidadActivos", "El campo CantidadActivos debe ser un valor positivo.");

            if (PrecioUnitario < 0)
                modelState.AddModelError("PrecioUnitario", "El campo PrecioUnitario debe ser un valor positivo.");

            if (Operacion != 'C' && Operacion != 'V')
                modelState.AddModelError("Operacion", "El campo Operacion acepta los siguientes valores: (Compra 'C' / Venta 'V')");

            return modelState.IsValid;
        }
    }
}

