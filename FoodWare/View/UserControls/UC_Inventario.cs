using System;
using System.Drawing;
using System.Threading.Tasks;     // Para tareas asíncronas
using System.Windows.Forms;
using FoodWare.Controller.Logic;  // Para MenuController
using FoodWare.Model.Entities;    // Para la clase Platillo
using FoodWare.Model.Interfaces; // Para IProductoRepository
using FoodWare.Model.DataAccess;  // Para ProductoSqlRepository
using FoodWare.Validations;      // Para nuestra librería de validación
using FoodWare.View.Helpers;      // Para EstilosApp

namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private readonly InventarioController _controller;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos(); // Llamamos a nuestro método de estilos

            // 1. Usamos el repositorio SQL real.
            IProductoRepository repositorioParaUsar = new ProductoSqlRepository();

            // 2. La Vista CREA el controlador y le PASA (inyecta) el repositorio.
            _controller = new InventarioController(repositorioParaUsar);

            // Agrega más categorías según sea necesario
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

            // Agrega más unidades de medida según sea necesario
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

        /// <summary>
        /// Aplica los estilos de EstilosApp a este UserControl.
        /// </summary>
        private void AplicarEstilos()
        {
            // Fondo del UserControl y Panel
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelInputs, EstilosApp.ColorFondo);

            // Etiquetas
            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblCategoria);
            EstilosApp.EstiloLabelModulo(lblUnidad);
            EstilosApp.EstiloLabelModulo(lblStock);
            EstilosApp.EstiloLabelModulo(lblStockMinimo);
            EstilosApp.EstiloLabelModulo(lblPrecio);

            // Cajas de Texto
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtStock);
            EstilosApp.EstiloTextBoxModulo(txtStockMinimo);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);

            // ComboBox
            EstilosApp.EstiloComboBoxModulo(cmbCategoria);
            EstilosApp.EstiloComboBoxModulo(cmbUnidadMedida);

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp. EstiloBotonModuloSecundario(btnActualizar);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvInventario);
        }

        /// <summary>
        /// Manejador de carga del UserControl. Se convierte en 'async void'
        /// para permitir la carga de datos sin bloquear la UI.
        /// </summary>
        private async void UC_Inventario_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Ahora llamamos a la versión asíncrona
                await CargarGridInventarioAsync();
            }
        }

        /// <summary>
        /// Manejador del clic en 'Guardar'. Se convierte en 'async void'
        /// para guardar sin bloquear la UI.
        /// </summary>
        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista RECOGE los datos.
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? ""; // <-- CAMPO CORREGIDO

                if (!decimal.TryParse(txtStock.Text, out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo)) // <-- CAMPO CORREGIDO
                {
                    MessageBox.Show("El stockMinimo debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStockMinimo.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // --- Llamada Asíncrona ---
                this.Cursor = Cursors.WaitCursor; // Indicador visual

                // 2. La Vista ENVÍA los datos al controlador
                await _controller.GuardarNuevoProductoAsync(nombre, categoria, unidad, stock, stockminimo, precio);

                // 3. Si todo salió bien, la Vista se ACTUALIZA.
                MessageBox.Show("¡Producto guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Recargamos el grid de forma asíncrona
                await CargarGridInventarioAsync();
            }
            catch (ArgumentException aex) // Errores de validación del Controlador
            {
                MessageBox.Show(aex.Message, "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Errores inesperados (BD, etc.)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Manejador del clic en 'Eliminar'. Se convierte en 'async void'.
        /// </summary>
        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica si hay una fila seleccionada
            if (this.dgvInventario.CurrentRow != null && this.dgvInventario.CurrentRow.DataBoundItem is Producto producto)
            {
                int id = producto.IdProducto;
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{producto.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        // --- Llamada Asíncrona ---
                        await _controller.EliminarProductoAsync(id);

                        // Actualiza la vista
                        await CargarGridInventarioAsync();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto de la lista para eliminar.", "Ningún producto seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- MÉTODOS DE AYUDA DE LA VISTA ---

        /// <summary>
        /// Pide los productos al controlador y actualiza el DataGridView.
        /// </summary>
        /// <summary>
        /// Pide los productos al controlador y actualiza el DataGridView de forma asíncrona.
        /// </summary>
        private async Task CargarGridInventarioAsync()
        {
            try
            {
                // Mostramos un indicador de carga
                this.Cursor = Cursors.WaitCursor;
                dgvInventario.DataSource = null;

                // 1. Obtenemos los datos de forma asíncrona
                var productos = await _controller.CargarProductosAsync();

                // 2. Volvemos al hilo de la UI para actualizar el Grid
                this.dgvInventario.DataSource = productos;

                // Opcional: ajustar columnas
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
                if (columnas != null && columnas["PrecioCosto"] is not null)
                {
                    columnas["PrecioCosto"]!.HeaderText = "Precio Costo";
                    columnas["PrecioCosto"]!.Width = 150;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el inventario: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Nos aseguramos de regresar el cursor a la normalidad
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Limpia las cajas de texto del formulario.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            cmbCategoria.SelectedIndex = -1;
            cmbUnidadMedida.SelectedIndex = -1;
            txtStock.Text = "";
            txtStockMinimo.Text = "";
            txtPrecio.Text = "";
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}