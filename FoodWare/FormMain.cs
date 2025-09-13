using System;
using System.Windows.Forms;
using FoodWare.UserControls;

namespace FoodWare
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            PersonalizarDiseno();
            AplicarEstilos();
            CargarInicio();
        }

        private void PersonalizarDiseno()
        {
            // Ocultar submenús al inicio
            panelGPSubmenu.Visible = false;
            panelAdminSubmenu.Visible = false;
            panelAnalisisSubmenu.Visible = false;

            // Placeholder de búsqueda
            try { txtBusqueda.PlaceholderText = "Buscar en FoodWare"; } catch { }
        }

        private void AplicarEstilos()
        {
            // Paneles
            EstilosApp.EstiloPanel(panelMenu, EstilosApp.ColorMenu);
            EstilosApp.EstiloPanel(panelBarra, EstilosApp.ColorBarra);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);

            // Botones principales
            EstilosApp.EstiloBotonMenu(btnGP);
            EstilosApp.EstiloBotonMenu(btnAdmin);
            EstilosApp.EstiloBotonMenu(btnAnalisis);
            EstilosApp.EstiloBotonMenu(btnConfig);

            // Botones secundarios
            EstilosApp.EstiloBotonSubmenu(btnInicio);
            EstilosApp.EstiloBotonSubmenu(btnInventario);
            EstilosApp.EstiloBotonSubmenu(btnMenu);
            EstilosApp.EstiloBotonSubmenu(btnVentas);
            EstilosApp.EstiloBotonSubmenu(btnEmpleados);
            EstilosApp.EstiloBotonSubmenu(btnFinanzas);
            EstilosApp.EstiloBotonSubmenu(btnReportes);
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

        // Botones padres
        private void btnGP_Click(object sender, EventArgs e) => MostrarSubmenu(panelGPSubmenu);
        private void btnAdmin_Click(object sender, EventArgs e) => MostrarSubmenu(panelAdminSubmenu);
        private void btnAnalisis_Click(object sender, EventArgs e) => MostrarSubmenu(panelAnalisisSubmenu);

        // Botones hijos
        private void btnInicio_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inicio()); OcultarSubmenu(); }
        private void btnInventario_Click(object sender, EventArgs e) { AbrirModulo(new UC_Inventario()); OcultarSubmenu(); }
        private void btnMenu_Click(object sender, EventArgs e) { AbrirModulo(new UC_Menu()); OcultarSubmenu(); }
        private void btnVentas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Ventas()); OcultarSubmenu(); }
        private void btnEmpleados_Click(object sender, EventArgs e) { AbrirModulo(new UC_Empleados()); OcultarSubmenu(); }
        private void btnFinanzas_Click(object sender, EventArgs e) { AbrirModulo(new UC_Finanzas()); OcultarSubmenu(); }
        private void btnReportes_Click(object sender, EventArgs e) { AbrirModulo(new UC_Reportes()); OcultarSubmenu(); }
        private void btnConfig_Click(object sender, EventArgs e) { AbrirModulo(new UC_Configuracion()); OcultarSubmenu(); }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                // Aquí irá tu lógica de búsqueda global
            }
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelContenido_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
