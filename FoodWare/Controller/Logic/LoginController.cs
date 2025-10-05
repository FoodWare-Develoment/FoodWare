using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace FoodWare.Controller.Logic
{
    public class LoginController
    {
        private readonly string connectionString;

        public LoginController()
        {
            connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public bool ValidarLogin(string username, string password)
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ContraseñaHash FROM Usuarios WHERE NombreUsuario = @username AND Activo = 1";
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var storedPasswordHash = command.ExecuteScalar() as string;

                        if (string.IsNullOrEmpty(storedPasswordHash))
                        {
                            return false;
                        }

                        storedPasswordHash = storedPasswordHash.Trim();

                        // Verifica el hash que vino de la base de datos. Esto es lo que está dando FALSE.
                        bool verificacionDeBD = BCrypt.Net.BCrypt.Verify("123", storedPasswordHash);

                        return verificacionDeBD;
                    }
                }
                catch { return false; }
            }
        }
    }
}