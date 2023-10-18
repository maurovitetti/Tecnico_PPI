namespace PPI.Models
{
    /// <summary>
    /// Representa un estado de una orden en el sistema.
    /// </summary>
    public class EstadoOrden
    {
        /// <summary>
        /// Obtiene o establece el identificador único del estado de orden.
        /// </summary>
        public int IdEstadoOrden { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del estado de orden.
        /// </summary>
        public string DescripcionEstado { get; set; } = null!;
    }
}
