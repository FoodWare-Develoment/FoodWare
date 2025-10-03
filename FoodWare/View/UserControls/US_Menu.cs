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
    public partial class UC_Menu : UserControl
    {
        private readonly MenuController _controller;

        public UC_Menu()
        {
            InitializeComponent();
            AplicarEstilos(); // Llamamos a nuestro método de estilos

            // 1. La Vista decide qué repositorio usar. Por ahora, el FALSO (Mock).
            IPlatilloRepository repositorioParaUsar = new PlatilloMockRepository();

            // 2. La Vista CREA el controlador y le PASA (inyecta) el repositorio.
            _controller = new MenuController(repositorioParaUsar);
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
            EstilosApp.EstiloTextBoxModulo(txtCategoria);
            EstilosApp.EstiloTextBoxModulo(txtPrecio);

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvMenu);
        }

        private void UC_Menu_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                CargarGridPlatillos();
            }
        }

        /// <summary>
        /// Pide los platillos al Controlador y actualiza el DataGridView.
        /// </summary>
        private void CargarGridPlatillos()
        {
            var platillos = _controller.CargarPlatillos();
            this.dgvMenu.DataSource = null; // Limpia el grid
            this.dgvMenu.DataSource = platillos; // Carga los nuevos datos

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

        /// <summary>
        /// Limpia las cajas de texto del formulario.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCategoria.Text = "";
            txtPrecio.Text = "";
            txtNombre.Focus(); // Pone el cursor de vuelta en el primer campo
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. La Vista ahora solo RECOGE los datos.
                // La validación compleja ya no es su responsabilidad.
                string nombre = txtNombre.Text;
                string cat = txtCategoria.Text;

                // Se convierten los valores aquí, ya que el controlador espera los tipos correctos.
                // Si la conversión falla, la vista puede manejarlo localmente.

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // 2. La Vista ENVÍA los datos al controlador.
                _controller.GuardarNuevoPlatillo(nombre, cat, precio);

                // 3. Si todo salió bien (no hubo excepciones), la Vista se ACTUALIZA.
                MessageBox.Show("¡Platillo guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGridPlatillos();
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
            // 1. Verifica si hay una fila seleccionada
            if (this.dgvMenu.CurrentRow != null && this.dgvMenu.CurrentRow.DataBoundItem is Platillo platilloSeleccionado)
            {
                // 2. Obtenemos el ID y el Nombre del platillo seleccionado
                int id = platilloSeleccionado.IdPlatillo;
                string nombre = platilloSeleccionado.Nombre;

                // 3. Pide confirmación al usuario
                var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{nombre}'?",
                                               "Confirmar eliminación",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        // 4. Envía la orden al controlador
                        _controller.EliminarPlatillo(id);

                        // 5. Actualiza la vista
                        CargarGridPlatillos();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un platillo de la lista para eliminar.", "Ningún platillo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
