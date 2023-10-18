namespace PPI.Models
{
    /// <summary>
    /// Representa un FCI en el sistema.
    /// </summary>
    public class FCI
    {
        /// <summary>
        /// Obtiene o establece el identificador único del FCI.
        /// </summary>
        public int IdFCI { get; set; }

        /// <summary>
        /// Obtiene o establece el símbolo de cotización del FCI (ticker).
        /// </summary>
        public string Ticker { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el nombre del FCI.
        /// </summary>
        public string Nombre { get; set; } = null!;
    }
}
