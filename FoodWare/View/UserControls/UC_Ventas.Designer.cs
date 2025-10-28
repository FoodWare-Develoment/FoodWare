namespace FoodWare.View.UserControls
{
    partial class UC_Ventas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tlpPrincipal = new TableLayoutPanel();
            panelComanda = new Panel();
            dgvComanda = new DataGridView();
            flpGestionTicket = new FlowLayoutPanel();
            btnQtyAumentar = new Button();
            btnQtyDisminuir = new Button();
            btnEliminarPlatillo = new Button();
            lblTotal = new Label();
            btnRegistrarVenta = new Button();
            panelMenuSeleccion = new Panel();
            flpCategorias = new FlowLayoutPanel();
            flpPlatillos = new FlowLayoutPanel();
            tlpPrincipal.SuspendLayout();
            panelComanda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvComanda).BeginInit();
            flpGestionTicket.SuspendLayout();
            panelMenuSeleccion.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 2;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64.99422F));
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35.00578F));
            tlpPrincipal.Controls.Add(panelComanda, 1, 0);
            tlpPrincipal.Controls.Add(panelMenuSeleccion, 0, 0);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.RowCount = 1;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPrincipal.Size = new Size(960, 664);
            tlpPrincipal.TabIndex = 0;
            // 
            // panelComanda
            // 
            panelComanda.AutoScroll = true;
            panelComanda.Controls.Add(dgvComanda);
            panelComanda.Controls.Add(flpGestionTicket);
            panelComanda.Controls.Add(lblTotal);
            panelComanda.Controls.Add(btnRegistrarVenta);
            panelComanda.Dock = DockStyle.Fill;
            panelComanda.Location = new Point(626, 3);
            panelComanda.Name = "panelComanda";
            panelComanda.Size = new Size(331, 658);
            panelComanda.TabIndex = 0;
            // 
            // dgvComanda
            // 
            dgvComanda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComanda.Dock = DockStyle.Fill;
            dgvComanda.Location = new Point(0, 0);
            dgvComanda.Name = "dgvComanda";
            dgvComanda.RowHeadersWidth = 51;
            dgvComanda.Size = new Size(331, 518);
            dgvComanda.TabIndex = 3;
            // 
            // flpGestionTicket
            // 
            flpGestionTicket.Controls.Add(btnQtyAumentar);
            flpGestionTicket.Controls.Add(btnQtyDisminuir);
            flpGestionTicket.Controls.Add(btnEliminarPlatillo);
            flpGestionTicket.Dock = DockStyle.Bottom;
            flpGestionTicket.FlowDirection = FlowDirection.RightToLeft;
            flpGestionTicket.Location = new Point(0, 518);
            flpGestionTicket.Name = "flpGestionTicket";
            flpGestionTicket.Padding = new Padding(5);
            flpGestionTicket.Size = new Size(331, 60);
            flpGestionTicket.TabIndex = 2;
            // 
            // btnQtyAumentar
            // 
            btnQtyAumentar.Location = new Point(218, 8);
            btnQtyAumentar.Name = "btnQtyAumentar";
            btnQtyAumentar.Size = new Size(100, 40);
            btnQtyAumentar.TabIndex = 0;
            btnQtyAumentar.Text = "Añadir (+)";
            btnQtyAumentar.UseVisualStyleBackColor = true;
            // 
            // btnQtyDisminuir
            // 
            btnQtyDisminuir.Location = new Point(112, 8);
            btnQtyDisminuir.Name = "btnQtyDisminuir";
            btnQtyDisminuir.Size = new Size(100, 40);
            btnQtyDisminuir.TabIndex = 1;
            btnQtyDisminuir.Text = "Quitar (-)";
            btnQtyDisminuir.UseVisualStyleBackColor = true;
            // 
            // btnEliminarPlatillo
            // 
            btnEliminarPlatillo.Location = new Point(6, 8);
            btnEliminarPlatillo.Name = "btnEliminarPlatillo";
            btnEliminarPlatillo.Size = new Size(100, 40);
            btnEliminarPlatillo.TabIndex = 2;
            btnEliminarPlatillo.Text = "Eliminar";
            btnEliminarPlatillo.UseVisualStyleBackColor = true;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.Location = new Point(0, 578);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(49, 20);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "$0.00";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRegistrarVenta
            // 
            btnRegistrarVenta.Dock = DockStyle.Bottom;
            btnRegistrarVenta.Location = new Point(0, 598);
            btnRegistrarVenta.Name = "btnRegistrarVenta";
            btnRegistrarVenta.Size = new Size(331, 60);
            btnRegistrarVenta.TabIndex = 0;
            btnRegistrarVenta.Text = "Registrar Venta";
            btnRegistrarVenta.UseVisualStyleBackColor = true;
            // 
            // panelMenuSeleccion
            // 
            panelMenuSeleccion.Controls.Add(flpCategorias);
            panelMenuSeleccion.Controls.Add(flpPlatillos);
            panelMenuSeleccion.Dock = DockStyle.Fill;
            panelMenuSeleccion.Location = new Point(3, 3);
            panelMenuSeleccion.Name = "panelMenuSeleccion";
            panelMenuSeleccion.Size = new Size(617, 658);
            panelMenuSeleccion.TabIndex = 1;
            // 
            // flpCategorias
            // 
            flpCategorias.AutoScroll = true;
            flpCategorias.Dock = DockStyle.Top;
            flpCategorias.Location = new Point(0, 0);
            flpCategorias.Name = "flpCategorias";
            flpCategorias.Size = new Size(617, 60);
            flpCategorias.TabIndex = 1;
            // 
            // flpPlatillos
            // 
            flpPlatillos.AutoScroll = true;
            flpPlatillos.Location = new Point(0, 61);
            flpPlatillos.Name = "flpPlatillos";
            flpPlatillos.Size = new Size(617, 598);
            flpPlatillos.TabIndex = 0;
            // 
            // UC_Ventas
            // 
            Controls.Add(tlpPrincipal);
            Name = "UC_Ventas";
            Size = new Size(960, 664);
            Load += UC_Ventas_Load;
            tlpPrincipal.ResumeLayout(false);
            panelComanda.ResumeLayout(false);
            panelComanda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvComanda).EndInit();
            flpGestionTicket.ResumeLayout(false);
            panelMenuSeleccion.ResumeLayout(false);
            ResumeLayout(false);

        }
        private TableLayoutPanel tlpPrincipal;
        private Panel panelComanda;
        private Button btnRegistrarVenta;
        private Label lblTotal;
        private FlowLayoutPanel flpGestionTicket;
        private Button btnQtyAumentar;
        private Button btnQtyDisminuir;
        private Button btnEliminarPlatillo;
        private DataGridView dgvComanda;
        private Panel panelMenuSeleccion;
        private FlowLayoutPanel flpPlatillos;
        private FlowLayoutPanel flpCategorias;
    }
}
