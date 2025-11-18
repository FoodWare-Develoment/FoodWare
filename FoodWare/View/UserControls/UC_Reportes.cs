using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Model.Interfaces;
using FoodWare.View.Helpers;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace FoodWare.View.UserControls
{
    public partial class UC_Reportes : UserControl
    {
        private readonly ReportesController _controller;

        // --- Constantes (S1192) ---
        private const string RPT_VENTAS_DIARIAS = "Reporte de Ventas Diarias";
        private const string RPT_TOP_PLATILLOS = "Top Platillos Vendidos";
        private const string RPT_STOCK_BAJO = "Reporte de Stock Bajo";
        private const string RPT_MERMAS = "Reporte de Mermas (Pérdidas)";

        private const string TituloError = "Error";
        private const string TituloInfo = "Información";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado al generar el reporte.";

        private const string VISTA_GRAFICO = "Grafico";
        private const string VISTA_TABLA = "Tabla";

        // --- Variable de estado ---
        private string _vistaActual = VISTA_GRAFICO;

        public UC_Reportes()
        {
            InitializeComponent();
            IReporteRepository repo = new ReporteSqlRepository();
            _controller = new ReportesController(repo);
            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelFiltros, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelModulo(lblTipoReporte);
            EstilosApp.EstiloLabelModulo(lblDesde);
            EstilosApp.EstiloLabelModulo(lblHasta);
            EstilosApp.EstiloComboBoxModulo(cmbTipoReporte);
            EstilosApp.EstiloBotonAccionPrincipal(btnGenerarReporte);
            EstilosApp.EstiloPanel(panelContenido, EstilosApp.ColorFondo);
            EstilosApp.EstiloDataGridView(dgvReporte);
            chartReporte.BackColor = EstilosApp.ColorFondo;
            chartReporte.ChartAreas[0].BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloPanel(panelVistaToggle, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelModulo(lblTipoVista);
            ActualizarEstilosToggle();
        }

        private void UC_Reportes_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            cmbTipoReporte.Items.Add(RPT_VENTAS_DIARIAS);
            cmbTipoReporte.Items.Add(RPT_TOP_PLATILLOS);
            cmbTipoReporte.Items.Add(RPT_STOCK_BAJO);
            cmbTipoReporte.Items.Add(RPT_MERMAS);
            cmbTipoReporte.SelectedIndex = 0;

            // Usamos DateTime.Today que ya tiene Kind = Local
            // y calculamos el primer día del mes.
            var hoy = DateTime.Today;
            dtpFechaInicio.Value = hoy.AddDays(1 - hoy.Day);
            dtpFechaFin.Value = hoy; // Hoy al final del día se calcula en el click

            CmbTipoReporte_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void CmbTipoReporte_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string reporteSeleccionado = cmbTipoReporte.SelectedItem?.ToString() ?? string.Empty;

            bool habilitarFechas = reporteSeleccionado != RPT_STOCK_BAJO;
            dtpFechaInicio.Enabled = habilitarFechas;
            dtpFechaFin.Enabled = habilitarFechas;
            lblDesde.Enabled = habilitarFechas;
            lblHasta.Enabled = habilitarFechas;

            bool tieneGrafico = reporteSeleccionado == RPT_VENTAS_DIARIAS || reporteSeleccionado == RPT_TOP_PLATILLOS;
            panelVistaToggle.Visible = tieneGrafico;
        }

        private async void BtnGenerarReporte_Click(object sender, EventArgs e)
        {
            string reporteSeleccionado = cmbTipoReporte.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(reporteSeleccionado))
            {
                MessageBox.Show("Por favor, seleccione un tipo de reporte.", TituloInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvReporte.DataSource = null;
            chartReporte.Series.Clear();

            this.Cursor = Cursors.WaitCursor;
            try
            {
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddTicks(-1);

                switch (reporteSeleccionado)
                {
                    case RPT_VENTAS_DIARIAS:
                        var ventas = await _controller.CargarReporteVentasAsync(fechaInicio, fechaFin);
                        ConfigurarGridVentas(ventas);
                        ConfigurarGraficoVentas(ventas);
                        break;

                    case RPT_TOP_PLATILLOS:
                        var platillos = await _controller.CargarTopPlatillosVendidosAsync(fechaInicio, fechaFin);
                        ConfigurarGridTopPlatillos(platillos);
                        ConfigurarGraficoTopPlatillos(platillos);
                        break;

                    case RPT_STOCK_BAJO:
                        var stock = await _controller.CargarReporteStockBajoAsync();
                        ConfigurarGridStockBajo(stock);
                        break;

                    case RPT_MERMAS:
                        var mermas = await _controller.CargarReporteMermasAsync(fechaInicio, fechaFin);
                        ConfigurarGridMermas(mermas);
                        break;
                }

                ActualizarVisibilidadContenido();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al generar reporte: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ActualizarVisibilidadContenido()
        {
            string reporteSeleccionado = cmbTipoReporte.SelectedItem?.ToString() ?? string.Empty;
            bool mostrarGrafico = false;

            if (reporteSeleccionado == RPT_VENTAS_DIARIAS || reporteSeleccionado == RPT_TOP_PLATILLOS)
            {
                mostrarGrafico = (_vistaActual == VISTA_GRAFICO);
            }

            chartReporte.Visible = mostrarGrafico;
            dgvReporte.Visible = !mostrarGrafico;

            ActualizarEstilosToggle();
        }

        private void ActualizarEstilosToggle()
        {
            if (_vistaActual == VISTA_GRAFICO)
            {
                EstilosApp.EstiloBotonModulo(btnVerGrafico);
                EstilosApp.EstiloBotonModuloSecundario(btnVerTabla);
            }
            else
            {
                EstilosApp.EstiloBotonModuloSecundario(btnVerGrafico);
                EstilosApp.EstiloBotonModulo(btnVerTabla);
            }
        }

        private void BtnVerGrafico_Click(object? sender, EventArgs e)
        {
            _vistaActual = VISTA_GRAFICO; 
            ActualizarVisibilidadContenido();
        }

        private void BtnVerTabla_Click(object? sender, EventArgs e)
        {
            _vistaActual = VISTA_TABLA; 
            ActualizarVisibilidadContenido();
        }

        private void ConfigurarGraficoVentas(List<ReporteVentasDto> datos)
        {
            chartReporte.Series.Clear();
            if (datos.Count == 0) return;

            var series = chartReporte.Series.Add("Ventas Diarias");
            series.ChartType = SeriesChartType.Column;

            datos = [.. datos.OrderBy(d => d.Dia)];

            foreach (var dato in datos)
            {
                series.Points.AddXY(dato.Dia.ToString("dd/MMM"), dato.TotalDiario);
            }

            series.IsValueShownAsLabel = true;
            series.LabelFormat = "C2";
            series.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            series.Color = EstilosApp.ColorActivo;

            chartReporte.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartReporte.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartReporte.Legends[0].Enabled = false;
        }

        private void ConfigurarGraficoTopPlatillos(List<PlatilloVendidoDto> datos)
        {
            chartReporte.Series.Clear();
            if (datos.Count == 0) return;

            var series = chartReporte.Series.Add("Top Platillos");
            series.ChartType = SeriesChartType.Pie;

            var topDatos = datos.Take(5).ToList();

            foreach (var dato in topDatos)
            {
                DataPoint dp = new(0, dato.TotalVendido)
                {
                    LegendText = $"{dato.Nombre} ({dato.TotalVendido})",
                    Label = $"{dato.TotalVendido}"
                };
                series.Points.Add(dp);
            }

            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            series.LabelForeColor = Color.White;

            chartReporte.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartReporte.Legends[0].Enabled = true;
            chartReporte.Legends[0].Docking = Docking.Right;
        }

        private void ConfigurarGridVentas(List<ReporteVentasDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Dia"] is DataGridViewColumn colDia) colDia.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["TotalDiario"] is DataGridViewColumn colTotal)
            {
                colTotal.HeaderText = "Total Vendido";
                colTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colTotal.DefaultCellStyle.Format = "C2";
            }
        }

        private void ConfigurarGridTopPlatillos(List<PlatilloVendidoDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Nombre"] is DataGridViewColumn colNombre) colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["TotalVendido"] is DataGridViewColumn colTotal)
            {
                colTotal.HeaderText = "Unidades Vendidas";
                colTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ConfigurarGridStockBajo(List<ProductoBajoStockDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Nombre"] is DataGridViewColumn colNombre) colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["StockActual"] is DataGridViewColumn colActual) colActual.HeaderText = "Stock Actual";
            if (dgvReporte.Columns["StockMinimo"] is DataGridViewColumn colMin) colMin.HeaderText = "Stock Mínimo";
            if (dgvReporte.Columns["CantidadAReordenar"] is DataGridViewColumn colReorden) colReorden.HeaderText = "Faltante";
            if (dgvReporte.Columns["Prioridad"] is DataGridViewColumn colPrio) colPrio.HeaderText = "Prioridad";
        }

        private void ConfigurarGridMermas(List<ReporteMermasDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Producto"] is DataGridViewColumn colProd) colProd.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["Motivo"] is DataGridViewColumn colMot) colMot.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["CantidadPerdida"] is DataGridViewColumn colCant) colCant.HeaderText = "Cantidad Perdida";
            if (dgvReporte.Columns["CostoPerdida"] is DataGridViewColumn colCosto)
            {
                colCosto.HeaderText = "Costo Perdido";
                colCosto.DefaultCellStyle.Format = "C2";
            }
        }
    }
}