using System;
using System.Windows.Forms;
using FoodWare.View.UserControls;
using FoodWare.View.Helpers;

namespace FoodWare.View.Forms
{
    /// <summary>
    /// Formulario principal y contenedor (Shell) de la aplicaci�n FoodWare.
    /// Gestiona la navegaci�n del men� lateral y la carga din�mica de m�dulos (UserControls) 
    /// en el panel de contenido principal.
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Inicializa el formulario principal.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            PersonalizarDiseno(); // Configura el estado visual inicial de los controles.
            AplicarEstilos();     // Aplica la paleta de colores centralizada desde EstilosApp.
            CargarInicio();       // Carga el UserControl por defecto (Dashboard/Inicio).
        }

        /// <summary>
        /// Establece el estado visual inicial de la UI al arrancar la aplicaci�n.
        /// (Oculta submen�s, define placeholders, etc.)
        /// </summary>
        private void PersonalizarDiseno()
        {
            // Ocultar submen�s al inicio
            panelGPSubmenu.Visible = false;
            panelAdminSubmenu.Visible = false;
            panelAnalisisSubmenu.Visible = false;

            // Placeholder de b�squeda
            txtBusqueda.PlaceholderText = "Buscar en FoodWare";
        }

        /// <summary>
        /// Aplica los estilos visuales centralizados desde la clase EstilosApp
        /// a los componentes de este formulario (paneles y botones).
        /// </summary>
        private void AplicarEstilos()
        {
            // Paneles
            EstilosApp.EstiloPanel(panelMenu, EstilosApp.ColorMenu);
            EstilosApp.EstiloPanel(panelBarra, EstilosApp.ColorBarra);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);

            // Botones principales (Nivel 1)
            EstilosApp.EstiloBotonMenu(btnGP);
            EstilosApp.EstiloBotonMenu(btnAdmin);
            EstilosApp.EstiloBotonMenu(btnAnalisis);
            EstilosApp.EstiloBotonMenu(btnConfig);

            // Botones secundarios (Nivel 2 - Submen�s)
            EstilosApp.EstiloBotonSubmenu(btnInicio);
            EstilosApp.EstiloBotonSubmenu(btnInventario);
            EstilosApp.EstiloBotonSubmenu(btnMenu);
            EstilosApp.EstiloBotonSubmenu(btnVentas);
            EstilosApp.EstiloBotonSubmenu(btnEmpleados);
            EstilosApp.EstiloBotonSubmenu(btnFinanzas);
            EstilosApp.EstiloBotonSubmenu(btnReportes);
        }

        /// <summary>
        /// Carga el m�dulo (UserControl) de Inicio por defecto en el panel de contenido.
        /// </summary>
        private void CargarInicio()
        {
            AbrirModulo(new UC_Inicio());
        }

        // --- L�GICA DE NAVEGACI�N DEL MEN� ---

        /// <summary>
        /// M�todo de ayuda para cerrar todos los submen�s abiertos.
        /// </summary>
        private void OcultarSubmenu()
        {
            if (panelGPSubmenu.Visible) panelGPSubmenu.Visible = false;
            if (panelAdminSubmenu.Visible) panelAdminSubmenu.Visible = false;
            if (panelAnalisisSubmenu.Visible) panelAnalisisSubmenu.Visible = false;
        }

        /// <summary>
        /// Gestiona la l�gica de acorde�n para mostrar/ocultar un submen� espec�fico.
        /// </summary>
        /// <param name="submenu">El Panel (que act�a como submen�) que se debe mostrar u ocultar.</param>
        private void MostrarSubmenu(Panel submenu)
        {
            if (!submenu.Visible)
            {
                // Si est� cerrado: cerramos cualquier otro abierto y abrimos este.
                OcultarSubmenu();
                submenu.Visible = true;
            }
            else
            {
                // Si ya estaba abierto: lo cerramos.
                submenu.Visible = false;
            }
        }

        /// <summary>
        /// M�todo principal para cargar din�micamente un m�dulo (UserControl) en el �rea de contenido.
        /// </summary>
        /// <param name="modulo">La instancia del UserControl que se va a cargar.</param>
        private void AbrirModulo(UserControl modulo)
        {
            panelContenido.Controls.Clear(); // Limpia el m�dulo anterior
            modulo.Dock = DockStyle.Fill; // Asegura que el control llene el panel
            panelContenido.Controls.Add(modulo); // A�ade el nuevo control
            modulo.BringToFront(); // Lo trae al frente
        }


        // --- EVENT HANDLERS (EVENTOS DE CLICK) ---

        // Botones padres (Categor�as)
        private void BtnGP_Click(object sender, EventArgs e) => MostrarSubmenu(panelGPSubmenu);
        private void BtnAdmin_Click(object sender, EventArgs e) => MostrarSubmenu(panelAdminSubmenu);
        private void BtnAnalisis_Click(object sender, EventArgs e) => MostrarSubmenu(panelAnalisisSubmenu);

        // Botones hijos (M�dulos)
        private void BtnInicio_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inicio()); OcultarSubmenu(); }
        private void BtnInventario_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inventario()); OcultarSubmenu(); }
        private void BtnMenu_Click(object sender, EventArgs e) { AbrirModulo(new UC_Menu()); OcultarSubmenu(); }
        private void BtnVentas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Ventas()); OcultarSubmenu(); }
        private void BtnEmpleados_Click(object sender, EventArgs e) { AbrirModulo(new UC_Empleados()); OcultarSubmenu(); }
        private void BtnFinanzas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Finanzas()); OcultarSubmenu(); }
        private void BtnReportes_Click(object sender, EventArgs e) { AbrirModulo(new UC_Reportes()); OcultarSubmenu(); }
        private void BtnConfig_Click(object sender, EventArgs e) { AbrirModulo(new UC_Configuracion()); OcultarSubmenu(); }


        // --- EVENTOS DE B�SQUEDA ---

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta la tecla "Enter" en el cuadro de b�squeda
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido "ding" de Windows

                // Implementar la l�gica de b�squeda global aqu�.
                MessageBox.Show("Buscando: " + txtBusqueda.Text); // Acci�n placeholder
            }
        }
    }
}