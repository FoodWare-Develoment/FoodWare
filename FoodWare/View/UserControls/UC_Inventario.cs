using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.View.Helpers;      // Para EstilosApp
using FoodWare.Controller.Logic;  // Para MenuController
using FoodWare.Model.Entities;    // Para la clase Platillo
using FoodWare.Validations;      // Para nuestra librería de validación

namespace FoodWare.View.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        private InventarioController _controller;

        public UC_Inventario()
        {
            InitializeComponent();
            AplicarEstilos(); // Llamamos a nuestro método de estilos
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
                CargarGridInventario(); // Le decimos que llene la tabla
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. RECOGE y VALIDA datos (Usando nuestra libreria)
                string nombre = txtNombre.Text;
                string cat = txtCategoria.Text;

                // 2. La Vista VALIDA datos (Usamos nuestra librería)

                // Validación de Nombre
                if (Validar.EsTextoVacio(nombre))
                {
                    MessageBox.Show("El nombre no puede estar vacío.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                //Validación de Categoría
                if (Validar.EsTextoVacio(cat))
                {
                    MessageBox.Show("La categoría no puede estar vacía.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCategoria.Focus();
                    return;
                }

                // Validación de Stock
                if (!Validar.EsEnteroPositivo(txtStock.Text, out int stock))
                {
                    MessageBox.Show("El stock debe ser un número entero positivo.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                // Validación de Precio (refactorizada)
                if (!Validar.EsDecimalPositivo(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número positivo válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // 3. La Vista ENVÍA datos al controlador
                // (Si llegamos aquí, todos los datos son válidos)
                _controller.GuardarNuevoProducto(nombre, cat, stock, precio);

                // 4. La Vista se ACTUALIZA
                MessageBox.Show("¡Producto guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGridInventario(); // Refresca la tabla
                LimpiarCampos(); // <-- ¡Tu mejora de UX!
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
                    CargarGridInventario();
                    LimpiarCampos();
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