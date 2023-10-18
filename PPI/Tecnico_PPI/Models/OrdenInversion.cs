using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPI.Models
{
    /// <summary>
    /// Representa una orden de inversión en el sistema.
    /// </summary>
    public class OrdenInversion
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la orden de inversión.
        /// </summary>
        public int IdOrdenInversion { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la cuenta asociada a la orden de inversión.
        /// </summary>
        public int IdCuenta { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del estado de la orden de inversión.
        /// </summary>
        public int IdEstado { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del tipo de activo asociado a la orden de inversión.
        /// </summary>
        public int IdTipoActivo { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del activo asociado a la orden de inversión.
        /// </summary>
        public int IdActivo { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de activos involucrados en la orden de inversión.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de operación (Compra 'C' / Venta 'V') de la orden de inversión.
        /// </summary>
        public char Operacion { get; set; }

        /// <summary>
        /// Obtiene o establece el precio unitario del activo.
        /// </summary>
        public decimal? PrecioUnitario { get; set; }

        /// <summary>
        /// Obtiene o establece el monto total involucrado en la orden de inversión.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoTotal { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la orden de inversión.
        /// </summary>
        public virtual EstadoOrden EstadoOrden { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el tipo de activo asociado a la orden de inversión.
        /// </summary>
        public TipoActivo TipoActivo { get; set; } = null!;

        /// <summary>
        /// btiene o establece el FCI
        /// </summary>
        public FCI? FCI { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el bono
        /// </summary>
        public Bono? Bono { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece la acción
        /// </summary>
        public Accion? Accion { get; set; } = null!;

        /// <summary>
        /// Clona las variable de una orden de inversión, menos los tres posibles activos
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static OrdenInversion ClonarOrden(OrdenInversion original)
        {
            return new OrdenInversion
            {
                IdOrdenInversion = original.IdOrdenInversion,
                IdCuenta = original.IdCuenta,
                IdEstado = original.IdEstado,
                IdTipoActivo = original.IdTipoActivo,
                IdActivo = original.IdActivo,
                Cantidad = original.Cantidad,
                Operacion = original.Operacion,
                PrecioUnitario = original.PrecioUnitario,
                MontoTotal = original.MontoTotal,
                EstadoOrden = original.EstadoOrden,
                TipoActivo = original.TipoActivo
            };
        }
    }
}
