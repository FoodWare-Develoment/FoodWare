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

namespace FoodWare.View.UserControls
{
    public partial class UC_Menu : UserControl
    {
        // --- CONSTANTES PARA SONARLINT (S1192) ---
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

                var columnas = this.dgvMenu.Columns;
                if (columnas != null && columnas["IdPlatillo"] is not null)
                {
                    columnas["IdPlatillo"]!.HeaderText = "ID";
                    columnas["IdPlatillo"]!.Width = 50;
                }
                if (columnas != null && columnas["PrecioVenta"] is not null)
                {
                    columnas["PrecioVenta"]!.HeaderText = "Precio de Venta";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar menú: {ex.Message}");
                MessageBox.Show("Error al cargar el menú. Contacte al administrador.", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtNombre.Focus();
            _platilloSeleccionado = null;
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar productos combo: {ex.Message}");
                MessageBox.Show("Error al cargar productos. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarGridRecetaAsync(int idPlatillo)
        {
            try
            {
                dgvReceta.DataSource = null;
                var receta = await _recetaController.CargarRecetaDePlatilloAsync(idPlatillo);
                dgvReceta.DataSource = receta;

                if (dgvReceta.Columns["IdReceta"] is DataGridViewColumn colReceta)
                {
                    colReceta.Visible = false;
                }
                if (dgvReceta.Columns["IdProducto"] is DataGridViewColumn colProducto)
                {
                    colProducto.Visible = false;
                }
                if (dgvReceta.Columns["PrecioCosto"] is DataGridViewColumn colCosto)
                {
                    colCosto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar receta: {ex.Message}");
                MessageBox.Show("Error al cargar la receta. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
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
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar platillo: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (this.dgvMenu.CurrentRow != null && this.dgvMenu.CurrentRow.DataBoundItem is Platillo platilloSeleccionado)
            {
                int id = platilloSeleccionado.IdPlatillo;
                string nombre = platilloSeleccionado.Nombre;

                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{nombre}'?",
                                               "Confirmar eliminación",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await _controller.EliminarPlatilloAsync(id);
                        await CargarGridPlatillosAsync();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al eliminar platillo: {ex.Message}");
                        MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un platillo de la lista para eliminar.", "Ningún platillo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (_platilloSeleccionado == null)
            {
                MessageBox.Show("Por favor, selecciona un platillo de la lista para actualizar.", "Ningún platillo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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

                Platillo platilloActualizado = new()
                {
                    IdPlatillo = _platilloSeleccionado.IdPlatillo,
                    Nombre = nombre,
                    Categoria = categoria,
                    PrecioVenta = precio
                };

                int idActualizado = platilloActualizado.IdPlatillo;

                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarPlatilloAsync(platilloActualizado);

                MessageBox.Show("¡Platillo actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridPlatillosAsync();
                SeleccionarPlatilloEnGrid(idActualizado);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar platillo: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void DgvMenu_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvMenu.CurrentCell = dgvMenu.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private async void ItemGestionarReceta_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null || dgvMenu.CurrentRow.DataBoundItem is not Platillo platilloSeleccionado)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                _platilloEnEdicion = platilloSeleccionado;
                lblTituloReceta.Text = $"Gestionando Receta de: {_platilloEnEdicion.Nombre}";

                dgvReceta.DataSource = null;
                cmbProductos.DataSource = null;

                dgvMenu.Visible = false;
                panelEdicionReceta.Visible = true;

                await CargarGridRecetaAsync(_platilloEnEdicion.IdPlatillo);
                await CargarProductosComboBoxAsync();
                ActualizarCostoRecetaUI(); // (C-6)
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al gestionar receta: {ex.Message}");
                MessageBox.Show("Error al preparar la gestión de receta. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                BtnVolverAlMenu_Click(sender, e);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnVolverAlMenu_Click(object sender, EventArgs e)
        {
            panelEdicionReceta.Visible = false;
            dgvMenu.Visible = true;
            _platilloEnEdicion = null;
            dgvReceta.DataSource = null;
        }

        private async void BtnAgregarIngrediente_Click(object sender, EventArgs e)
        {
            if (_platilloEnEdicion == null || cmbProductos.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un producto.", TituloDatosIncompletos, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idPlatillo = _platilloEnEdicion.IdPlatillo;
                int idProducto = (int)cmbProductos.SelectedValue;

                if (!decimal.TryParse(txtCantidadReceta.Text, out decimal cantidad))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await _recetaController.GuardarNuevoIngredienteAsync(idPlatillo, idProducto, cantidad);
                await CargarGridRecetaAsync(idPlatillo);
                ActualizarCostoRecetaUI(); // (C-6)

                txtCantidadReceta.Clear();
                cmbProductos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al agregar ingrediente: {ex.Message}");
                MessageBox.Show("Error al guardar el ingrediente. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEliminarIngrediente_Click(object sender, EventArgs e)
        {
            if (dgvReceta.CurrentRow == null || dgvReceta.CurrentRow.DataBoundItem is not RecetaDetalle ingrediente)
            {
                MessageBox.Show("Debe seleccionar un ingrediente de la lista de receta.", TituloDatosIncompletos, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{ingrediente.NombreProducto}' de la receta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                Platillo? platillo = _platilloEnEdicion;

                if (platillo is null)
                {
                    MessageBox.Show("Error de lógica: No hay ningún platillo en modo de edición.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    await _recetaController.EliminarIngredienteAsync(ingrediente.IdReceta);
                    await CargarGridRecetaAsync(platillo.IdPlatillo);
                    ActualizarCostoRecetaUI(); // (C-6)
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al eliminar ingrediente: {ex.Message}");
                    MessageBox.Show("Error al eliminar el ingrediente. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ItemEditarPlatillo_Click(object? sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow != null && dgvMenu.CurrentRow.DataBoundItem is Platillo platillo)
            {
                _platilloSeleccionado = platillo;
                txtNombre.Text = platillo.Nombre;
                cmbCategoria.SelectedItem = platillo.Categoria;
                txtPrecio.Text = platillo.PrecioVenta.ToString("F2");
                txtNombre.Focus();
            }
            else
            {
                LimpiarCampos();
            }
        }

        private void SeleccionarPlatilloEnGrid(int idPlatillo)
        {
            foreach (DataGridViewRow row in dgvMenu.Rows)
            {
                if (row.DataBoundItem is Platillo platillo && platillo.IdPlatillo == idPlatillo)
                {
                    row.Selected = true;
                    dgvMenu.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }
    }
}