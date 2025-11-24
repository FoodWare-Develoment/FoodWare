using System;
using System.Drawing; // Aunque ya no se usa aquí, es estándar dejarlo por si acaso.
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    /// <summary>
    /// Módulo (UserControl) que actúa como la pantalla principal o Dashboard.
    /// Actualmente muestra una imagen de bienvenida (pictureBoxBienvenida) definida en el Diseñador.
    /// </summary>
    public partial class UC_Inicio : UserControl
    {
        /// <summary>
        /// Inicializa el módulo de Inicio.
        /// </summary>
        public UC_Inicio()
        {
            InitializeComponent();
        }

        
    }
}