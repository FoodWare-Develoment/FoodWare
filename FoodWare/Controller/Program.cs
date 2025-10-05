using FoodWare.View.Forms;
using Microsoft.Extensions.Configuration; // Para poder leer la configuración.
using System.IO;                        // Para obtener la ruta del archivo.

namespace FoodWare.Controller
{
    internal static class Program
    {
        // Una propiedad pública y estática para guardar nuestra configuración.
        // Esto permite que cualquier otra parte del código (como el LoginController)
        // pueda acceder a ella fácilmente usando "Program.Configuration".
        public static IConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        ///  Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Este bloque construye la configuración al inicio de la app.
            // 1. Crea un constructor de configuración.
            var builder = new ConfigurationBuilder()
                // 2. Le dice que busque los archivos en el directorio actual (donde está el .exe).
                .SetBasePath(Directory.GetCurrentDirectory())
                // 3. Le ordena que encuentre y lea el archivo "appsettings.json".
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // 4. Construye la configuración y la guarda en nuestra propiedad estática.
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();

            LoginForm loginForm = new();
            loginForm.ShowDialog();

            if (loginForm.DialogResult == DialogResult.OK)
            {
                Application.Run(new FormMain());
            }
        }
    }
}