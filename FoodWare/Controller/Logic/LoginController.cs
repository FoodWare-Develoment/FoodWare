using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System;
using System.Diagnostics;

namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// Gestiona la lógica de negocio para la autenticación de usuarios,
    /// actuando como intermediario entre la Vista (LoginForm) y el Modelo (Base de Datos).
    /// </summary>
    public class LoginController
    {
        private readonly string connectionString;

        /// <summary>
        /// Constructor: Se ejecuta una vez cuando se crea el LoginController.
        /// Su función es inicializar la cadena de conexión desde el archivo de configuración.
        /// </summary>
        public LoginController()
        {
            // Leemos la cadena de conexión desde la configuración (appsettings.json o secrets.json)
            // para poder usarla en los métodos de esta clase.
            connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        /// <summary>
        /// Valida las credenciales de un usuario contra la base de datos de forma segura.
        /// </summary>
        /// <param name="username">El nombre de usuario ingresado en el formulario.</param>
        /// <param name="password">La contraseña en texto plano ingresada en el formulario.</param>
        /// <returns>Un valor del enum LoginResult indicando el resultado de la operación.</returns>
        public LoginResult ValidarLogin(string username, string password)
        {
            // Usamos un bloque 'using' para asegurar que la conexión a la BD se cierre siempre,
            // incluso si ocurre un error, liberando recursos del sistema.
            using var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                // Preparamos una consulta SQL parametrizada. Usar "@username" es crucial
                // para prevenir ataques de seguridad conocidos como Inyección SQL.
                string query = "SELECT ContraseñaHash FROM Usuarios WHERE NombreUsuario = @username AND Activo = 1";

                using var command = new SqlCommand(query, connection);
                // Asignamos el valor del parámetro de forma segura.
                command.Parameters.AddWithValue("@username", username);

                // ExecuteScalar es muy eficiente aquí: devuelve solo el primer valor de la primera fila
                // (nuestro hash) o null si la consulta no encuentra ninguna coincidencia.
                var storedPasswordHash = command.ExecuteScalar() as string;

                // Si la consulta no devolvió nada, el usuario no existe o no está activo.
                if (string.IsNullOrEmpty(storedPasswordHash))
                {
                    return LoginResult.CredencialesInvalidas;
                }

                // Como buena práctica, limpiamos el hash por si la BD tuviera espacios en blanco.
                storedPasswordHash = storedPasswordHash.Trim();

                // Usamos la librería BCrypt para comparar de forma segura la contraseña que escribió
                // el usuario ('password') con el hash que obtuvimos de la base de datos.
                bool esValido = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                // Devolvemos el resultado correspondiente basado en si la verificación fue exitosa.
                return esValido ? LoginResult.Exitoso : LoginResult.CredencialesInvalidas;
            }
            catch (Exception ex)
            {
                // Si cualquier cosa dentro del 'try' falla (ej. la BD está offline), se captura la excepción.
                // Registramos el detalle del error en la consola de depuración de Visual Studio.
                Debug.WriteLine($"Error de base de datos al validar login: {ex.Message}");
                // Devolvemos un estado de error específico para que la interfaz sepa que el problema es del sistema.
                return LoginResult.ErrorDeBaseDeDatos;
            }
        }
    }
}