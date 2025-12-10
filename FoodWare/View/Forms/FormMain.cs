using System;
using System.Windows.Forms;
using FoodWare.View.UserControls;
using FoodWare.View.Helpers;
using FoodWare.Controller.Logic;

namespace FoodWare.View.Forms
{
    public partial class FormMain : Form
    {
        private readonly string _rolUsuario;

        public FormMain()
        {
            InitializeComponent();
            _rolUsuario = UserSession.NombreRol;

            PersonalizarDiseno();
            AplicarEstilos();
            AplicarSeguridadPorRol();
            CargarInicio();
        }

        private void PersonalizarDiseno()
        {
            panelGPSubmenu.Visible = false;
            panelAdminSubmenu.Visible = false;
            panelAnalisisSubmenu.Visible = false;
        }

        private void AplicarEstilos()
        {
            EstilosApp.EstiloPanel(panelMenu, EstilosApp.ColorMenu);
            EstilosApp.EstiloPanel(panelBarra, EstilosApp.ColorBarra);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);

            EstilosApp.EstiloBotonMenu(btnGP);
            btnGP.Image = Properties.Resources.icons_gestion_24;
            btnGP.Text = " Gestion Principal";

            EstilosApp.EstiloBotonMenu(btnAdmin);
            btnAdmin.Image = Properties.Resources.icons_administracion_24;
            btnAdmin.Text = " Administracion";

            EstilosApp.EstiloBotonMenu(btnAnalisis);
            btnAnalisis.Image = Properties.Resources.icons_analisis_24;
            btnAnalisis.Text = " Analisis";

            EstilosApp.EstiloBotonMenu(btnConfig);
            btnConfig.Image = Properties.Resources.icons_configuracion_24;
            btnConfig.Text = " Configuracion";

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

            EstilosApp.EstiloBotonSubmenu(btnEmpleados);
            btnEmpleados.Image = Properties.Resources.icons_empleados_24;
            btnEmpleados.Text = " Empleados";

            EstilosApp.EstiloBotonSubmenu(btnFinanzas);
            btnFinanzas.Image = Properties.Resources.icons_finanzas_24;
            btnFinanzas.Text = " Finanzas";

            EstilosApp.EstiloBotonSubmenu(btnReportes);
            btnReportes.Image = Properties.Resources.icons_reportes_24;
            btnReportes.Text = " Reportes";
        }

        private void AplicarSeguridadPorRol()
        {
            // 1. Ocultar todo lo sensible por defecto
            btnInventario.Visible = false;
            btnAdmin.Visible = false;
            btnAnalisis.Visible = false;
            btnConfig.Visible = false;
            btnFinanzas.Visible = false;

            // 2. Módulos operativos básicos siempre visibles
            btnGP.Visible = true;
            btnVentas.Visible = true;
            btnMenu.Visible = true;

            switch (_rolUsuario)
            {
                case "Administrador":
                case "Gerente":
                    // Acceso Total
                    btnInventario.Visible = true;
                    btnAdmin.Visible = true;
                    btnFinanzas.Visible = true;
                    btnAnalisis.Visible = true;
                    btnConfig.Visible = true;
                    break;

                case "Cajero":
                    // Tarea: Ver Ventas, Ver Menú, Cerrar Caja (Finanzas)
                    // NO ve inventario, ni empleados, ni reportes.
                    btnFinanzas.Visible = true; // Permitimos entrar (UC_Finanzas se blindará solo)
                    btnAdmin.Visible = true; // Mostramos el padre
                    btnEmpleados.Visible = false; // Ocultamos el hermano prohibido
                    break;

                case "Chef":
                    btnInventario.Visible = true;
                    // Chef ve Inventario y Menú. No ve Finanzas.
                    break;

                case "Mesero":
                    // Solo Ventas y Menú (Configuración default)
                    break;

                default:
                    btnGP.Visible = false;
                    btnAdmin.Visible = false;
                    btnAnalisis.Visible = false;
                    btnConfig.Visible = false;
                    break;
            }
        }

        private void CargarInicio() { AbrirModulo(new UC_Inicio()); }

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

    }
}