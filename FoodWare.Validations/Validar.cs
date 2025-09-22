namespace FoodWare.Validations
{
    /// <summary>
    /// Proporciona métodos de validación estáticos para la entrada
    /// del usuario en los formularios de FoodWare.
    /// </summary>
    public static class Validar
    {
        /// <summary>
        /// Verifica si un string está vacío, nulo o es solo espacio en blanco.
        /// </summary>
        /// <param name="texto">El texto a validar.</param>
        /// <returns>True si está vacío, false si tiene contenido.</returns>
        public static bool EsTextoVacio(string texto)
        {
            return string.IsNullOrWhiteSpace(texto);
        }

        /// <summary>
        /// Verifica si un string es un número entero (int) válido y positivo (mayor que cero).
        /// Ideal para validar Stock.
        /// </summary>
        /// <param name="texto">El texto a validar.</param>
        /// <returns>True si es un entero positivo, false si no.</returns>
        public static bool EsEnteroPositivo(string texto)
        {
            // Usamos TryParse, como mencionaste. Es la mejor práctica.
            if (int.TryParse(texto, out int resultado))
            {
                // Es un número, ¿pero es positivo?
                return resultado > 0;
            }
            // No se pudo convertir a int
            return false;
        }

        /// <summary>
        /// Verifica si un string es un número decimal válido y positivo (mayor que cero).
        /// Ideal para validar Precios.
        /// </summary>
        /// <param name="texto">El texto a validar.</param>
        /// <returns>True si es un decimal positivo, false si no.</returns>
        public static bool EsDecimalPositivo(string texto)
        {
            // Usamos TryParse para decimal
            if (decimal.TryParse(texto, out decimal resultado))
            {
                // Es un número, ¿pero es positivo?
                return resultado > 0;
            }
            // No se pudo convertir a decimal
            return false;
        }

        /// <summary>
        /// (Versión extendida) Verifica si es un decimal positivo y devuelve el valor.
        /// Esto te ahorra hacer TryParse dos veces en el formulario.
        /// </summary>
        /// <param name="texto">El texto a validar.</param>
        /// <param name="resultado">El valor decimal convertido (si es exitoso).</param>
        /// <returns>True si la validación es exitosa, false si no.</returns>
        public static bool EsDecimalPositivo(string texto, out decimal resultado)
        {
            if (decimal.TryParse(texto, out resultado))
            {
                return resultado > 0;
            }
            return false;
        }
    }
}