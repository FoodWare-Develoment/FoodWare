namespace FoodWare.View.UserControls
{
    partial class UC_Reportes
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
            this.tlpReportes = new System.Windows.Forms.TableLayoutPanel();
            this.panelPlatillos = new System.Windows.Forms.Panel();
            this.dgvTopPlatillos = new System.Windows.Forms.DataGridView();
            this.lblTituloPlatillos = new System.Windows.Forms.Label();
            this.panelStock = new System.Windows.Forms.Panel();
            this.dgvStockBajo = new System.Windows.Forms.DataGridView();
            this.lblTituloStock = new System.Windows.Forms.Label();
            this.tlpReportes.SuspendLayout();
            this.panelPlatillos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopPlatillos)).BeginInit();
            this.panelStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBajo)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpReportes
            // 
            this.tlpReportes.ColumnCount = 1;
            this.tlpReportes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpReportes.Controls.Add(this.panelPlatillos, 0, 0);
            this.tlpReportes.Controls.Add(this.panelStock, 0, 1);
            this.tlpReportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpReportes.Location = new System.Drawing.Point(0, 0);
            this.tlpReportes.Name = "tlpReportes";
            this.tlpReportes.RowCount = 2;
            this.tlpReportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpReportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpReportes.Size = new System.Drawing.Size(800, 600);
            this.tlpReportes.TabIndex = 0;
            // 
            // panelPlatillos
            // 
            // --- INICIO DE LA CORRECCIÓN ---
            // El Label (Dock=Top) se añade PRIMERO
            this.panelPlatillos.Controls.Add(this.dgvTopPlatillos);
            this.panelPlatillos.Controls.Add(this.lblTituloPlatillos);
            // El Grid (Dock=Fill) se añade SEGUNDO
            // --- FIN DE LA CORRECCIÓN ---
            this.panelPlatillos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlatillos.Location = new System.Drawing.Point(3, 3);
            this.panelPlatillos.Name = "panelPlatillos";
            this.panelPlatillos.Padding = new System.Windows.Forms.Padding(10);
            this.panelPlatillos.Size = new System.Drawing.Size(794, 294);
            this.panelPlatillos.TabIndex = 0;
            // 
            // dgvTopPlatillos
            // 
            this.dgvTopPlatillos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopPlatillos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopPlatillos.Location = new System.Drawing.Point(10, 43); // Ajustado para un label más alto
            this.dgvTopPlatillos.Name = "dgvTopPlatillos";
            this.dgvTopPlatillos.RowHeadersWidth = 51;
            this.dgvTopPlatillos.Size = new System.Drawing.Size(774, 241);
            this.dgvTopPlatillos.TabIndex = 1;
            // 
            // lblTituloPlatillos
            // 
            this.lblTituloPlatillos.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloPlatillos.Location = new System.Drawing.Point(10, 10);
            this.lblTituloPlatillos.Name = "lblTituloPlatillos";
            this.lblTituloPlatillos.Size = new System.Drawing.Size(774, 33); // Aumenté la altura
            this.lblTituloPlatillos.TabIndex = 0;
            this.lblTituloPlatillos.Text = "Ranking de Platillos Más Vendidos";
            // 
            // panelStock
            // 
            // --- INICIO DE LA CORRECCIÓN ---
            // El Label (Dock=Top) se añade PRIMERO
            this.panelStock.Controls.Add(this.dgvStockBajo);
            this.panelStock.Controls.Add(this.lblTituloStock);
            // El Grid (Dock=Fill) se añade SEGUNDO
            // --- FIN DE LA CORRECCIÓN ---
            this.panelStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStock.Location = new System.Drawing.Point(3, 303);
            this.panelStock.Name = "panelStock";
            this.panelStock.Padding = new System.Windows.Forms.Padding(10);
            this.panelStock.Size = new System.Drawing.Size(794, 294);
            this.panelStock.TabIndex = 1;
            // 
            // dgvStockBajo
            // 
            this.dgvStockBajo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockBajo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockBajo.Location = new System.Drawing.Point(10, 43); // Ajustado para un label más alto
            this.dgvStockBajo.Name = "dgvStockBajo";
            this.dgvStockBajo.RowHeadersWidth = 51;
            this.dgvStockBajo.Size = new System.Drawing.Size(774, 241);
            this.dgvStockBajo.TabIndex = 1;
            // 
            // lblTituloStock
            // 
            this.lblTituloStock.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloStock.Location = new System.Drawing.Point(10, 10);
            this.lblTituloStock.Name = "lblTituloStock";
            this.lblTituloStock.Size = new System.Drawing.Size(774, 33); // Aumenté la altura
            this.lblTituloStock.TabIndex = 0;
            this.lblTituloStock.Text = "Reporte de Prioridad de Reabastecimiento";
            // 
            // UC_Reportes
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpReportes);
            this.Name = "UC_Reportes";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.UC_Reportes_Load);
            this.tlpReportes.ResumeLayout(false);
            this.panelPlatillos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopPlatillos)).EndInit();
            this.panelStock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBajo)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpReportes;
        private System.Windows.Forms.Panel panelPlatillos;
        private System.Windows.Forms.DataGridView dgvTopPlatillos;
        private System.Windows.Forms.Label lblTituloPlatillos;
        private System.Windows.Forms.Panel panelStock;
        private System.Windows.Forms.DataGridView dgvStockBajo;
        private System.Windows.Forms.Label lblTituloStock;
    }
}