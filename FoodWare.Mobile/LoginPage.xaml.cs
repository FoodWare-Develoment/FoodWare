using FoodWare.Shared.Entities;
using System.Net.Http.Json;

namespace FoodWare.Mobile;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _client;

    public LoginPage()
    {
        InitializeComponent();
        var handler = HttpsClientHandlerService.GetPlatformMessageHandler();
        _client = new HttpClient(handler);
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsuario.Text) || string.IsNullOrWhiteSpace(TxtPassword.Text))
        {
            await DisplayAlert("Error", "Ingrese usuario y contraseña", "OK");
            return;
        }

        AiCargando.IsRunning = true;

        try
        {
            var loginReq = new LoginRequest
            {
                Usuario = TxtUsuario.Text,
                Password = TxtPassword.Text
            };

            string url = "https://10.0.2.2:7264/api/auth/login";

            var response = await _client.PostAsJsonAsync(url, loginReq);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (resultado != null)
                {
                    Preferences.Set("UserId", resultado.IdUsuario);
                    Preferences.Set("UserName", resultado.NombreCompleto);

                    var app = Application.Current;
                    if (app != null && app.Windows.Count > 0 && app.Windows[0] != null)
                    {
                        app.Windows[0].Page = new AppShell();
                    }
                    else
                    {
                        await DisplayAlert("Error", "En la página principal de la aplicación.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Acceso Denegado", "Usuario o contraseña incorrectos", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo conectar: {ex.Message}", "OK");
        }
        finally
        {
            AiCargando.IsRunning = false;
        }
    }
}