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
            // (btnLimpiar puedes usar EstiloBotonMenu o crear uno nuevo 'EstiloBotonSecundario')

            // Grid
            EstilosApp.EstiloDataGridView(dgvInventario);
        }

        // ... (Aquí va el resto del código del Paso 5d) ...
    }
}