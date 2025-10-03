namespace FoodWare.View.Forms
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelBarra;
        private System.Windows.Forms.Panel panelContenido;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.Panel panelBusqueda; // contenedor para el buscador

        private System.Windows.Forms.Button btnGP;
        private System.Windows.Forms.Panel panelGPSubmenu;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnVentas;

        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Panel panelAdminSubmenu;
        private System.Windows.Forms.Button btnEmpleados;
        private System.Windows.Forms.Button btnFinanzas;

        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Panel panelAnalisisSubmenu;
        private System.Windows.Forms.Button btnReportes;

        private System.Windows.Forms.Button btnConfig;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnConfig = new Button();
            panelAnalisisSubmenu = new Panel();
            btnReportes = new Button();
            btnAnalisis = new Button();
            panelAdminSubmenu = new Panel();
            btnFinanzas = new Button();
            btnEmpleados = new Button();
            btnAdmin = new Button();
            panelGPSubmenu = new Panel();
            btnVentas = new Button();
            btnMenu = new Button();
            btnInventario = new Button();
            btnInicio = new Button();
            btnGP = new Button();
            panelBarra = new Panel();
            lblTitulo = new Label();
            panelBusqueda = new Panel();
            txtBusqueda = new TextBox();
            panelContenido = new Panel();
            panelMenu.SuspendLayout();
            panelAnalisisSubmenu.SuspendLayout();
            panelAdminSubmenu.SuspendLayout();
            panelGPSubmenu.SuspendLayout();
            panelBarra.SuspendLayout();
            panelBusqueda.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.AutoScroll = true;
            panelMenu.Controls.Add(btnConfig);
            panelMenu.Controls.Add(panelAnalisisSubmenu);
            panelMenu.Controls.Add(btnAnalisis);
            panelMenu.Controls.Add(panelAdminSubmenu);
            panelMenu.Controls.Add(btnAdmin);
            panelMenu.Controls.Add(panelGPSubmenu);
            panelMenu.Controls.Add(btnGP);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 56);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(240, 664);
            panelMenu.TabIndex = 2;
            // 
            // btnConfig
            // 
            btnConfig.Dock = DockStyle.Bottom;
            btnConfig.Location = new Point(0, 641);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(240, 23);
            btnConfig.TabIndex = 0;
            btnConfig.Text = "Configuracion";
            btnConfig.Click += BtnConfig_Click;
            // 
            // panelAnalisisSubmenu
            // 
            panelAnalisisSubmenu.Controls.Add(btnReportes);
            panelAnalisisSubmenu.Dock = DockStyle.Top;
            panelAnalisisSubmenu.Location = new Point(0, 269);
            panelAnalisisSubmenu.Name = "panelAnalisisSubmenu";
            panelAnalisisSubmenu.Size = new Size(240, 100);
            panelAnalisisSubmenu.TabIndex = 1;
            // 
            // btnReportes
            // 
            btnReportes.Dock = DockStyle.Top;
            btnReportes.Location = new Point(0, 0);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(240, 23);
            btnReportes.TabIndex = 0;
            btnReportes.Text = "Reportes";
            btnReportes.Click += BtnReportes_Click;
            // 
            // btnAnalisis
            // 
            btnAnalisis.Dock = DockStyle.Top;
            btnAnalisis.Location = new Point(0, 246);
            btnAnalisis.Name = "btnAnalisis";
            btnAnalisis.Size = new Size(240, 23);
            btnAnalisis.TabIndex = 2;
            btnAnalisis.Text = "Analisis";
            btnAnalisis.Click += BtnAnalisis_Click;
            // 
            // panelAdminSubmenu
            // 
            panelAdminSubmenu.Controls.Add(btnFinanzas);
            panelAdminSubmenu.Controls.Add(btnEmpleados);
            panelAdminSubmenu.Dock = DockStyle.Top;
            panelAdminSubmenu.Location = new Point(0, 146);
            panelAdminSubmenu.Name = "panelAdminSubmenu";
            panelAdminSubmenu.Size = new Size(240, 100);
            panelAdminSubmenu.TabIndex = 3;
            // 
            // btnFinanzas
            // 
            btnFinanzas.Dock = DockStyle.Top;
            btnFinanzas.Location = new Point(0, 23);
            btnFinanzas.Name = "btnFinanzas";
            btnFinanzas.Size = new Size(240, 23);
            btnFinanzas.TabIndex = 0;
            btnFinanzas.Text = "Finanzas";
            btnFinanzas.Click += BtnFinanzas_Click;
            // 
            // btnEmpleados
            // 
            btnEmpleados.Dock = DockStyle.Top;
            btnEmpleados.Location = new Point(0, 0);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(240, 23);
            btnEmpleados.TabIndex = 1;
            btnEmpleados.Text = "Empleados";
            btnEmpleados.Click += BtnEmpleados_Click;
            // 
            // btnAdmin
            // 
            btnAdmin.Dock = DockStyle.Top;
            btnAdmin.Location = new Point(0, 123);
            btnAdmin.Name = "btnAdmin";
            btnAdmin.Size = new Size(240, 23);
            btnAdmin.TabIndex = 4;
            btnAdmin.Text = "Administracion";
            btnAdmin.Click += BtnAdmin_Click;
            // 
            // panelGPSubmenu
            // 
            panelGPSubmenu.Controls.Add(btnVentas);
            panelGPSubmenu.Controls.Add(btnMenu);
            panelGPSubmenu.Controls.Add(btnInventario);
            panelGPSubmenu.Controls.Add(btnInicio);
            panelGPSubmenu.Dock = DockStyle.Top;
            panelGPSubmenu.Location = new Point(0, 23);
            panelGPSubmenu.Name = "panelGPSubmenu";
            panelGPSubmenu.Size = new Size(240, 100);
            panelGPSubmenu.TabIndex = 5;
            // 
            // btnVentas
            // 
            btnVentas.Dock = DockStyle.Top;
            btnVentas.Location = new Point(0, 69);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(240, 23);
            btnVentas.TabIndex = 0;
            btnVentas.Text = "Ventas";
            btnVentas.Click += BtnVentas_Click;
            // 
            // btnMenu
            // 
            btnMenu.Dock = DockStyle.Top;
            btnMenu.Location = new Point(0, 46);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(240, 23);
            btnMenu.TabIndex = 1;
            btnMenu.Text = "Menu";
            btnMenu.Click += BtnMenu_Click;
            // 
            // btnInventario
            // 
            btnInventario.Dock = DockStyle.Top;
            btnInventario.Location = new Point(0, 23);
            btnInventario.Name = "btnInventario";
            btnInventario.Size = new Size(240, 23);
            btnInventario.TabIndex = 2;
            btnInventario.Text = "Inventario";
            btnInventario.Click += BtnInventario_Click;
            // 
            // btnInicio
            // 
            btnInicio.Dock = DockStyle.Top;
            btnInicio.Location = new Point(0, 0);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(240, 23);
            btnInicio.TabIndex = 3;
            btnInicio.Text = "Inicio";
            btnInicio.Click += BtnInicio_Click;
            // 
            // btnGP
            // 
            btnGP.Dock = DockStyle.Top;
            btnGP.Location = new Point(0, 0);
            btnGP.Name = "btnGP";
            btnGP.Size = new Size(240, 23);
            btnGP.TabIndex = 6;
            btnGP.Text = "Gestion Principal";
            btnGP.Click += BtnGP_Click;
            // 
            // panelBarra
            // 
            panelBarra.Controls.Add(lblTitulo);
            panelBarra.Controls.Add(panelBusqueda);
            panelBarra.Dock = DockStyle.Top;
            panelBarra.Location = new Point(0, 0);
            panelBarra.Name = "panelBarra";
            panelBarra.Size = new Size(1200, 56);
            panelBarra.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Bauhaus 93", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(150, 56);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "FoodWare";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelBusqueda
            // 
            panelBusqueda.Controls.Add(txtBusqueda);
            panelBusqueda.Dock = DockStyle.Right;
            panelBusqueda.Location = new Point(940, 0);
            panelBusqueda.Name = "panelBusqueda";
            panelBusqueda.Padding = new Padding(10, 14, 10, 14);
            panelBusqueda.Size = new Size(260, 56);
            panelBusqueda.TabIndex = 0;
            // 
            // txtBusqueda
            // 
            txtBusqueda.Dock = DockStyle.Fill;
            txtBusqueda.Location = new Point(10, 14);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.Size = new Size(240, 27);
            txtBusqueda.TabIndex = 101;
            txtBusqueda.KeyDown += TxtBusqueda_KeyDown;
            // 
            // panelContenido
            // 
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(240, 56);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(960, 664);
            panelContenido.TabIndex = 0;
            // 
            // FormMain
            // 
            ClientSize = new Size(1200, 720);
            Controls.Add(panelContenido);
            Controls.Add(panelMenu);
            Controls.Add(panelBarra);
            MinimumSize = new Size(900, 600);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FoodWare";
            panelMenu.ResumeLayout(false);
            panelAnalisisSubmenu.ResumeLayout(false);
            panelAdminSubmenu.ResumeLayout(false);
            panelGPSubmenu.ResumeLayout(false);
            panelBarra.ResumeLayout(false);
            panelBusqueda.ResumeLayout(false);
            panelBusqueda.PerformLayout();
            ResumeLayout(false);
        }
    }
}
