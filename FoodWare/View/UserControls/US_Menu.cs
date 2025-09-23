using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.View.Helpers;      // Para EstilosApp
using FoodWare.Controller.Logic;  // Para MenuController
using FoodWare.Model.Entities;    // Para la clase Platillo
using FoodWare.Validations;      // Para nuestra librería de validación

namespace FoodWare.View.UserControls
{
    public partial class UC_Menu : UserControl
    {
        private MenuController _controller;

        public UC_Menu()
        {
            InitializeComponent();
            _controller = new MenuController();
            AplicarEstilos();
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
                // 1. RECOGE y VALIDA datos (Usando nuestra libreria)
                string nombre = txtNombre.Text;
                string cat = txtCategoria.Text;

                // Validamos nombre
                if (Validar.EsTextoVacio(nombre))
                {
                    MessageBox.Show("El nombre no puede estar vacío.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                // Validamos categoría
                if (Validar.EsTextoVacio(cat))
                {
                    MessageBox.Show("La categoría no puede estar vacía.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCategoria.Focus();
                    return;
                }

                // Usamos la versión de la librería que nos da el decimal
                if (!Validar.EsDecimalPositivo(txtPrecio.Text, out decimal precio))
                {
                    MessageBox.Show("Precio debe ser un número positivo válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }

                // 2. ENVÍA datos al controlador
                _controller.GuardarNuevoPlatillo(nombre, cat, precio);

                // 3. ACTUALIZA la Vista
                MessageBox.Show("¡Platillo guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGridPlatillos(); // Refresca el grid
                LimpiarCampos();       // Limpia los campos
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
