using System.Globalization; // Necesario para TryParse

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
        public static bool EsTextoVacio(string texto)
        {
            return string.IsNullOrWhiteSpace(texto);
        }

        /// <summary>
        /// Verifica si un string es un número entero (int) válido y positivo
        /// </summary>
        public static bool EsEnteroPositivo(string texto, out int resultado)
        {
            // Usamos TryParse DIRECTAMENTE sobre el parámetro 'out'
            // 'resultado' se volverá 0 si TryParse falla.
            if (int.TryParse(texto, out resultado))
            {
                // Es un número. ¿Es positivo?
                return resultado > 0;
            }
            // No se pudo convertir. 'resultado' ya es 0.
            return false;
        }

        /// <summary>
        /// Verifica si un string es un número decimal válido y positivo
        /// </summary>
        public static bool EsDecimalPositivo(string texto, out decimal resultado)
        {
            // 'resultado' se volverá 0.0m si TryParse falla.
            if (decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out resultado))
            {
                // Es un número. ¿Es positivo?
                return resultado > 0;
            }
            // No se pudo convertir. 'resultado' ya es 0.0m.
            return false;
        }
    }
}