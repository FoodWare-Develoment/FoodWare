using FoodWare.View.Forms;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FoodWare.Controller
{
    internal static class Program
    {
        public static IConfiguration Configuration { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

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