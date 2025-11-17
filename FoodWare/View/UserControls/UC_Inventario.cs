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
using Microsoft.VisualBasic; // Para InputBox

namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private const string TituloExito = "Éxito";
        private const string TituloError = "Error";
        private const string TituloDatoInvalido = "Dato inválido";
        private const string TituloAdvertencia = "Aviso";
        private const string TituloAccionBloqueada = "Acción Bloqueada";
        private const string TituloProductoNoSel = "Producto no seleccionado";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado. Contacte al administrador.";
        private const string MsgProductoNoSel = "Por favor, selecciona un producto de la lista para esta acción.";
        private const string MsgProductoNoSelEditar = "Por favor, selecciona un producto de la lista (con clic derecho > Editar) antes de esta acción.";

        private readonly InventarioController _controller;
        private Producto? _productoSeleccionado;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos();

            IProductoRepository repositorioParaUsar = new ProductoSqlRepository();
            IMovimientoRepository movimientoRepo = new MovimientoSqlRepository();
            IRecetaRepository recetaRepo = new RecetaSqlRepository();

            _controller = new InventarioController(repositorioParaUsar, movimientoRepo, recetaRepo);

            // ... (Llenado de ComboBoxes) ...
            cmbCategoria.Items.Add("Abarrotes Secos");
            cmbCategoria.Items.Add("Proteínas");
            cmbCategoria.Items.Add("Frutas y Verduras");
            cmbCategoria.Items.Add("Lácteos y Derivados");
            // ... (etc.) ...
            cmbUnidadMedida.Items.Add("kg");
            cmbUnidadMedida.Items.Add("Unidad");
            // ... (etc.) ...
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
            }
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtStock.Text, out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo))
                {
                    MessageBox.Show("El stockMinimo debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockMinimo.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                await _controller.GuardarNuevoProductoAsync(nombre, categoria, unidad, stock, stockminimo, precio);
                MessageBox.Show("¡Producto guardado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (_productoSeleccionado == null)
            {
                MessageBox.Show(MsgProductoNoSel, TituloProductoNoSel, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtStock.Text, out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo))
                {
                    MessageBox.Show("El stock Mínimo debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockMinimo.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                Producto productoActualizado = new()
                {
                    IdProducto = _productoSeleccionado.IdProducto,
                    Nombre = nombre,
                    Categoria = categoria,
                    UnidadMedida = unidad,
                    StockActual = stock,
                    StockMinimo = stockminimo,
                    PrecioCosto = precio
                };

                int idActualizado = productoActualizado.IdProducto;

                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarProductoAsync(productoActualizado);
                MessageBox.Show("¡Producto actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridInventarioAsync();
                SeleccionarProductoEnGrid(idActualizado);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                int id = producto.IdProducto;
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{producto.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await _controller.EliminarProductoAsync(id);

                        await CargarGridInventarioAsync();
                        LimpiarCampos();
                    }
                    catch (InvalidOperationException ioex)
                    {
                        MessageBox.Show(ioex.Message, TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al eliminar producto: {ex.Message}");
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
                MessageBox.Show(MsgProductoNoSel, TituloProductoNoSel, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                var columnas = this.dgvInventario.Columns;
                if (columnas != null && columnas["IdProducto"] is not null)
                {
                    columnas["IdProducto"]!.HeaderText = "ID";
                    columnas["IdProducto"]!.Width = 50;
                }
                if (columnas != null && columnas["StockActual"] is not null)
                {
                    columnas["StockActual"]!.HeaderText = "Stock Actual";
                }
                if (columnas != null && columnas["StockMinimo"] is not null)
                {
                    columnas["StockMinimo"]!.HeaderText = "Stock Mínimo";
                }
                if (columnas != null && columnas["UnidadMedida"] is not null)
                {
                    columnas["UnidadMedida"]!.HeaderText = "Unidad";
                }
                if (columnas != null && columnas["PrecioCosto"] is not null)
                {
                    columnas["PrecioCosto"]!.HeaderText = "Precio Costo";
                    columnas["PrecioCosto"]!.Width = 150;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar inventario: {ex.Message}");
                MessageBox.Show("Error al cargar el inventario. Contacte al administrador.", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            cmbUnidadMedida.SelectedIndex = -1;
            txtStock.Text = "";
            txtStockMinimo.Text = "";
            txtPrecio.Text = "";
            _productoSeleccionado = null;
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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
                txtNombre.Focus();
            }
        }

        private void SeleccionarProductoEnGrid(int idProducto)
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.DataBoundItem is Producto producto && producto.IdProducto == idProducto)
                {
                    dgvInventario.CurrentCell = row.Cells[0];
                    dgvInventario.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        // --- MÉTODO CORREGIDO (S1066) ---
        private void DgvInventario_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.dgvInventario.NewRowIndex)
                return;

            // Combinamos los dos 'if' en uno solo con '&&'
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
            if (_productoSeleccionado == null)
            {
                MessageBox.Show(MsgProductoNoSelEditar, TituloProductoNoSel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sCantidad = Interaction.InputBox($"Añadir stock para: {_productoSeleccionado.Nombre}\n(Stock actual: {_productoSeleccionado.StockActual})", "Registrar Entrada", "0.00");

            if (decimal.TryParse(sCantidad, out decimal cantidad) && cantidad > 0)
            {
                string motivo = Interaction.InputBox("Motivo de la entrada (Opcional):", "Registrar Entrada", "Compra proveedor");

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    await _controller.RegistrarEntradaAsync(_productoSeleccionado.IdProducto, UserSession.IdUsuario, cantidad, motivo);

                    MessageBox.Show("¡Stock actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarGridInventarioAsync();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar entrada: {ex.Message}", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else if (!string.IsNullOrWhiteSpace(sCantidad))
            {
                MessageBox.Show("La cantidad debe ser un número positivo.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnRegistrarMerma_Click(object sender, EventArgs e)
        {
            if (_productoSeleccionado == null)
            {
                MessageBox.Show(MsgProductoNoSelEditar, TituloProductoNoSel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sCantidad = Interaction.InputBox($"Registrar merma para: {_productoSeleccionado.Nombre}\n(Stock actual: {_productoSeleccionado.StockActual})", "Registrar Merma", "0.00");

            if (decimal.TryParse(sCantidad, out decimal cantidad) && cantidad > 0)
            {
                string motivo = Interaction.InputBox("Motivo de la merma (¡Obligatorio!):", "Registrar Merma", "Caducado");

                if (string.IsNullOrWhiteSpace(motivo))
                {
                    MessageBox.Show("El motivo es obligatorio para registrar una merma.", TituloAdvertencia, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    await _controller.RegistrarMermaAsync(_productoSeleccionado.IdProducto, UserSession.IdUsuario, cantidad, motivo);

                    MessageBox.Show("¡Merma registrada! El stock ha sido actualizado.", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarGridInventarioAsync();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar merma: {ex.Message}", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else if (!string.IsNullOrWhiteSpace(sCantidad))
            {
                MessageBox.Show("La cantidad debe ser un número positivo.", TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}