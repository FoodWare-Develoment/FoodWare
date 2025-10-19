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

        public UC_Menu()
        {
            InitializeComponent();
            AplicarEstilos(); // Llamamos a nuestro método de estilos

            // 1. La Vista decide qué repositorio usar.
            IPlatilloRepository repositorioParaUsar = new PlatilloSqlRepository();

            // 2. La Vista CREA el controlador y le PASA (inyecta) el repositorio.
            _controller = new MenuController(repositorioParaUsar);

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
    }
}
