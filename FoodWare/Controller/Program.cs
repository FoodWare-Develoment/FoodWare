using FoodWare.View.Forms;
using Microsoft.Extensions.Configuration; // Para poder leer la configuraci�n.
using System.IO;                        // Para obtener la ruta del archivo.

namespace FoodWare.Controller
{
    internal static class Program
    {
        // Una propiedad p�blica y est�tica para guardar nuestra configuraci�n.
        // Esto permite que cualquier otra parte del c�digo (como el LoginController)
        // pueda acceder a ella f�cilmente usando "Program.Configuration".
        public static IConfiguration Configuration { get; private set; } = null!;

        /// <summary>
        ///  Punto de entrada principal de la aplicaci�n.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Este bloque construye la configuraci�n al inicio de la app.
            // 1. Crea un constructor de configuraci�n.
            var builder = new ConfigurationBuilder()
                // 2. Le dice que busque los archivos en el directorio actual (donde est� el .exe).
                .SetBasePath(Directory.GetCurrentDirectory())
                // 3. Le ordena que encuentre y lea el archivo "appsettings.json".
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // 4. Construye la configuraci�n y la guarda en nuestra propiedad est�tica.
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