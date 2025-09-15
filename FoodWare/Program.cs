namespace FoodWare
{
    internal static class Program
    {
        /// <summary>
        ///  Punto de entrada principal de la aplicaci�n.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. Inicializa la configuraci�n de la app (fuentes, DPI, etc.)
            ApplicationConfiguration.Initialize();

            // 2. Creamos y mostramos el LoginForm primero como un Di�logo (ShowDialog).
            // La ejecuci�n del c�digo se detiene aqu� hasta que el LoginForm se cierre.
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            // 3. Revisamos el resultado DESPU�S de que se cerr� el login.
            // (El login nos devuelve "DialogResult.OK" si el usuario y pass fueron correctos).
            if (loginForm.DialogResult == DialogResult.OK)
            {
                // 4. Si el login fue exitoso, lanzamos la aplicaci�n principal (FormMain).
                Application.Run(new FormMain());
            }

            // Si el resultado NO es OK (el usuario cerr� la ventana [X]), 
            // la funci�n Main() termina y la aplicaci�n se cierra limpiamente.
        }
    }
}