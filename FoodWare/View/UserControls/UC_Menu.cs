using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.Model.Interfaces; // Para IProductoRepository
using FoodWare.Model.DataAccess; // Para ProductoMockRepository
using FoodWare.Controller.Logic;  // Para MenuController
using FoodWare.Model.Entities;    // Para la clase Platillo
using FoodWare.Validations;      // Para nuestra librería de validación
using FoodWare.View.Helpers;      // Para EstilosApp
using System.Threading.Tasks;   //Para tareas asincronas

namespace FoodWare.View.UserControls
{
    public partial class UC_Menu : UserControl
    {
        private readonly MenuController _controller;

        private readonly RecetaController _recetaController;
        private readonly InventarioController _inventarioController;
        private Platillo? _platilloEnEdicion; // Campo de estado

        public UC_Menu()
        {
            InitializeComponent();
            AplicarEstilos(); // Llamamos a nuestro método de estilos

            // 1. La Vista decide qué repositorio usar.
            IPlatilloRepository repositorioParaUsar = new PlatilloSqlRepository();
            IRecetaRepository recetaRepo = new RecetaSqlRepository();
            IProductoRepository productoRepo = new ProductoSqlRepository();

            // 2. La Vista CREA el controlador y le PASA (inyecta) el repositorio.
            _controller = new MenuController(repositorioParaUsar);
            _recetaController = new RecetaController(recetaRepo);
            _inventarioController = new InventarioController(productoRepo);

            // Agrega más categorías según sea necesario
            cmbCategoria.Items.Add("Entradas");
            cmbCategoria.Items.Add("Platos Fuertes");
            cmbCategoria.Items.Add("Postres");
            cmbCategoria.Items.Add("Bebidas");
            cmbCategoria.Items.Add("Sopas y Ensaladas");
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
            EstilosApp.EstiloLabelModulo(lblPrecio);

            // Cajas de Texto
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);

            // ComboBox
            EstilosApp.EstiloComboBoxModulo(cmbCategoria);

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvMenu);

            // ESTILOS PARA EL PANEL DE EDICION DE RECETA

            // Fondo del Panel de Edición de Receta
            EstilosApp.EstiloPanel(panelEdicionReceta, EstilosApp.ColorFondo);

            // Etiquetas del Panel de Edición de Receta
            EstilosApp.EstiloLabelModulo(lblTituloReceta);
            EstilosApp.EstiloLabelModulo(lblProducto);
            EstilosApp.EstiloLabelModulo(lblCantidadReceta);

            // DataGridView del Panel de Edición de Receta
            EstilosApp.EstiloDataGridView(dgvReceta);

            // ComboBox y TextBox del Panel de Edición de Receta
            EstilosApp.EstiloComboBoxModulo(cmbProductos);
            EstilosApp.EstiloTextBoxModulo(txtCantidadReceta);

            // Botones del Panel de Edición de Receta
            EstilosApp.EstiloBotonModulo(btnAgregarIngrediente);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminarIngrediente);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizarIngrediente);
            EstilosApp.EstiloBotonModuloAlerta(btnVolverAlMenu);
        }

        /// <summary>
        /// Manejador de carga del UserControl. Se convierte en 'async void'
        /// </summary>
        private async void UC_Menu_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Llamamos al método asíncrono
                await CargarGridPlatillosAsync();
            }
        }

        /// <summary>
        /// Pide los platillos al Controlador y actualiza el DataGridView.
        /// </summary>
        /// <summary>
        /// Pide los platillos al Controlador y actualiza el DataGridView de forma asíncrona.
        /// </summary>
        private async Task CargarGridPlatillosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var platillos = await _controller.CargarPlatillosAsync();
                this.dgvMenu.DataSource = null;
                this.dgvMenu.DataSource = platillos;

                // Opcional: ajustar columnas
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
                MessageBox.Show($"Error al cargar el menú: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
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
            txtPrecio.Text = "";
            txtNombre.Focus(); // Pone el cursor de vuelta en el primer campo
        }

        /// <summary>
        /// Carga la lista de todos los productos del inventario en el ComboBox.
        /// </summary>
        private async Task CargarProductosComboBoxAsync()
        {
            try
            {
                // Buena práctica para evitar errores de DataBinding
                cmbProductos.DataSource = null;

                var productos = await _inventarioController.CargarProductosAsync();
                cmbProductos.DataSource = productos;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "IdProducto";
                cmbProductos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga el grid de la receta para un platillo específico.
        /// </summary>
        private async Task CargarGridRecetaAsync(int idPlatillo)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvReceta.DataSource = null;
                var receta = await _recetaController.CargarRecetaDePlatilloAsync(idPlatillo);
                dgvReceta.DataSource = receta;

                // Ocultar columnas que no necesitamos ver
                // Ocultar columnas que no necesitamos ver
                if (dgvReceta.Columns["IdReceta"] is DataGridViewColumn colReceta)
                {
                    colReceta.Visible = false;
                }
                if (dgvReceta.Columns["IdProducto"] is DataGridViewColumn colProducto)
                {
                    colProducto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la receta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Manejador del clic en 'Guardar'. Se convierte en 'async void'.
        /// </summary>
        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista RECOGE los datos.
                string nombre = txtNombre.Text;
                string categoria = cmbCategoria.SelectedItem?.ToString() ?? "";

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // --- Llamada Asíncrona ---
                this.Cursor = Cursors.WaitCursor;

                // 2. La Vista ENVÍA los datos al controlador.
                await _controller.GuardarNuevoPlatilloAsync(nombre, categoria, precio);

                // 3. Si todo salió bien, la Vista se ACTUALIZA.
                MessageBox.Show("¡Platillo guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                await CargarGridPlatillosAsync(); // Recargamos el grid
            }
            catch (ArgumentException aex) // Errores de validación
            {
                MessageBox.Show(aex.Message, "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Errores inesperados
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

                        // --- Llamada Asíncrona ---
                        await _controller.EliminarPlatilloAsync(id);

                        // 5. Actualiza la vista
                        await CargarGridPlatillosAsync();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            // La lógica para actualizar un producto irá aquí más adelante.
            // Por ahora, podemos poner un mensaje temporal.
            MessageBox.Show("La función de actualizar se implementará pronto.", "Función no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Asegura que la fila sea seleccionada con clic derecho ANTES de que se abra el menú.
        /// </summary>
        private void DgvMenu_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 'e.Button' comprueba si fue clic derecho
            // 'e.RowIndex >= 0' comprueba que no se hizo clic en el encabezado
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                // Forzamos la selección de la celda (y fila) bajo el cursor
                dgvMenu.CurrentCell = dgvMenu.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        /// <summary>
        /// Evento principal. Cambia al "Modo Edición" de recetas.
        /// </summary>
        private async void ItemGestionarReceta_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null || dgvMenu.CurrentRow.DataBoundItem is not Platillo platilloSeleccionado)
                return;

            // 1. Guardamos el platillo que estamos editando
            _platilloEnEdicion = platilloSeleccionado;

            // 2. Configuramos el panel de edición
            lblTituloReceta.Text = $"Gestionando Receta de: {_platilloEnEdicion.Nombre}";

            // 3. Cargamos los datos
            await CargarGridRecetaAsync(_platilloEnEdicion.IdPlatillo);
            await CargarProductosComboBoxAsync();

            // 4. Intercambiamos los paneles (El "Switch")
            dgvMenu.Visible = false;
            panelEdicionReceta.Visible = true;
        }

        /// <summary>
        /// Cambia de vuelta al "Modo Vista" de platillos.
        /// </summary>
        private void BtnVolverAlMenu_Click(object sender, EventArgs e)
        {
            // 1. Intercambiamos los paneles
            panelEdicionReceta.Visible = false;
            dgvMenu.Visible = true;

            // 2. Limpiamos la variable de estado y el grid
            _platilloEnEdicion = null;
            dgvReceta.DataSource = null;
        }

        private async void BtnAgregarIngrediente_Click(object sender, EventArgs e)
        {
            // Validamos que tengamos un platillo y un producto seleccionados
            if (_platilloEnEdicion == null || cmbProductos.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un producto.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idPlatillo = _platilloEnEdicion.IdPlatillo;
                int idProducto = (int)cmbProductos.SelectedValue;

                if (!decimal.TryParse(txtCantidadReceta.Text, out decimal cantidad))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Llamamos al controlador
                await _recetaController.GuardarNuevoIngredienteAsync(idPlatillo, idProducto, cantidad);

                // Recargamos el grid de la receta
                await CargarGridRecetaAsync(idPlatillo);

                // Limpiamos los campos
                txtCantidadReceta.Clear();
                cmbProductos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEliminarIngrediente_Click(object sender, EventArgs e)
        {
            if (dgvReceta.CurrentRow == null || dgvReceta.CurrentRow.DataBoundItem is not RecetaDetalle ingrediente)
            {
                MessageBox.Show("Debe seleccionar un ingrediente de la lista de receta.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{ingrediente.NombreProducto}' de la receta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                // --- AÑADIR ESTA VALIDACIÓN ---
                // 1. Asignamos el campo a una variable local.
                Platillo? platillo = _platilloEnEdicion;

                // 2. Comprobamos la variable local.
                if (platillo is null)
                {
                    MessageBox.Show("Error de lógica: No hay ningún platillo en modo de edición.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // --- FIN DE LA VALIDACIÓN ---

                try
                {
                    // Llamamos al controlador
                    await _recetaController.EliminarIngredienteAsync(ingrediente.IdReceta);

                    // Recargamos el grid (Ahora usamos la variable 'platillo' que NO es nula)
                    await CargarGridRecetaAsync(platillo.IdPlatillo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
