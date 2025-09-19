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
            // Este método carga TODOS los componentes definidos en el archivo .Designer.cs
            // (En este caso, carga el 'pictureBoxBienvenida').
            InitializeComponent();
        }

        // TODO: (Guía de Proyecto): Este UserControl (con la imagen de bienvenida)
        // sigue siendo un marcador de posición. En la fase 2 del proyecto,
        // este control debe ser reemplazado por el Dashboard real de KPIs
        // (Ventas del día, Mesas abiertas, Alertas de inventario, etc.).
    }
}