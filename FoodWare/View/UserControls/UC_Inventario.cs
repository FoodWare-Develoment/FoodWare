using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoodWare.Controller.Logic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;
using FoodWare.Validations;
using FoodWare.View.Helpers;
using Microsoft.VisualBasic;

namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private const string TituloExito = "Éxito";
        private const string TituloError = "Error";
        private const string TituloDatoInvalido = "Dato inválido";
        private const string TituloAdvertencia = "Aviso";
        private const string TituloAccionBloqueada = "Acción Bloqueada";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado. Contacte al administrador.";
        private const string MsgProductoNoSelEditar = "Por favor, selecciona un producto de la lista (con clic derecho > Editar) antes de esta acción.";

        private readonly InventarioController _controller;
        private Producto? _productoSeleccionado;
        private bool _modoEdicion = false;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos();

            IProductoRepository repositorioParaUsar = new ProductoSqlRepository();
            IMovimientoRepository movimientoRepo = new MovimientoSqlRepository();
            IRecetaRepository recetaRepo = new RecetaSqlRepository();

            _controller = new InventarioController(repositorioParaUsar, movimientoRepo, recetaRepo);

            cmbCategoria.Items.Add("Abarrotes Secos");
            cmbCategoria.Items.Add("Proteínas");
            cmbCategoria.Items.Add("Frutas y Verduras");
            cmbCategoria.Items.Add("Lácteos y Derivados");
            cmbCategoria.Items.Add("Panadería y Tortilleria");
            cmbCategoria.Items.Add("Congelados");
            cmbCategoria.Items.Add("Bebidas Alcohólicas");
            cmbCategoria.Items.Add("Bebidas No Alcohólicas");
            cmbCategoria.Items.Add("Desechables y Empaques");
            cmbCategoria.Items.Add("Productos de Limpieza");
            cmbCategoria.Items.Add("Otros");

            cmbUnidadMedida.Items.Add("kg");
            cmbUnidadMedida.Items.Add("g");
            cmbUnidadMedida.Items.Add("L");
            cmbUnidadMedida.Items.Add("mL");
            cmbUnidadMedida.Items.Add("Unidad");
            cmbUnidadMedida.Items.Add("Bolsa");
            cmbUnidadMedida.Items.Add("Paquete");
            cmbUnidadMedida.Items.Add("Caja");
            cmbUnidadMedida.Items.Add("Lata");
            cmbUnidadMedida.Items.Add("Frasco");
            cmbUnidadMedida.Items.Add("Botella");
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelInputs, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblCategoria);
            EstilosApp.EstiloLabelModulo(lblUnidad);
            EstilosApp.EstiloLabelModulo(lblStock);
            EstilosApp.EstiloLabelModulo(lblStockMinimo);
            EstilosApp.EstiloLabelModulo(lblPrecio);
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtStock);
            EstilosApp.EstiloTextBoxModulo(txtStockMinimo);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);
            EstilosApp.EstiloComboBoxModulo(cmbCategoria);
            EstilosApp.EstiloComboBoxModulo(cmbUnidadMedida);

            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizar);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);
            EstilosApp.EstiloBotonModulo(btnAnadirStock);
            EstilosApp.EstiloBotonModuloAlerta(btnRegistrarMerma);

            EstilosApp.EstiloDataGridView(dgvInventario);
        }

        private async void UC_Inventario_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                await CargarGridInventarioAsync();
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
                btnAnadirStock.Visible = true;
                btnRegistrarMerma.Visible = true;
                btnLimpiar.Text = "Cancelar";
                txtStock.Enabled = false;
            }
            else
            {
                btnGuardar.Visible = true;
                btnActualizar.Visible = false;
                btnEliminar.Visible = false;
                btnAnadirStock.Visible = false;
                btnRegistrarMerma.Visible = false;
                btnLimpiar.Text = "Limpiar";
                txtStock.Enabled = true;
                _productoSeleccionado = null;
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            cmbCategoria.SelectedIndex = -1;
            cmbUnidadMedida.SelectedIndex = -1;
            txtStock.Text = "";
            txtStockMinimo.Text = "";
            txtPrecio.Text = "";

            EstablecerModoEdicion(false);
        }

        private void ItemEditarProducto_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow != null && dgvInventario.CurrentRow.DataBoundItem is Producto producto)
            {
                _productoSeleccionado = producto;

                txtNombre.Text = producto.Nombre;
                cmbCategoria.SelectedItem = producto.Categoria;
                cmbUnidadMedida.SelectedItem = producto.UnidadMedida;
                txtStock.Text = producto.StockActual.ToString("F2");
                txtStockMinimo.Text = producto.StockMinimo.ToString("F2");
                txtPrecio.Text = producto.PrecioCosto.ToString("F2");

                EstablecerModoEdicion(true);
                txtNombre.Focus();
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_modoEdicion) return;

            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtStock.Text, out decimal stock)) { MessageBox.Show("El stock debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo)) { MessageBox.Show("El stockMinimo debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio)) { MessageBox.Show("El precio debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                this.Cursor = Cursors.WaitCursor;
                await _controller.GuardarNuevoProductoAsync(nombre, categoria, unidad, stock, stockminimo, precio);

                MessageBox.Show("¡Producto guardado exitosamente!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridInventarioAsync();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar producto: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _productoSeleccionado == null) return;

            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? "";
                decimal stock = _productoSeleccionado.StockActual;

                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo)) { MessageBox.Show("Stock Mínimo inválido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio)) { MessageBox.Show("Precio inválido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                // 1. Guardamos el ID antes de que se pierda la referencia
                int idParaSeleccionar = _productoSeleccionado.IdProducto;

                Producto productoActualizado = new()
                {
                    IdProducto = idParaSeleccionar,
                    Nombre = nombre,
                    Categoria = categoria,
                    UnidadMedida = unidad,
                    StockActual = stock,
                    StockMinimo = stockminimo,
                    PrecioCosto = precio
                };

                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarProductoAsync(productoActualizado);
                MessageBox.Show("¡Producto actualizado correctamente!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridInventarioAsync();

                // 2. Volvemos a seleccionar la fila en el grid
                SeleccionarProductoEnGrid(idParaSeleccionar);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar producto: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (this.dgvInventario.CurrentRow != null && this.dgvInventario.CurrentRow.DataBoundItem is Producto producto)
            {
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{producto.Nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await _controller.EliminarProductoAsync(producto.IdProducto);

                        await CargarGridInventarioAsync();
                        LimpiarCampos();
                    }
                    catch (InvalidOperationException ioex)
                    {
                        MessageBox.Show(ioex.Message, TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al eliminar: {ex.Message}");
                        MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private async Task CargarGridInventarioAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvInventario.DataSource = null;
                var productos = await _controller.CargarProductosAsync();
                this.dgvInventario.DataSource = productos;

                ConfigurarColumnasGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error carga inventario: {ex.Message}");
                MessageBox.Show("Error de conexión al cargar inventario.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ConfigurarColumnasGrid()
        {
            var columnas = this.dgvInventario.Columns;
            if (columnas["IdProducto"] != null) { columnas["IdProducto"]!.HeaderText = "ID"; columnas["IdProducto"]!.Width = 50; }
            if (columnas["StockActual"] != null) columnas["StockActual"]!.HeaderText = "Stock Actual";
            if (columnas["StockMinimo"] != null) columnas["StockMinimo"]!.HeaderText = "Stock Mínimo";
            if (columnas["UnidadMedida"] != null) columnas["UnidadMedida"]!.HeaderText = "Unidad";
            if (columnas["PrecioCosto"] != null) { columnas["PrecioCosto"]!.HeaderText = "Precio Costo"; columnas["PrecioCosto"]!.Width = 150; }
        }

        private void DgvInventario_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.dgvInventario.NewRowIndex) return;

            if (dgvInventario.Rows[e.RowIndex].DataBoundItem is Producto producto &&
                producto.StockActual <= producto.StockMinimo)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 230, 230);
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.Red;
                e.CellStyle.SelectionForeColor = Color.White;
            }
        }

        private async void BtnAnadirStock_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _productoSeleccionado == null)
            {
                MessageBox.Show(MsgProductoNoSelEditar, TituloAdvertencia, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sCantidad = Interaction.InputBox($"Añadir stock para: {_productoSeleccionado.Nombre}\n(Stock actual: {_productoSeleccionado.StockActual})", "Registrar Entrada", "0.00");
            if (decimal.TryParse(sCantidad, out decimal cantidad) && cantidad > 0)
            {
                string motivo = Interaction.InputBox("Motivo de la entrada (Opcional):", "Registrar Entrada", "Compra proveedor");
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    int idProducto = _productoSeleccionado.IdProducto; // Guardar ID

                    await _controller.RegistrarEntradaAsync(idProducto, UserSession.IdUsuario, cantidad, motivo);
                    MessageBox.Show("¡Stock actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarGridInventarioAsync();
                    LimpiarCampos();

                    // Re-seleccionar
                    SeleccionarProductoEnGrid(idProducto);
                }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                finally { this.Cursor = Cursors.Default; }
            }
        }

        private async void BtnRegistrarMerma_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _productoSeleccionado == null)
            {
                MessageBox.Show(MsgProductoNoSelEditar, TituloAdvertencia, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sCantidad = Interaction.InputBox($"Registrar merma para: {_productoSeleccionado.Nombre}\n(Stock actual: {_productoSeleccionado.StockActual})", "Registrar Merma", "0.00");
            if (decimal.TryParse(sCantidad, out decimal cantidad) && cantidad > 0)
            {
                string motivo = Interaction.InputBox("Motivo de la merma (Obligatorio):", "Registrar Merma", "Caducado");
                if (string.IsNullOrWhiteSpace(motivo)) { MessageBox.Show("El motivo es obligatorio.", TituloAdvertencia, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    int idProducto = _productoSeleccionado.IdProducto; // Guardar ID

                    await _controller.RegistrarMermaAsync(idProducto, UserSession.IdUsuario, cantidad, motivo);
                    MessageBox.Show("¡Merma registrada!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarGridInventarioAsync();
                    LimpiarCampos();

                    // Re-seleccionar
                    SeleccionarProductoEnGrid(idProducto);
                }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                finally { this.Cursor = Cursors.Default; }
            }
        }

        private void SeleccionarProductoEnGrid(int idProducto)
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.DataBoundItem is Producto producto && producto.IdProducto == idProducto)
                {
                    row.Selected = true;
                    dgvInventario.CurrentCell = row.Cells[0];
                    dgvInventario.FirstDisplayedScrollingRowIndex = row.Index; // Hacer visible
                    break;
                }
            }
        }
    }
}