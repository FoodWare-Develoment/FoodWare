using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;
using FoodWare.Controller.Logic;
using FoodWare.View.Helpers;
using FoodWare.Controller.Exceptionss;

namespace FoodWare.View.UserControls
{
    public partial class UC_Ventas : UserControl
    {
        // --- Constantes de Literales (SonarLint S1192) ---
        private const string TituloAviso = "Aviso";
        private const string TituloConfirmar = "Confirmar";
        private const string TituloError = "Error";
        private const string TituloErrorConexion = "Error de Conexión";
        private const string TituloExito = "Éxito";
        private const string TituloStockInsuficiente = "Stock Insuficiente";
        private const string TituloComandaVacia = "Comanda Vacía";
        private const string DefaultFormaPago = "Efectivo";

        private const string FiltroTodos = "Todos";

        // --- Controladores ---
        private readonly MenuController _menuController;
        private readonly VentasController _ventasController;

        // --- Listas de Estado ---
        private List<Platillo> _listaPlatillosCompleta;
        private List<Platillo> _listaPlatillosFiltrada;
        private readonly BindingList<DetalleVenta> _comandaActual;

        // --- Variables de Estado de UI ---
        private string _formaDePagoSeleccionada = DefaultFormaPago;
        private string _categoriaSeleccionada = FiltroTodos;

        // --- Timer para Debounce ---
        private readonly System.Windows.Forms.Timer _debounceTimer;

        public UC_Ventas()
        {
            InitializeComponent();
            AplicarEstilos();

            IPlatilloRepository platilloRepo = new PlatilloSqlRepository();
            _menuController = new MenuController(platilloRepo);

            _ventasController = new VentasController(
                new VentaSqlRepository(),
                new RecetaSqlRepository(),
                new ProductoSqlRepository(),
                new MovimientoSqlRepository()
            );

            _listaPlatillosCompleta = [];
            _listaPlatillosFiltrada = [];
            _comandaActual = [];
            dgvComanda.DataSource = _comandaActual;

            // --- Configuración del Debounce (400ms) ---
            _debounceTimer = new System.Windows.Forms.Timer
            {
                Interval = 400
            };
            _debounceTimer.Tick += DebounceTimer_Tick;

            // --- Conexión de Eventos ---
            this.btnRegistrarVenta.Click += BtnRegistrarVenta_Click;
            this.btnEliminarPlatillo.Click += BtnEliminarPlatillo_Click;
            this.btnQtyDisminuir.Click += BtnQtyDisminuir_Click;
            this.btnQtyAumentar.Click += BtnQtyAumentar_Click;
            this.btnEfectivo.Click += BtnEfectivo_Click;
            this.btnTarjeta.Click += BtnTarjeta_Click;

            this.txtBusquedaTPV.TextChanged += TxtBusquedaTPV_TextChanged;
            this.txtBusquedaTPV.KeyDown += TxtBusquedaTPV_KeyDown;
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            this.tlpPrincipal.BackColor = EstilosApp.ColorFondo;
            this.panelComanda.BackColor = EstilosApp.ColorFondo;
            this.panelMenuSeleccion.BackColor = EstilosApp.ColorFondo;

            this.flpCategorias.BackColor = Color.White;
            this.flpPlatillos.BackColor = Color.White;
            this.flpGestionTicket.BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloDataGridView(dgvComanda);
            EstilosApp.EstiloBotonAccionPrincipal(btnRegistrarVenta);

            EstilosApp.EstiloBotonModuloAlerta(btnEliminarPlatillo);
            EstilosApp.EstiloBotonModuloSecundario(btnQtyDisminuir);
            EstilosApp.EstiloBotonModuloSecundario(btnQtyAumentar);

            lblTotal.ForeColor = EstilosApp.ColorTextoOscuro;
            lblTotal.BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloTextBoxModulo(txtBusquedaTPV);
            flpFormaPago.BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloBotonModulo(btnEfectivo);
            EstilosApp.EstiloBotonModuloSecundario(btnTarjeta);
        }

        private async void UC_Ventas_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                await CargarDatosInicialesAsync();
                ActualizarBotonesPago();
            }
        }

        // --- MÉTODOS DE LÓGICA PRINCIPAL ---

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _listaPlatillosCompleta = await _menuController.CargarPlatillosAsync();
                _listaPlatillosFiltrada = [.. _listaPlatillosCompleta];

                PoblarBotonesCategorias();
                ActualizarFiltroPlatillos(usarDebounce: false);
                ConfigurarGridComanda();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar menú de ventas: {ex.Message}");
                MessageBox.Show("Error al cargar el menú. Contacte al administrador.", TituloErrorConexion, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void PoblarBotonesCategorias()
        {
            flpCategorias.Controls.Clear();

            // Usamos la constante FiltroTodos
            Button btnTodos = new() { Text = FiltroTodos, Height = 50, Width = 100, Tag = FiltroTodos };
            EstilosApp.EstiloBotonModulo(btnTodos);
            btnTodos.Click += BotonCategoria_Click;
            flpCategorias.Controls.Add(btnTodos);

            var categorias = _listaPlatillosCompleta
                                .Select(p => p.Categoria)
                                .Distinct()
                                .OrderBy(c => c);

            foreach (var categoria in categorias)
            {
                Button btnCat = new() { Text = categoria, Height = 50, Width = 100, Tag = categoria };
                EstilosApp.EstiloBotonModuloSecundario(btnCat);
                btnCat.Click += BotonCategoria_Click;
                flpCategorias.Controls.Add(btnCat);
            }
        }

        private void PoblarBotonesPlatillos(List<Platillo> platillos)
        {
            flpPlatillos.SuspendLayout();
            flpPlatillos.Controls.Clear();

            foreach (var platillo in platillos)
            {
                Button btnPlatillo = new()
                {
                    Text = $"{platillo.Nombre}\n{platillo.PrecioVenta:C}",
                    Width = 130,
                    Height = 80,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Tag = platillo
                };
                EstilosApp.EstiloBotonModulo(btnPlatillo);
                btnPlatillo.Click += BotonPlatillo_Click;
                flpPlatillos.Controls.Add(btnPlatillo);
            }
            flpPlatillos.ResumeLayout();
        }

        private void ConfigurarGridComanda()
        {
            dgvComanda.DataSource = _comandaActual;
            if (dgvComanda.Columns["IdDetalleVenta"] is DataGridViewColumn colDetalle) colDetalle.Visible = false;
            if (dgvComanda.Columns["IdVenta"] is DataGridViewColumn colVenta) colVenta.Visible = false;
            if (dgvComanda.Columns["IdPlatillo"] is DataGridViewColumn colPlatillo) colPlatillo.Visible = false;
            if (dgvComanda.Columns["NombrePlatillo"] is DataGridViewColumn colNombre)
            {
                colNombre.HeaderText = "Platillo";
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvComanda.Columns["PrecioUnitario"] is DataGridViewColumn colPrecio) colPrecio.HeaderText = "Precio";
        }

        private void ActualizarTotal()
        {
            decimal total = _comandaActual.Sum(detalle => detalle.Cantidad * detalle.PrecioUnitario);
            lblTotal.Text = total.ToString("C");
        }

        private void ActualizarBotonesPago()
        {
            EstilosApp.EstiloBotonModuloSecundario(btnEfectivo);
            EstilosApp.EstiloBotonModuloSecundario(btnTarjeta);

            if (_formaDePagoSeleccionada == "Efectivo")
            {
                EstilosApp.EstiloBotonModulo(btnEfectivo);
            }
            else if (_formaDePagoSeleccionada == "Tarjeta")
            {
                EstilosApp.EstiloBotonModulo(btnTarjeta);
            }
        }

        private void ActualizarFiltroPlatillos(bool usarDebounce = false)
        {
            if (usarDebounce)
            {
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
            else
            {
                EjecutarFiltro();
            }
        }

        private void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();
            EjecutarFiltro();
        }

        private void EjecutarFiltro()
        {
            try
            {
                string filtroBusqueda = txtBusquedaTPV.Text.Trim();
                var listaTemp = _listaPlatillosCompleta;

                // 1. Filtrar por Categoría
                if (_categoriaSeleccionada != FiltroTodos)
                {
                    listaTemp = [.. listaTemp.Where(p => p.Categoria == _categoriaSeleccionada)];
                }

                // 2. Filtrar por Búsqueda
                if (!string.IsNullOrWhiteSpace(filtroBusqueda))
                {
                    listaTemp = [.. listaTemp.Where(p => p.Nombre.Contains(filtroBusqueda, StringComparison.OrdinalIgnoreCase))];
                }

                _listaPlatillosFiltrada = listaTemp;
                PoblarBotonesPlatillos(_listaPlatillosFiltrada);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al filtrar: {ex.Message}");
            }
        }

        private void AgregarPlatilloAComanda(Platillo platillo)
        {
            var detalleExistente = _comandaActual.FirstOrDefault(d => d.IdPlatillo == platillo.IdPlatillo);

            if (detalleExistente != null)
            {
                detalleExistente.Cantidad++;
                _comandaActual.ResetBindings();
            }
            else
            {
                var nuevoDetalle = new DetalleVenta
                {
                    IdPlatillo = platillo.IdPlatillo,
                    NombrePlatillo = platillo.Nombre,
                    Cantidad = 1,
                    PrecioUnitario = platillo.PrecioVenta
                };
                _comandaActual.Add(nuevoDetalle);
            }
            ActualizarTotal();
        }

        // --- EVENT HANDLERS (CLICS DE BOTONES) ---

        private void BotonCategoria_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not string categoria) return;

            _categoriaSeleccionada = categoria;

            foreach (Control c in flpCategorias.Controls)
            {
                if (c is Button b) EstilosApp.EstiloBotonModuloSecundario(b);
            }
            EstilosApp.EstiloBotonModulo(btn);

            ActualizarFiltroPlatillos(usarDebounce: false);
        }

        private void BotonPlatillo_Click(object? sender, EventArgs e)
        {
            Button btn = (Button)sender!;
            Platillo platillo = (Platillo)btn.Tag!;

            AgregarPlatilloAComanda(platillo);
        }

        private void BtnQtyAumentar_Click(object? sender, EventArgs e)
        {
            if (dgvComanda.CurrentRow != null && dgvComanda.CurrentRow.DataBoundItem is DetalleVenta detalle)
            {
                detalle.Cantidad++;
                _comandaActual.ResetBindings();
                ActualizarTotal();
            }
            else
            {
                MessageBox.Show("Selecciona un platillo de la comanda primero.", TituloAviso, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnQtyDisminuir_Click(object? sender, EventArgs e)
        {
            if (dgvComanda.CurrentRow != null && dgvComanda.CurrentRow.DataBoundItem is DetalleVenta detalle)
            {
                if (detalle.Cantidad > 1)
                {
                    detalle.Cantidad--;
                }
                else
                {
                    var confirm = MessageBox.Show($"¿Quitar '{detalle.NombrePlatillo}' de la comanda?", TituloConfirmar, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        _comandaActual.Remove(detalle);
                    }
                }
                _comandaActual.ResetBindings();
                ActualizarTotal();
            }
            else
            {
                MessageBox.Show("Selecciona un platillo de la comanda primero.", TituloAviso, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEliminarPlatillo_Click(object? sender, EventArgs e)
        {
            if (dgvComanda.CurrentRow != null && dgvComanda.CurrentRow.DataBoundItem is DetalleVenta detalle)
            {
                var confirm = MessageBox.Show($"¿Eliminar '{detalle.NombrePlatillo}' (Cantidad: {detalle.Cantidad}) de la comanda?", TituloConfirmar, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    _comandaActual.Remove(detalle);
                    ActualizarTotal();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un platillo de la comanda para eliminar.", TituloAviso, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEfectivo_Click(object? sender, EventArgs e)
        {
            _formaDePagoSeleccionada = "Efectivo";
            ActualizarBotonesPago();
        }

        private void BtnTarjeta_Click(object? sender, EventArgs e)
        {
            _formaDePagoSeleccionada = "Tarjeta";
            ActualizarBotonesPago();
        }

        private void TxtBusquedaTPV_TextChanged(object? sender, EventArgs e)
        {
            ActualizarFiltroPlatillos(usarDebounce: true);
        }

        private void TxtBusquedaTPV_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (_debounceTimer.Enabled)
                {
                    _debounceTimer.Stop();
                    EjecutarFiltro();
                }

                var primerResultado = _listaPlatillosFiltrada.FirstOrDefault();

                if (primerResultado != null)
                {
                    AgregarPlatilloAComanda(primerResultado);
                    txtBusquedaTPV.Clear();
                }
            }
        }

        private async void BtnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            if (_comandaActual.Count == 0)
            {
                MessageBox.Show("No hay platillos en la comanda para registrar.", TituloComandaVacia, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea registrar esta venta por un total de {lblTotal.Text}?", "Confirmar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnRegistrarVenta.Enabled = false;

                Venta nuevaVenta = new()
                {
                    IdUsuario = UserSession.IdUsuario,
                    FormaDePago = _formaDePagoSeleccionada,
                    TotalVenta = _comandaActual.Sum(d => d.Cantidad * d.PrecioUnitario)
                };

                await _ventasController.RegistrarVentaAsync(nuevaVenta, [.. _comandaActual]);

                MessageBox.Show("¡Venta registrada exitosamente!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _comandaActual.Clear();
                ActualizarTotal();
            }
            catch (StockInsuficienteException stockEx)
            {
                string mensaje = $"{stockEx.Message}\n\n" +
                                 $"Para el platillo: '{stockEx.PlatilloConProblema}'\n" +
                                 $"Solo puede vender un máximo de {stockEx.MaximaCantidadVendible} unidades.";
                MessageBox.Show(mensaje, TituloStockInsuficiente, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al registrar venta: {ex.Message}");
                MessageBox.Show("Ocurrió un error al registrar la venta. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRegistrarVenta.Enabled = true;
            }
        }
    }
}