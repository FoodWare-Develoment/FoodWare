using FoodWare.View.Forms;
using Microsoft.Extensions.Configuration; // Para leer la configuraci�n.
using System;
using System.Windows.Forms;

namespace FoodWare.Controller
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicaci�n.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Aqu� puedes inicializar configuraciones o servicios si es necesario.
            Application.Run(new FormMain());
        }
    }
}