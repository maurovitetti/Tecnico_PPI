namespace PPI.Models
{
    /// <summary>
    /// Representa un tipo de activo en el sistema.
    /// </summary>
    public class TipoActivo
    {
        /// <summary>
        /// Obtiene o establece el identificador único del tipo de activo.
        /// </summary>
        public int IdTipoActivo { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del tipo de activo.
        /// </summary>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece la comisión asociada al tipo de activo.
        /// </summary>
        public decimal Comision { get; set; }

        /// <summary>
        /// Obtiene o establece el impuesto asociado al tipo de activo.
        /// </summary>
        public decimal Impuesto { get; set; }
    }
}
