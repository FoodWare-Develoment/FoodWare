using System.Windows.Forms.DataVisualization.Charting;

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
        // contrasseña: 123
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            panelFiltros = new Panel();
            panelVistaToggle = new Panel();
            lblTipoVista = new Label();
            btnVerTabla = new Button();
            btnVerGrafico = new Button();
            btnGenerarReporte = new Button();
            dtpFechaFin = new DateTimePicker();
            lblHasta = new Label();
            dtpFechaInicio = new DateTimePicker();
            lblDesde = new Label();
            cmbTipoReporte = new ComboBox();
            lblTipoReporte = new Label();
            panelContenido = new Panel();
            dgvReporte = new DataGridView();
            chartReporte = new Chart();
            panelFiltros.SuspendLayout();
            panelVistaToggle.SuspendLayout();
            panelContenido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartReporte).BeginInit();
            SuspendLayout();
            // 
            // panelFiltros
            // 
            panelFiltros.Controls.Add(panelVistaToggle);
            panelFiltros.Controls.Add(btnGenerarReporte);
            panelFiltros.Controls.Add(dtpFechaFin);
            panelFiltros.Controls.Add(lblHasta);
            panelFiltros.Controls.Add(dtpFechaInicio);
            panelFiltros.Controls.Add(lblDesde);
            panelFiltros.Controls.Add(cmbTipoReporte);
            panelFiltros.Controls.Add(lblTipoReporte);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Location = new Point(0, 0);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Size = new Size(960, 100);
            panelFiltros.TabIndex = 0;
            // 
            // panelVistaToggle
            // 
            panelVistaToggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelVistaToggle.Controls.Add(lblTipoVista);
            panelVistaToggle.Controls.Add(btnVerTabla);
            panelVistaToggle.Controls.Add(btnVerGrafico);
            panelVistaToggle.Location = new Point(603, 3);
            panelVistaToggle.Name = "panelVistaToggle";
            panelVistaToggle.Size = new Size(201, 94);
            panelVistaToggle.TabIndex = 7;
            panelVistaToggle.Visible = false;
            // 
            // lblTipoVista
            // 
            lblTipoVista.AutoSize = true;
            lblTipoVista.Location = new Point(14, 9);
            lblTipoVista.Name = "lblTipoVista";
            lblTipoVista.Size = new Size(101, 20);
            lblTipoVista.TabIndex = 2;
            lblTipoVista.Text = "Tipo de Vista:";
            // 
            // btnVerTabla
            // 
            btnVerTabla.Location = new Point(104, 39);
            btnVerTabla.Name = "btnVerTabla";
            btnVerTabla.Size = new Size(85, 35);
            btnVerTabla.TabIndex = 1;
            btnVerTabla.Text = "Tabla";
            btnVerTabla.UseVisualStyleBackColor = true;
            btnVerTabla.Click += BtnVerTabla_Click;
            // 
            // btnVerGrafico
            // 
            btnVerGrafico.Location = new Point(14, 39);
            btnVerGrafico.Name = "btnVerGrafico";
            btnVerGrafico.Size = new Size(85, 35);
            btnVerGrafico.TabIndex = 0;
            btnVerGrafico.Text = "Gráfico";
            btnVerGrafico.UseVisualStyleBackColor = true;
            btnVerGrafico.Click += BtnVerGrafico_Click;
            // 
            // btnGenerarReporte
            // 
            btnGenerarReporte.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerarReporte.Location = new Point(810, 28);
            btnGenerarReporte.Name = "btnGenerarReporte";
            btnGenerarReporte.Size = new Size(138, 45);
            btnGenerarReporte.TabIndex = 6;
            btnGenerarReporte.Text = "Generar";
            btnGenerarReporte.UseVisualStyleBackColor = true;
            btnGenerarReporte.Click += BtnGenerarReporte_Click;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(370, 57);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(160, 27);
            dtpFechaFin.TabIndex = 5;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(313, 60);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 4;
            lblHasta.Text = "Hasta:";
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(370, 15);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(160, 27);
            dtpFechaInicio.TabIndex = 3;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(313, 18);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(51, 20);
            lblDesde.TabIndex = 2;
            lblDesde.Text = "Desde:";
            // 
            // cmbTipoReporte
            // 
            cmbTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoReporte.FormattingEnabled = true;
            cmbTipoReporte.Location = new Point(24, 57);
            cmbTipoReporte.Name = "cmbTipoReporte";
            cmbTipoReporte.Size = new Size(272, 28);
            cmbTipoReporte.TabIndex = 1;
            cmbTipoReporte.SelectedIndexChanged += CmbTipoReporte_SelectedIndexChanged;
            // 
            // lblTipoReporte
            // 
            lblTipoReporte.AutoSize = true;
            lblTipoReporte.Location = new Point(24, 28);
            lblTipoReporte.Name = "lblTipoReporte";
            lblTipoReporte.Size = new Size(127, 20);
            lblTipoReporte.TabIndex = 0;
            lblTipoReporte.Text = "Seleccionar Reporte";
            // 
            // panelContenido
            // 
            panelContenido.Controls.Add(chartReporte);
            panelContenido.Controls.Add(dgvReporte);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(0, 100);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new Padding(10);
            panelContenido.Size = new Size(960, 564);
            panelContenido.TabIndex = 1;
            // 
            // dgvReporte
            // 
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Dock = DockStyle.Fill;
            dgvReporte.Location = new Point(10, 10);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.RowHeadersWidth = 51;
            dgvReporte.Size = new Size(940, 544);
            dgvReporte.TabIndex = 0;
            // 
            // chartReporte
            // 
            chartArea1.Name = "ChartArea1";
            chartReporte.ChartAreas.Add(chartArea1);
            chartReporte.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chartReporte.Legends.Add(legend1);
            chartReporte.Location = new Point(10, 10);
            chartReporte.Name = "chartReporte";
            chartReporte.Size = new Size(940, 544);
            chartReporte.TabIndex = 1;
            chartReporte.Text = "chart1";
            chartReporte.Visible = false;
            // 
            // UC_Reportes
            // 
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelContenido);
            Controls.Add(panelFiltros);
            Name = "UC_Reportes";
            Size = new Size(960, 664);
            Load += UC_Reportes_Load;
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            panelVistaToggle.ResumeLayout(false); 
            panelVistaToggle.PerformLayout();
            panelContenido.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartReporte).EndInit();
            ResumeLayout(false);
        }

        private Panel panelFiltros;
        private Label lblTipoReporte;
        private ComboBox cmbTipoReporte;
        private Label lblDesde;
        private DateTimePicker dtpFechaInicio;
        private Label lblHasta;
        private DateTimePicker dtpFechaFin;
        private Button btnGenerarReporte;
        private Panel panelContenido;
        private DataGridView dgvReporte;
        private Chart chartReporte;
        private Panel panelVistaToggle;
        private Label lblTipoVista;
        private Button btnVerTabla;
        private Button btnVerGrafico;
    }
}