using System.Windows.Forms.DataVisualization.Charting;

namespace FoodWare.View.UserControls
{
    partial class UC_Finanzas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();

            panelHeader = new Panel();
            lblTitulo = new Label();
            dtpFechaFin = new DateTimePicker();
            dtpFechaInicio = new DateTimePicker();
            btnActualizar = new Button();

            panelKPIs = new TableLayoutPanel();
            cardIngresos = new Panel(); lblIngresosVal = new Label(); lblIngresosTit = new Label();
            cardCostos = new Panel(); lblCostosVal = new Label(); lblCostosTit = new Label();
            cardGastos = new Panel(); lblGastosVal = new Label(); lblGastosTit = new Label();
            cardUtilidad = new Panel(); lblUtilidadVal = new Label(); lblUtilidadTit = new Label();

            chartGastos = new Chart();

            panelAcciones = new FlowLayoutPanel();
            btnRegistrarGasto = new Button();
            btnCorteCaja = new Button();

            panelHeader.SuspendLayout();
            panelKPIs.SuspendLayout();
            cardIngresos.SuspendLayout(); cardCostos.SuspendLayout(); cardGastos.SuspendLayout(); cardUtilidad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartGastos).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();

            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 60;
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(btnActualizar);
            panelHeader.Controls.Add(dtpFechaFin);
            panelHeader.Controls.Add(dtpFechaInicio);

            lblTitulo.Text = "Dashboard Financiero";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(10, 15);
            lblTitulo.AutoSize = true;

            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(300, 18);
            dtpFechaInicio.Width = 120;

            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(430, 18);
            dtpFechaFin.Width = 120;

            btnActualizar.Text = "Filtrar";
            btnActualizar.Location = new Point(560, 15);
            btnActualizar.Click += BtnActualizar_Click;

            panelKPIs.Dock = DockStyle.Top;
            panelKPIs.Height = 120;
            panelKPIs.ColumnCount = 4;
            panelKPIs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelKPIs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelKPIs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelKPIs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            ConfigurarTarjeta(cardIngresos, lblIngresosTit, lblIngresosVal, "Ingresos Brutos", Color.SeaGreen);
            ConfigurarTarjeta(cardCostos, lblCostosTit, lblCostosVal, "Costos + Mermas", Color.DarkOrange);
            ConfigurarTarjeta(cardGastos, lblGastosTit, lblGastosVal, "Gastos Operativos", Color.Firebrick);
            ConfigurarTarjeta(cardUtilidad, lblUtilidadTit, lblUtilidadVal, "Utilidad Neta", Color.RoyalBlue);

            panelKPIs.Controls.Add(cardIngresos, 0, 0);
            panelKPIs.Controls.Add(cardCostos, 1, 0);
            panelKPIs.Controls.Add(cardGastos, 2, 0);
            panelKPIs.Controls.Add(cardUtilidad, 3, 0);

            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Height = 70;
            panelAcciones.Padding = new Padding(10);
            panelAcciones.Controls.Add(btnRegistrarGasto);
            panelAcciones.Controls.Add(btnCorteCaja);

            btnRegistrarGasto.Text = "Registrar Gasto";
            btnRegistrarGasto.Size = new Size(150, 50);
            btnRegistrarGasto.Click += BtnRegistrarGasto_Click;

            btnCorteCaja.Text = "REALIZAR CORTE";
            btnCorteCaja.Size = new Size(150, 50);
            btnCorteCaja.Click += BtnCorteCaja_Click;

            chartGastos.Dock = DockStyle.Fill;
            chartArea1.Name = "ChartArea1";
            chartGastos.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chartGastos.Legends.Add(legend1);

            Controls.Add(chartGastos);
            Controls.Add(panelAcciones);
            Controls.Add(panelKPIs);
            Controls.Add(panelHeader);
            Name = "UC_Finanzas";
            Size = new Size(960, 664);
            Load += UC_Finanzas_Load;

            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelKPIs.ResumeLayout(false);
            cardIngresos.ResumeLayout(false); cardCostos.ResumeLayout(false);
            cardGastos.ResumeLayout(false); cardUtilidad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartGastos).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigurarTarjeta(Panel p, Label lblTit, Label lblVal, string titulo, Color color)
        {
            p.Dock = DockStyle.Fill;
            p.BackColor = color;
            p.Padding = new Padding(5);
            p.Margin = new Padding(5);

            lblTit.Text = titulo;
            lblTit.Dock = DockStyle.Top;
            lblTit.ForeColor = Color.White;
            lblTit.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTit.TextAlign = ContentAlignment.TopLeft;

            lblVal.Text = "$0.00";
            lblVal.Dock = DockStyle.Fill;
            lblVal.ForeColor = Color.White;
            lblVal.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblVal.TextAlign = ContentAlignment.MiddleCenter;

            p.Controls.Add(lblVal);
            p.Controls.Add(lblTit);
        }

        private Panel panelHeader;
        private Label lblTitulo;
        private DateTimePicker dtpFechaInicio;
        private DateTimePicker dtpFechaFin;
        private Button btnActualizar;
        private TableLayoutPanel panelKPIs;
        private Panel cardIngresos, cardCostos, cardGastos, cardUtilidad;
        private Label lblIngresosVal, lblIngresosTit, lblCostosVal, lblCostosTit, lblGastosVal, lblGastosTit, lblUtilidadVal, lblUtilidadTit;
        private Chart chartGastos;
        private FlowLayoutPanel panelAcciones;
        private Button btnRegistrarGasto;
        private Button btnCorteCaja;
    }
}