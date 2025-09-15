namespace FoodWare
{
    internal static class Program
    {
        /// <summary>
        ///  Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. Inicializa la configuración de la app (fuentes, DPI, etc.)
            ApplicationConfiguration.Initialize();

            // 2. Creamos y mostramos el LoginForm primero como un Diálogo (ShowDialog).
            // La ejecución del código se detiene aquí hasta que el LoginForm se cierre.
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            // 3. Revisamos el resultado DESPUÉS de que se cerró el login.
            // (El login nos devuelve "DialogResult.OK" si el usuario y pass fueron correctos).
            if (loginForm.DialogResult == DialogResult.OK)
            {
                // 4. Si el login fue exitoso, lanzamos la aplicación principal (FormMain).
                Application.Run(new FormMain());
            }

            // Si el resultado NO es OK (el usuario cerró la ventana [X]), 
            // la función Main() termina y la aplicación se cierra limpiamente.
        }
    }
}