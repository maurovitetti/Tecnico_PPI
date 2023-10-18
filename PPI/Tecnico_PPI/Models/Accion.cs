namespace PPI.Models
{
    /// <summary>
    /// Representa una acción en el sistema.
    /// </summary>
    public class Accion
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la acción.
        /// </summary>
        public int IdAccion { get; set; }

        /// <summary>
        /// Obtiene o establece el símbolo de cotización de la acción (ticker).
        /// </summary>
        public string Ticker { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el nombre de la acción.
        /// </summary>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el precio unitario de la acción.
        /// </summary>
        public decimal PrecioUnitario { get; set; }
    }
}
