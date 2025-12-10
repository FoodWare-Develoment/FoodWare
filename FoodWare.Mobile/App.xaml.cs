namespace FoodWare.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Lógica de inicio condicional:
            // Si existe la llave "UserId" en las preferencias, el usuario ya inició sesión.
            if (Preferences.ContainsKey("UserId"))
            {
                return new Window(new AppShell());
            }
            else
            {
                // Si no, lo mandamos a loguearse.
                return new Window(new LoginPage());
            }
        }
    }
}