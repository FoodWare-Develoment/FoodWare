using FoodWare.Shared.Entities;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace FoodWare.Mobile;

public partial class MainPage : ContentPage
{
    private const string ApiUrlPlatillos = "https://10.0.2.2:7264/api/platillos";

    private static readonly Color ColorPrimario = Color.FromArgb("#2C3E50");
    private readonly HttpClient _client;

    private List<Platillo> _todosLosPlatillos = [];
    private readonly ObservableCollection<Platillo> _carrito = [];

    public MainPage()
    {
        InitializeComponent();
        var handler = HttpsClientHandlerService.GetPlatformMessageHandler();
        _client = new HttpClient(handler);
        _ = CargarDatos();
    }

    private async Task CargarDatos()
    {
        try
        {
            var lista = await _client.GetFromJsonAsync<List<Platillo>>(ApiUrlPlatillos);

            if (lista != null)
            {
                _todosLosPlatillos = lista;
                CvPlatillos.ItemsSource = _todosLosPlatillos;
                GenerarCategorias();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Conexión",
                $"Verifica que la API esté corriendo.\n\nError: {ex.Message}",
                "OK");
        }
    }

    private void GenerarCategorias()
    {
        HslCategorias.Children.Clear();
        var btnTodos = CrearBotonCategoria("Todos", true);
        btnTodos.Clicked += (s, e) => FiltrarPorCategoria("Todos");
        HslCategorias.Children.Add(btnTodos);

        var categorias = _todosLosPlatillos
            .Select(p => p.Categoria)
            .Distinct()
            .OrderBy(c => c);

        foreach (var cat in categorias)
        {
            var btn = CrearBotonCategoria(cat, false);
            btn.Clicked += (s, e) => FiltrarPorCategoria(cat);
            HslCategorias.Children.Add(btn);
        }
    }

    private static Button CrearBotonCategoria(string texto, bool esActivo)
    {
        return new Button
        {
            Text = texto,
            BackgroundColor = esActivo ? ColorPrimario : Colors.White,
            TextColor = esActivo ? Colors.White : ColorPrimario,
            BorderColor = ColorPrimario,
            BorderWidth = 1,
            CornerRadius = 20,
            HeightRequest = 40,
            Padding = new Thickness(15, 0)
        };
    }

    private void FiltrarPorCategoria(string categoria)
    {
        foreach (var child in HslCategorias.Children)
        {
            if (child is Button btn)
            {
                bool esElSeleccionado = btn.Text == categoria;
                btn.BackgroundColor = esElSeleccionado ? ColorPrimario : Colors.White;
                btn.TextColor = esElSeleccionado ? Colors.White : ColorPrimario;
            }
        }

        if (categoria == "Todos")
        {
            CvPlatillos.ItemsSource = _todosLosPlatillos;
        }
        else
        {
            CvPlatillos.ItemsSource = _todosLosPlatillos.Where(p => p.Categoria == categoria).ToList();
        }
    }

    private void OnPlatilloTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Platillo platilloSeleccionado)
        {
            if (sender is VisualElement visualElement)
            {
                visualElement.ScaleTo(0.95, 50).ContinueWith(t => visualElement.ScaleTo(1.0, 50));
            }
            _carrito.Add(platilloSeleccionado);
            ActualizarResumenPedido();
        }
    }

    private void ActualizarResumenPedido()
    {
        decimal total = _carrito.Sum(p => p.PrecioVenta);
        LblTotalPedido.Text = total.ToString("C2");
        BtnVerPedido.Text = $"Ver ({_carrito.Count})";
    }

    private async void BtnVerPedido_Clicked(object sender, EventArgs e)
    {
        if (_carrito.Count == 0) return;

        string resumen = string.Join("\n", _carrito.Select(p => $"- {p.Nombre}"));
        bool confirmar = await DisplayAlert("Confirmar Comanda", $"{resumen}\n\nTotal: {LblTotalPedido.Text}", "Enviar", "Cancelar");

        if (!confirmar) return;

        try
        {
            var detallesAgrupados = _carrito
                .GroupBy(p => p.IdPlatillo)
                .Select(g => new DetalleVenta
                {
                    IdPlatillo = g.Key,
                    Cantidad = g.Count(),
                    PrecioUnitario = g.First().PrecioVenta,
                    NombrePlatillo = g.First().Nombre
                }).ToList();

            int idUsuarioActual = Preferences.Get("UserId", 0);

            var ventaRequest = new VentaRequest
            {
                IdUsuario = idUsuarioActual,
                FormaDePago = "Efectivo",
                Detalles = detallesAgrupados
            };

            string urlVentas = "https://10.0.2.2:7264/api/ventas";

            var response = await _client.PostAsJsonAsync(urlVentas, ventaRequest);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("¡Éxito!", "La comanda se envió a cocina y se guardó en BD.", "OK");
                _carrito.Clear();
                ActualizarResumenPedido();
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error en el Servidor", $"No se pudo registrar: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Conexión", $"El celular no pudo llegar a la API.\n\n{ex.Message}", "OK");
        }
    }
}