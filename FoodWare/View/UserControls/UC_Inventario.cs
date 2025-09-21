using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.View.Helpers;
using FoodWare.Controller.Logic;
using FoodWare.Model.Entities;


namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private InventarioController _controller;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos(); // ¡Llamamos a nuestro método de estilos!
            _controller = new InventarioController();
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
            EstilosApp.EstiloLabelModulo(lblStock);
            EstilosApp.EstiloLabelModulo(lblPrecio);

            // Cajas de Texto
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtCategoria);
            EstilosApp.EstiloTextBoxModulo(txtStock);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvInventario);
        }

        private void UC_Inventario_Load(object sender, EventArgs e)
        {
            if (!DesignMode) // Esto evita que el código se ejecute en el diseñador
            {
                CargarGrid(); // Le decimos que llene la tabla
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista RECOGE datos
                string nombre = txtNombre.Text;
                string cat = txtCategoria.Text;
                int stock = int.Parse(txtStock.Text); // (Se recomienda usar TryParse para validar)
                decimal precio = decimal.Parse(txtPrecio.Text); // (Se recomienda usar TryParse)

                // 2. La Vista ENVÍA datos al controlador
                _controller.GuardarNuevoProducto(nombre, cat, stock, precio);

                // 3. La Vista se ACTUALIZA
                MessageBox.Show("¡Producto guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrid(); // Refresca la tabla
                LimpiarCampos(); // Limpia las cajas de texto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    CargarGrid();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un producto para eliminar.");
            }
        }

        // --- MÉTODOS DE AYUDA DE LA VISTA ---

        /// <summary>
        /// Pide los productos al controlador y actualiza el DataGridView.
        /// </summary>
        private void CargarGrid()
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
            txtCategoria.Text = "";
            txtStock.Text = "";
            txtPrecio.Text = "";
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}