namespace PPI.Models
{
    /// <summary>
    /// Representa un Bono en el sistema.
    /// </summary>
    public class Bono
    {
        /// <summary>
        /// Obtiene o establece el identificador único del Bono.
        /// </summary>
        public int IdBono { get; set; }

        /// <summary>
        /// Obtiene o establece el símbolo de cotización del Bono (ticker).
        /// </summary>
        public string Ticker { get; set; } = null!;

        /// <summary>
        /// Obtiene o establece el nombre del Bono.
        /// </summary>
        public string Nombre { get; set; } = null!;
    }
}
