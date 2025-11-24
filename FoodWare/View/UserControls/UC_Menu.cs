using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;
using FoodWare.Controller.Logic;
using FoodWare.Model.Entities;
using FoodWare.Validations;
using FoodWare.View.Helpers;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace FoodWare.View.UserControls
{
    public partial class UC_Menu : UserControl
    {
        // Constantes
        private const string TituloExito = "Éxito";
        private const string TituloError = "Error";
        private const string TituloDatoInvalido = "Dato inválido";
        private const string TituloDatosIncompletos = "Datos incompletos";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado. Contacte al administrador.";

        private readonly MenuController _controller;
        private readonly RecetaController _recetaController;
        private readonly InventarioController _inventarioController;

        private Platillo? _platilloEnEdicion;
        private Platillo? _platilloSeleccionado;
        private bool _modoEdicion = false;

        public UC_Menu()
        {
            InitializeComponent();
            AplicarEstilos();

            IPlatilloRepository platilloRepo = new PlatilloSqlRepository();
            IRecetaRepository recetaRepo = new RecetaSqlRepository();
            IProductoRepository productoRepo = new ProductoSqlRepository();
            IMovimientoRepository movimientoRepo = new MovimientoSqlRepository();

            _controller = new MenuController(platilloRepo);
            _recetaController = new RecetaController(recetaRepo);
            _inventarioController = new InventarioController(productoRepo, movimientoRepo, recetaRepo);

            cmbCategoria.Items.Add("Entradas");
            cmbCategoria.Items.Add("Platos Fuertes");
            cmbCategoria.Items.Add("Postres");
            cmbCategoria.Items.Add("Bebidas");
            cmbCategoria.Items.Add("Sopas y Ensaladas");
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelInputs, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblCategoria);
            EstilosApp.EstiloLabelModulo(lblPrecio);
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);
            EstilosApp.EstiloComboBoxModulo(cmbCategoria);

            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizar);

            EstilosApp.EstiloDataGridView(dgvMenu);

            EstilosApp.EstiloPanel(panelEdicionReceta, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelTitulo(lblTituloReceta);
            EstilosApp.EstiloLabelModulo(lblProducto);
            EstilosApp.EstiloLabelModulo(lblCantidadReceta);
            EstilosApp.EstiloLabelTitulo(lblCostoReceta);
            EstilosApp.EstiloDataGridView(dgvReceta);
            EstilosApp.EstiloComboBoxModulo(cmbProductos);
            EstilosApp.EstiloTextBoxModulo(txtCantidadReceta);
            EstilosApp.EstiloBotonModulo(btnAgregarIngrediente);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminarIngrediente);
            EstilosApp.EstiloBotonModuloSecundario(btnVolverAlMenu);
        }

        private async void UC_Menu_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                await CargarGridPlatillosAsync();
                EstablecerModoEdicion(false);
            }
        }

        private void EstablecerModoEdicion(bool activo)
        {
            _modoEdicion = activo;

            if (activo)
            {
                btnGuardar.Visible = false;
                btnActualizar.Visible = true;
                btnEliminar.Visible = true;
            }
            else
            {
                btnGuardar.Visible = true;
                btnActualizar.Visible = false;
                btnEliminar.Visible = false;
                _platilloSeleccionado = null;
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtNombre.Focus();

            EstablecerModoEdicion(false);
        }

        private void ItemEditarPlatillo_Click(object? sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow != null && dgvMenu.CurrentRow.DataBoundItem is Platillo platillo)
            {
                _platilloSeleccionado = platillo;

                txtNombre.Text = platillo.Nombre;
                cmbCategoria.SelectedItem = platillo.Categoria;
                txtPrecio.Text = platillo.PrecioVenta.ToString("F2");

                EstablecerModoEdicion(true);
                txtNombre.Focus();
            }
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_modoEdicion) return;

            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                await _controller.GuardarNuevoPlatilloAsync(nombre, categoria, precio);
                MessageBox.Show("¡Platillo guardado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridPlatillosAsync();
            }
            catch (ArgumentException aex) { MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error guardar platillo: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _platilloSeleccionado == null) return;

            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // 1. Guardar ID
                int idParaSeleccionar = _platilloSeleccionado.IdPlatillo;

                Platillo platilloActualizado = new()
                {
                    IdPlatillo = idParaSeleccionar,
                    Nombre = nombre,
                    Categoria = categoria,
                    PrecioVenta = precio
                };

                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarPlatilloAsync(platilloActualizado);

                MessageBox.Show("¡Platillo actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridPlatillosAsync();

                // 2. Reseleccionar
                SeleccionarPlatilloEnGrid(idParaSeleccionar);
            }
            catch (ArgumentException aex) { MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error actualizar platillo: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (this.dgvMenu.CurrentRow != null && this.dgvMenu.CurrentRow.DataBoundItem is Platillo platilloSeleccionado)
            {
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{platilloSeleccionado.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await _controller.EliminarPlatilloAsync(platilloSeleccionado.IdPlatillo);
                        await CargarGridPlatillosAsync();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error eliminar platillo: {ex.Message}");
                        MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally { this.Cursor = Cursors.Default; }
                }
            }
        }


        private async Task CargarGridPlatillosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var platillos = await _controller.CargarPlatillosAsync();
                this.dgvMenu.DataSource = null;
                this.dgvMenu.DataSource = platillos;

                var cols = dgvMenu.Columns;
                if (cols["IdPlatillo"] != null) { cols["IdPlatillo"]!.HeaderText = "ID"; cols["IdPlatillo"]!.Width = 50; }
                if (cols["PrecioVenta"] != null) cols["PrecioVenta"]!.HeaderText = "Precio de Venta";
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void ItemGestionarReceta_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null || dgvMenu.CurrentRow.DataBoundItem is not Platillo platillo) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                _platilloEnEdicion = platillo;
                lblTituloReceta.Text = $"Gestionando Receta de: {_platilloEnEdicion.Nombre}";

                dgvReceta.DataSource = null;
                cmbProductos.DataSource = null;

                dgvMenu.Visible = false;
                panelEdicionReceta.Visible = true;

                await CargarGridRecetaAsync(_platilloEnEdicion.IdPlatillo);
                await CargarProductosComboBoxAsync();
                ActualizarCostoRecetaUI();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargar receta: {ex.Message}");
                MessageBox.Show("Error al cargar receta.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BtnVolverAlMenu_Click(sender, e);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void BtnVolverAlMenu_Click(object sender, EventArgs e)
        {
            panelEdicionReceta.Visible = false;
            dgvMenu.Visible = true;
            _platilloEnEdicion = null;
            dgvReceta.DataSource = null;
            LimpiarCampos();
        }

        private async Task CargarProductosComboBoxAsync()
        {
            try
            {
                cmbProductos.DataSource = null;
                var productos = await _inventarioController.CargarProductosAsync();
                cmbProductos.DataSource = productos;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "IdProducto";
                cmbProductos.SelectedIndex = -1;
            }
            catch { /* Log */ }
        }

        private async Task CargarGridRecetaAsync(int idPlatillo)
        {
            try
            {
                dgvReceta.DataSource = null;
                var receta = await _recetaController.CargarRecetaDePlatilloAsync(idPlatillo);
                dgvReceta.DataSource = receta;
                if (dgvReceta.Columns["IdReceta"] != null) dgvReceta.Columns["IdReceta"]!.Visible = false;
                if (dgvReceta.Columns["IdProducto"] != null) dgvReceta.Columns["IdProducto"]!.Visible = false;
                if (dgvReceta.Columns["PrecioCosto"] != null) dgvReceta.Columns["PrecioCosto"]!.Visible = false;
            }
            catch { /* Log */ }
        }

        private void ActualizarCostoRecetaUI()
        {
            if (dgvReceta.DataSource is not List<RecetaDetalle> receta)
            {
                lblCostoReceta.Text = "Costo: $0.00";
                return;
            }
            decimal costoTotal = receta.Sum(item => item.Cantidad * item.PrecioCosto);
            lblCostoReceta.Text = $"Costo Total de Receta: {costoTotal:C}";
        }

        private async void BtnAgregarIngrediente_Click(object sender, EventArgs e)
        {
            if (_platilloEnEdicion == null || cmbProductos.SelectedValue == null) { MessageBox.Show("Seleccione producto.", TituloDatosIncompletos, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                int idPlatillo = _platilloEnEdicion.IdPlatillo;
                int idProducto = (int)cmbProductos.SelectedValue;
                if (!decimal.TryParse(txtCantidadReceta.Text, out decimal cantidad)) { MessageBox.Show("Cantidad inválida.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                await _recetaController.GuardarNuevoIngredienteAsync(idPlatillo, idProducto, cantidad);
                await CargarGridRecetaAsync(idPlatillo);
                ActualizarCostoRecetaUI();
                txtCantidadReceta.Clear();
                cmbProductos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error agregar ingrediente: {ex.Message}");
                MessageBox.Show("Error al agregar.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEliminarIngrediente_Click(object sender, EventArgs e)
        {
            if (dgvReceta.CurrentRow == null || dgvReceta.CurrentRow.DataBoundItem is not RecetaDetalle ingrediente) return;
            if (_platilloEnEdicion == null) return;

            try
            {
                await _recetaController.EliminarIngredienteAsync(ingrediente.IdReceta);
                await CargarGridRecetaAsync(_platilloEnEdicion.IdPlatillo);
                ActualizarCostoRecetaUI();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error eliminar ingrediente: {ex.Message}");
                MessageBox.Show("Error al eliminar.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMenu_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                dgvMenu.CurrentCell = dgvMenu.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void SeleccionarPlatilloEnGrid(int idPlatillo)
        {
            foreach (DataGridViewRow row in dgvMenu.Rows)
            {
                if (row.DataBoundItem is Platillo p && p.IdPlatillo == idPlatillo)
                {
                    row.Selected = true;
                    dgvMenu.CurrentCell = row.Cells[0];
                    dgvMenu.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }
    }
}