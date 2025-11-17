using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoodWare.Controller.Exceptionss;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.View.Helpers;

namespace FoodWare.View.UserControls
{
    public partial class UC_Ventas : UserControl
    {
        private readonly MenuController _menuController;
        private readonly VentasController _ventasController;
        private List<Platillo> _listaPlatillosCompleta;
        private readonly BindingList<DetalleVenta> _comandaActual;

        public UC_Ventas()
        {
            InitializeComponent();
            AplicarEstilos();

            // 1. Inicializar el controlador de Menu
            IPlatilloRepository platilloRepo = new PlatilloSqlRepository();
            _menuController = new MenuController(platilloRepo);

            // 2. Inicializar el VentasController
            _ventasController = new VentasController(
                new VentaSqlRepository(),
                new RecetaSqlRepository(),
                new ProductoSqlRepository(),
                new MovimientoSqlRepository()
            );

            // 3. Inicializar nuestras listas de estado
            _listaPlatillosCompleta = [];
            _comandaActual = [];

            // 4. Conectar la comanda (BindingList) al DataGridView
            dgvComanda.DataSource = _comandaActual;

            // 5. Conectar los eventos
            this.btnRegistrarVenta.Click += BtnRegistrarVenta_Click;
            this.btnEliminarPlatillo.Click += BtnEliminarPlatillo_Click;
            this.btnQtyDisminuir.Click += BtnQtyDisminuir_Click;
            this.btnQtyAumentar.Click += BtnQtyAumentar_Click;
        }

        /// <summary>
        /// Aplica los estilos de EstilosApp a este UserControl.
        /// </summary>
        private void AplicarEstilos()
        {
            // Fondo general
            this.BackColor = EstilosApp.ColorFondo;
            this.tlpPrincipal.BackColor = EstilosApp.ColorFondo;
            this.panelComanda.BackColor = EstilosApp.ColorFondo;
            this.panelMenuSeleccion.BackColor = EstilosApp.ColorFondo;

            // Paneles de botones (les damos un color blanco para distinguirlos)
            this.flpCategorias.BackColor = Color.White;
            this.flpPlatillos.BackColor = Color.White;
            this.flpGestionTicket.BackColor = EstilosApp.ColorFondo;

            // Grid y Botones principales
            EstilosApp.EstiloDataGridView(dgvComanda);
            EstilosApp.EstiloBotonAccionPrincipal(btnRegistrarVenta);

            // Botones de gestión
            EstilosApp.EstiloBotonModuloAlerta(btnEliminarPlatillo);
            EstilosApp.EstiloBotonModuloSecundario(btnQtyDisminuir);
            EstilosApp.EstiloBotonModuloSecundario(btnQtyAumentar);

            // Total
            lblTotal.ForeColor = EstilosApp.ColorTextoOscuro;
            lblTotal.BackColor = EstilosApp.ColorFondo;
        }

        /// <summary>
        /// Evento de carga: Se dispara cuando el módulo de Ventas se hace visible.
        /// </summary>
        private async void UC_Ventas_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                await CargarDatosInicialesAsync();
            }
        }

        // --- MÉTODOS DE LÓGICA PRINCIPAL ---

        /// <summary>
        /// Tarea principal: Carga platillos y pobla los botones de la UI.
        /// </summary>
        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                await Task.Delay(1);

                _listaPlatillosCompleta = await _menuController.CargarPlatillosAsync();
                PoblarBotonesCategorias();
                PoblarBotonesPlatillos(_listaPlatillosCompleta);
                ConfigurarGridComanda();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar menú de ventas: {ex.Message}");
                MessageBox.Show("Error al cargar el menú. Contacte al administrador.", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Lee las categorías de la lista de platillos y crea los botones en flpCategorias.
        /// </summary>
        private void PoblarBotonesCategorias()
        {
            flpCategorias.Controls.Clear();

            // 1. Botón "Todos"
            Button btnTodos = new()
            {
                Text = "Todos",
                Height = 50,
                Width = 100,
                Tag = "Todos" // Usamos Tag para guardar el valor
            };
            EstilosApp.EstiloBotonModulo(btnTodos); // Estilo verde
            btnTodos.Click += BotonCategoria_Click; // Asignamos el evento
            flpCategorias.Controls.Add(btnTodos);

            // 2. Un botón por cada categoría única (usando LINQ)
            var categorias = _listaPlatillosCompleta
                                .Select(p => p.Categoria)
                                .Distinct()
                                .OrderBy(c => c);

            foreach (var categoria in categorias)
            {
                Button btnCat = new()
                {
                    Text = categoria,
                    Height = 50,
                    Width = 100,
                    Tag = categoria
                };
                EstilosApp.EstiloBotonModuloSecundario(btnCat); // Estilo blanco
                btnCat.Click += BotonCategoria_Click;
                flpCategorias.Controls.Add(btnCat);
            }
        }

        /// <summary>
        /// Recibe una lista de platillos y crea los botones en flpPlatillos.
        /// </summary>
        private void PoblarBotonesPlatillos(List<Platillo> platillos)
        {
            flpPlatillos.Controls.Clear();

            foreach (var platillo in platillos)
            {
                Button btnPlatillo = new()
                {
                    Text = $"{platillo.Nombre}\n{platillo.PrecioVenta:C}", // 'C' es formato de moneda ($0.00)
                    Width = 130,
                    Height = 80,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Tag = platillo // ¡CLAVE! Guardamos el objeto Platillo entero en el Tag.
                };

                EstilosApp.EstiloBotonModulo(btnPlatillo);
                btnPlatillo.Click += BotonPlatillo_Click; // Asignamos el evento
                flpPlatillos.Controls.Add(btnPlatillo);
            }
        }

        /// <summary>
        /// Configura las columnas del DataGridView de la comanda la primera vez.
        /// </summary>
        private void ConfigurarGridComanda()
        {
            dgvComanda.DataSource = _comandaActual;

            if (dgvComanda.Columns["IdDetalleVenta"] is DataGridViewColumn colDetalle)
            {
                colDetalle.Visible = false;
            }

            if (dgvComanda.Columns["IdVenta"] is DataGridViewColumn colVenta)
            {
                colVenta.Visible = false;
            }

            if (dgvComanda.Columns["IdPlatillo"] is DataGridViewColumn colPlatillo)
            {
                colPlatillo.Visible = false;
            }

            if (dgvComanda.Columns["NombrePlatillo"] is DataGridViewColumn colNombre)
            {
                colNombre.HeaderText = "Platillo";
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvComanda.Columns["PrecioUnitario"] is DataGridViewColumn colPrecio)
            {
                colPrecio.HeaderText = "Precio";
            }
        }

        /// <summary>
        /// Actualiza el Label lblTotal sumando los items de la comanda.
        /// </summary>
        private void ActualizarTotal()
        {
            decimal total = _comandaActual.Sum(detalle => detalle.Cantidad * detalle.PrecioUnitario);
            lblTotal.Text = total.ToString("C");
        }

        // --- EVENT HANDLERS (CLICS DE BOTONES) ---

        private void BotonCategoria_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not string categoria) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (categoria == "Todos")
                {
                    PoblarBotonesPlatillos(_listaPlatillosCompleta);
                }
                else
                {
                    var platillosFiltrados = _listaPlatillosCompleta
                                                .Where(p => p.Categoria == categoria)
                                                .ToList();
                    PoblarBotonesPlatillos(platillosFiltrados);
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                MessageBox.Show("Selecciona un platillo de la comanda primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var confirm = MessageBox.Show($"¿Quitar '{detalle.NombrePlatillo}' de la comanda?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                MessageBox.Show("Selecciona un platillo de la comanda primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEliminarPlatillo_Click(object? sender, EventArgs e)
        {
            if (dgvComanda.CurrentRow != null && dgvComanda.CurrentRow.DataBoundItem is DetalleVenta detalle)
            {
                var confirm = MessageBox.Show($"¿Eliminar '{detalle.NombrePlatillo}' (Cantidad: {detalle.Cantidad}) de la comanda?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    _comandaActual.Remove(detalle);
                    ActualizarTotal();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un platillo de la comanda para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            if (_comandaActual.Count == 0)
            {
                MessageBox.Show("No hay platillos en la comanda para registrar.", "Comanda Vacía", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea registrar esta venta por un total de {lblTotal.Text}?", "Confirmar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnRegistrarVenta.Enabled = false;

                Venta nuevaVenta = new()
                {
                    IdUsuario = UserSession.IdUsuario,
                    FormaDePago = "Efectivo", // <-- Tarea C-2 pendiente aquí
                    TotalVenta = _comandaActual.Sum(d => d.Cantidad * d.PrecioUnitario)
                };

                await _ventasController.RegistrarVentaAsync(nuevaVenta, [.. _comandaActual]);

                MessageBox.Show("¡Venta registrada exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _comandaActual.Clear();
                ActualizarTotal();
            }
            catch (StockInsuficienteException stockEx)
            {
                string mensaje = $"{stockEx.Message}\n\n" +
                                 $"Para el platillo: '{stockEx.PlatilloConProblema}'\n" +
                                 $"Solo puede vender un máximo de {stockEx.MaximaCantidadVendible} unidades.";

                MessageBox.Show(mensaje, "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al registrar venta: {ex.Message}");
                MessageBox.Show("Ocurrió un error al registrar la venta. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRegistrarVenta.Enabled = true;
            }
        }
    }
}