using FoodWare.View.Forms;

namespace FoodWare.Controller
{
    /// <summary>
    /// Resultado de la validación del login.
    /// </summary>
    public enum LoginResult
    {
        Exitoso,
        CredencialesInvalidas,
        ErrorDeBaseDeDatos
    }

    public class LoginController
    {
        public LoginResult ValidarLogin(string usuario, string contraseña)
        {
            // Implementación de validación de login
            return LoginResult.Exitoso; // Ejemplo de retorno
        }
    }
}