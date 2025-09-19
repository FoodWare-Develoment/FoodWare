using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// Contiene la lógica de negocio para operaciones de autenticación.
    /// </summary>
    public class LoginController
    {
        /// <summary>
        /// Valida las credenciales del usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario ingresado.</param>
        /// <param name="password">La contraseña ingresada.</param>
        /// <returns>True si el login es válido, false si no lo es.</returns>
        public bool ValidarLogin(string username, string password)
        {
            //
            // ¡AQUÍ ESTÁ LA LÓGICA QUE MOVIMOS!
            //
            // TODO: Reemplazar esta simulación con la lógica real de validación contra la BD.
            bool loginValido = (username == "admin" && password == "123");

            // El controlador solo devuelve el resultado.
            return loginValido;
        }
    }
}