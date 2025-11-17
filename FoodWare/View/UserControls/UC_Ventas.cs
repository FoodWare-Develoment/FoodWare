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

        // --- Controladores ---
        private readonly MenuController _menuController;
        private readonly VentasController _ventasController;

        // --- Listas de Estado ---
        private List<Platillo> _listaPlatillosCompleta;
        private readonly BindingList<DetalleVenta> _comandaActual;

        // --- Variables de Estado de UI ---
        private string _formaDePagoSeleccionada = DefaultFormaPago;
        private string _categoriaSeleccionada = "Todos";

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
            _comandaActual = [];
            dgvComanda.DataSource = _comandaActual;

            // --- Conexión de Eventos ---
            this.btnRegistrarVenta.Click += BtnRegistrarVenta_Click;
            this.btnEliminarPlatillo.Click += BtnEliminarPlatillo_Click;
            this.btnQtyDisminuir.Click += BtnQtyDisminuir_Click;
            this.btnQtyAumentar.Click += BtnQtyAumentar_Click;
            this.btnEfectivo.Click += BtnEfectivo_Click;
            this.btnTarjeta.Click += BtnTarjeta_Click;
            this.txtBusquedaTPV.TextChanged += TxtBusquedaTPV_TextChanged;
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

            // Estilos botones de pago
            EstilosApp.EstiloBotonModulo(btnEfectivo);
            EstilosApp.EstiloBotonModuloSecundario(btnTarjeta);
        }

        private async void UC_Ventas_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                await CargarDatosInicialesAsync();
                ActualizarBotonesPago(); // Asegura que el botón "Efectivo" inicie verde
            }
        }

        // --- MÉTODOS DE LÓGICA PRINCIPAL ---

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _listaPlatillosCompleta = await _menuController.CargarPlatillosAsync();

                PoblarBotonesCategorias();
                ActualizarFiltroPlatillos(); //Llama al filtro inicial
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

            Button btnTodos = new() { Text = "Todos", Height = 50, Width = 100, Tag = "Todos" };
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
            // Resetear ambos
            EstilosApp.EstiloBotonModuloSecundario(btnEfectivo);
            EstilosApp.EstiloBotonModuloSecundario(btnTarjeta);

            // Activar el seleccionado
            if (_formaDePagoSeleccionada == "Efectivo")
            {
                EstilosApp.EstiloBotonModulo(btnEfectivo); // Verde
            }
            else if (_formaDePagoSeleccionada == "Tarjeta")
            {
                EstilosApp.EstiloBotonModulo(btnTarjeta); // Verde
            }
        }

        private void ActualizarFiltroPlatillos()
        {
            try
            {
                string filtroBusqueda = txtBusquedaTPV.Text.Trim();
                var listaFiltrada = _listaPlatillosCompleta;

                // 1. Filtrar por Categoría
                if (_categoriaSeleccionada != "Todos")
                {
                    listaFiltrada = listaFiltrada.Where(p => p.Categoria == _categoriaSeleccionada).ToList();
                }

                // 2. Filtrar por Búsqueda
                if (!string.IsNullOrWhiteSpace(filtroBusqueda))
                {
                    listaFiltrada = listaFiltrada
                        .Where(p => p.Nombre.Contains(filtroBusqueda, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // 3. Repoblar botones
                PoblarBotonesPlatillos(listaFiltrada);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al filtrar platillos: {ex.Message}");
                // No mostramos MessageBox para no interrumpir al usuario mientras escribe
            }
        }


        // --- EVENT HANDLERS (CLICS DE BOTONES) ---

        private void BotonCategoria_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not string categoria) return;

            _categoriaSeleccionada = categoria;

            // Actualizar estilos de botones de categoría
            foreach (Control c in flpCategorias.Controls)
            {
                if (c is Button b) EstilosApp.EstiloBotonModuloSecundario(b);
            }
            EstilosApp.EstiloBotonModulo(btn); // Activa el presionado

            // Llamamos al helper centralizado
            ActualizarFiltroPlatillos();
        }

        private void BotonPlatillo_Click(object? sender, EventArgs e)
        {
            Button btn = (Button)sender!;
            Platillo platillo = (Platillo)btn.Tag!;

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
            // Llamamos al helper centralizado
            ActualizarFiltroPlatillos();
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