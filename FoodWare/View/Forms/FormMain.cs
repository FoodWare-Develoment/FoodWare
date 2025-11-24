using System;
using System.Windows.Forms;
using FoodWare.View.UserControls;
using FoodWare.View.Helpers;
using FoodWare.Controller.Logic;

namespace FoodWare.View.Forms
{
    public partial class FormMain : Form
    {
        // Almacena el rol del usuario que inició sesión
        private readonly string _rolUsuario;

        /// <summary>
        /// Constructor modificado para aceptar el rol del usuario.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            // Leemos el rol desde la sesión estática
            _rolUsuario = UserSession.NombreRol;

            PersonalizarDiseno();
            AplicarEstilos();
            AplicarSeguridadPorRol(); // Aplica seguridad basada en el rol
            CargarInicio();
        }

        private void PersonalizarDiseno()
        {
            panelGPSubmenu.Visible = false;
            panelAdminSubmenu.Visible = false;
            panelAnalisisSubmenu.Visible = false;
            txtBusqueda.PlaceholderText = "Buscar en FoodWare";
        }

        private void AplicarEstilos()
        {
            EstilosApp.EstiloPanel(panelMenu, EstilosApp.ColorMenu);
            EstilosApp.EstiloPanel(panelBarra, EstilosApp.ColorBarra);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);

            // Botón Gestión Principal
            EstilosApp.EstiloBotonMenu(btnGP);
            btnGP.Image = Properties.Resources.icons_gestion_24;
            btnGP.Text = " Gestion Principal";

            // Botón Administración
            EstilosApp.EstiloBotonMenu(btnAdmin);
            btnAdmin.Image = Properties.Resources.icons_administracion_24;
            btnAdmin.Text = " Administracion";

            // Botón Análisis
            EstilosApp.EstiloBotonMenu(btnAnalisis);
            btnAnalisis.Image = Properties.Resources.icons_analisis_24;
            btnAnalisis.Text = " Analisis";

            // Botón Configuración
            EstilosApp.EstiloBotonMenu(btnConfig);
            btnConfig.Image = Properties.Resources.icons_configuracion_24;
            btnConfig.Text = " Configuracion";

            // --- Submenú Gestión Principal ---
            EstilosApp.EstiloBotonSubmenu(btnInicio);
            btnInicio.Image = Properties.Resources.icons_inicio_24;
            btnInicio.Text = " Inicio";

            EstilosApp.EstiloBotonSubmenu(btnInventario);
            btnInventario.Image = Properties.Resources.icons_inventario_24;
            btnInventario.Text = " Inventario";

            EstilosApp.EstiloBotonSubmenu(btnMenu);
            btnMenu.Image = Properties.Resources.icons_menu_24;
            btnMenu.Text = " Menu";

            EstilosApp.EstiloBotonSubmenu(btnVentas);
            btnVentas.Image = Properties.Resources.icons_ventas_24;
            btnVentas.Text = " Ventas";

            // --- Submenú Administración ---
            EstilosApp.EstiloBotonSubmenu(btnEmpleados);
            btnEmpleados.Image = Properties.Resources.icons_empleados_24;
            btnEmpleados.Text = " Empleados";

            EstilosApp.EstiloBotonSubmenu(btnFinanzas);
            btnFinanzas.Image = Properties.Resources.icons_finanzas_24;
            btnFinanzas.Text = " Finanzas";

            // --- Submenú Análisis ---
            EstilosApp.EstiloBotonSubmenu(btnReportes);
            btnReportes.Image = Properties.Resources.icons_reportes_24;
            btnReportes.Text = " Reportes";
        }

        /// <summary>
        /// Oculta o muestra botones del menú según el rol del usuario.
        /// </summary>
        private void AplicarSeguridadPorRol()
        {

            // Primero, ocultamos todo lo sensible por defecto (permisos de Mesero)
            btnInventario.Visible = false;
            btnAdmin.Visible = false;
            btnAnalisis.Visible = false;
            btnConfig.Visible = false;

            // Los meseros SÍ ven esto
            btnGP.Visible = true;
            btnVentas.Visible = true;
            btnMenu.Visible = true;

            // Usamos un switch para añadir permisos
            switch (_rolUsuario)
            {
                case "Administrador":
                case "Gerente":
                    // Admin y Gerente ven todo
                    btnInventario.Visible = true;
                    btnAdmin.Visible = true;
                    btnAnalisis.Visible = true;
                    btnConfig.Visible = true;
                    break;

                case "Chef":
                    // Chef ve Inventario y Menú
                    btnInventario.Visible = true;
                    btnMenu.Visible = true;
                    break;

                case "Mesero":
                    // Ya está configurado por defecto
                    break;

                default:
                    // Si el rol es desconocido, ocultamos todo por seguridad
                    btnGP.Visible = false;
                    btnAdmin.Visible = false;
                    btnAnalisis.Visible = false;
                    btnConfig.Visible = false;
                    break;
            }
        }

        private void CargarInicio()
        {
            AbrirModulo(new UC_Inicio());
        }

        private void OcultarSubmenu()
        {
            if (panelGPSubmenu.Visible) panelGPSubmenu.Visible = false;
            if (panelAdminSubmenu.Visible) panelAdminSubmenu.Visible = false;
            if (panelAnalisisSubmenu.Visible) panelAnalisisSubmenu.Visible = false;
        }

        private void MostrarSubmenu(Panel submenu)
        {
            if (!submenu.Visible)
            {
                OcultarSubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void AbrirModulo(UserControl modulo)
        {
            panelContenido.Controls.Clear();
            modulo.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(modulo);
            modulo.BringToFront();
        }

        // --- Event Handlers ---
        private void BtnGP_Click(object sender, EventArgs e) => MostrarSubmenu(panelGPSubmenu);
        private void BtnAdmin_Click(object sender, EventArgs e) => MostrarSubmenu(panelAdminSubmenu);
        private void BtnAnalisis_Click(object sender, EventArgs e) => MostrarSubmenu(panelAnalisisSubmenu);
        private void BtnInicio_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inicio()); OcultarSubmenu(); }
        private void BtnInventario_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inventario()); OcultarSubmenu(); }
        private void BtnMenu_Click(object sender, EventArgs e) { AbrirModulo(new UC_Menu()); OcultarSubmenu(); }
        private void BtnVentas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Ventas()); OcultarSubmenu(); }
        private void BtnEmpleados_Click(object sender, EventArgs e) { AbrirModulo(new UC_Empleados()); OcultarSubmenu(); }
        private void BtnFinanzas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Finanzas()); OcultarSubmenu(); }
        private void BtnReportes_Click(object sender, EventArgs e) { AbrirModulo(new UC_Reportes()); OcultarSubmenu(); }
        private void BtnConfig_Click(object sender, EventArgs e) { AbrirModulo(new UC_Configuracion()); OcultarSubmenu(); }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                MessageBox.Show("Buscando: " + txtBusqueda.Text);
            }
        }
    }
}