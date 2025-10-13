using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.Model.Interfaces; // Para IProductoRepository
using FoodWare.Model.DataAccess; // Para ProductoMockRepository
using FoodWare.Controller.Logic;  // Para MenuController
using FoodWare.Model.Entities;    // Para la clase Platillo
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

            // 1. La Vista decide qué repositorio usar. Por ahora, el FALSO (Mock).
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

        private void UC_Inventario_Load(object sender, EventArgs e)
        {
            if (!DesignMode) // Esto evita que el código se ejecute en el diseñador
            {
                CargarGridInventario(); // Le decimos que llene la tabla
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista ahora solo RECOGE los datos.
                // La validación compleja ya no es su responsabilidad.
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";
                string unidad = cmbUnidadMedida.SelectedItem?.ToString() ?? "";

                // Se convierten los valores aquí, ya que el controlador espera los tipos correctos.
                // Si la conversión falla, la vista puede manejarlo localmente.
                if (!decimal.TryParse(txtStock.Text, out decimal stock))
                {
                    MessageBox.Show("El stock debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockminimo))
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

                // 2. La Vista ENVÍA los datos al controlador.
                _controller.GuardarNuevoProducto(nombre, categoria, unidad, stock, stockminimo, precio);

                // 3. Si todo salió bien (no hubo excepciones), la Vista se ACTUALIZA.
                MessageBox.Show("¡Producto guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGridInventario();
                LimpiarCampos();
            }
            // 4. La Vista ahora está preparada para capturar ERRORES DE NEGOCIO del controlador.
            catch (ArgumentException aex)
            {
                // Muestra el mensaje de error específico que viene del controlador.
                MessageBox.Show(aex.Message, "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // 5. Y también está preparada para cualquier otro error inesperado.
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica si hay una fila seleccionada en el grid
            if (this.dgvInventario.CurrentRow != null && this.dgvInventario.CurrentRow.DataBoundItem is Producto producto)
            {
                int id = producto.IdProducto;

                // 2. Pide confirmación
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{producto.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    // 3. Envía la orden al controlador
                    _controller.EliminarProducto(id);

                    // 4. Actualiza la vista
                    CargarGridInventario();
                    LimpiarCampos();
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
        private void CargarGridInventario()
        {
            var productos = _controller.CargarProductos();
            this.dgvInventario.DataSource = null;
            this.dgvInventario.DataSource = productos;

            // Opcional: ajustar columnas
            var columnas = this.dgvInventario.Columns;
            if (columnas != null && columnas["IdProducto"] is not null)
            {
                columnas["IdProducto"]!.HeaderText = "ID";
                columnas["IdProducto"]!.Width = 50;
            }
            // Verifica que la columna "StockActual" existe antes de acceder a ella
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