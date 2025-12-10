using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Shared.Interfaces;
using FoodWare.View.Helpers;
using System.Collections.Generic;
using FoodWare.Shared.Entities;
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
        private const string RPT_RENTABILIDAD = "Análisis de Rentabilidad";
        private const string RPT_VENTAS_HORA = "Reporte de Ventas por Hora";

        private const string TituloError = "Error";
        private const string TituloInfo = "Información";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado al generar el reporte.";

        private const string VISTA_GRAFICO = "Grafico";
        private const string VISTA_TABLA = "Tabla";

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

            cmbTipoReporte.Items.Add(RPT_VENTAS_DIARIAS); // Gráfico + Tabla
            cmbTipoReporte.Items.Add(RPT_TOP_PLATILLOS); // Gráfico + Tabla
            cmbTipoReporte.Items.Add(RPT_VENTAS_HORA);   // Gráfico + Tabla 
            cmbTipoReporte.Items.Add(RPT_RENTABILIDAD);  // Gráfico + Tabla 
            cmbTipoReporte.Items.Add(RPT_MERMAS);       // Solo tabla
            cmbTipoReporte.Items.Add(RPT_STOCK_BAJO);   // Solo tabla
            cmbTipoReporte.SelectedIndex = 0;

            var hoy = DateTime.Today;
            dtpFechaInicio.Value = hoy.AddDays(1 - hoy.Day);
            dtpFechaFin.Value = hoy;

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

            // Mostrar conmutador si el reporte tiene gráfico
            bool tieneGrafico = reporteSeleccionado == RPT_VENTAS_DIARIAS ||
                                reporteSeleccionado == RPT_TOP_PLATILLOS ||
                                reporteSeleccionado == RPT_VENTAS_HORA ||
                                reporteSeleccionado == RPT_RENTABILIDAD;

            panelVistaToggle.Visible = tieneGrafico;

            // Si no tiene gráfico, forzamos la vista de tabla
            if (!tieneGrafico)
            {
                _vistaActual = VISTA_TABLA;
            }
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

                    case RPT_RENTABILIDAD:
                        var rentabilidad = await _controller.CargarReporteRentabilidadAsync(fechaInicio, fechaFin);
                        ConfigurarGridRentabilidad(rentabilidad);
                        ConfigurarGraficoRentabilidad(rentabilidad);
                        break;

                    case RPT_VENTAS_HORA:
                        var ventasHora = await _controller.CargarReporteVentasPorHoraAsync(fechaInicio, fechaFin);
                        ConfigurarGridVentasHora(ventasHora);
                        ConfigurarGraficoVentasHora(ventasHora);
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

            // Verificamos si el reporte seleccionado soporta gráficos
            if (reporteSeleccionado == RPT_VENTAS_DIARIAS ||
                reporteSeleccionado == RPT_TOP_PLATILLOS ||
                reporteSeleccionado == RPT_VENTAS_HORA ||
                reporteSeleccionado == RPT_RENTABILIDAD)
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

        // --- CONFIGURACIÓN DE GRÁFICOS ---

        private void ConfigurarGraficoVentas(List<ReporteVentasDto> datos)
        {
            chartReporte.Series.Clear();
            if (datos.Count == 0) return;

            var series = chartReporte.Series.Add("Ventas Diarias");
            series.ChartType = SeriesChartType.Column;

            // Estilo visual
            series.Color = EstilosApp.ColorActivo;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "C2";
            series.Font = new Font("Segoe UI", 8, FontStyle.Bold);

            foreach (var dato in datos.OrderBy(d => d.Dia))
            {
                series.Points.AddXY(dato.Dia.ToString("dd/MMM"), dato.TotalDiario);
            }

            chartReporte.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartReporte.ChartAreas[0].AxisY.LabelStyle.Format = "C0"; // Eje Y en moneda
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
                    LegendText = $"{dato.Nombre}",
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

        // --- VENTAS POR HORA ---
        private void ConfigurarGraficoVentasHora(List<ReporteVentasHoraDto> datos)
        {
            chartReporte.Series.Clear();
            if (datos.Count == 0) return;

            var series = chartReporte.Series.Add("Flujo de Ventas");
            series.ChartType = SeriesChartType.Spline; // Línea curva suave
            series.BorderWidth = 3;
            series.Color = EstilosApp.ColorAccion;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 8;

            // Aseguramos que cubra las horas aunque no haya ventas (rellenar huecos)
            for (int i = 0; i <= 23; i++)
            {
                var datoHora = datos.FirstOrDefault(d => d.Hora == i);
                decimal valor = datoHora?.TotalVendido ?? 0;
                series.Points.AddXY($"{i:00}:00", valor);
            }

            chartReporte.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartReporte.ChartAreas[0].AxisX.Interval = 2; // Mostrar cada 2 horas
            chartReporte.ChartAreas[0].AxisY.LabelStyle.Format = "C0";
            chartReporte.Legends[0].Enabled = false;
        }

        // --- RENTABILIDAD ---
        private void ConfigurarGraficoRentabilidad(List<ReporteRentabilidadDto> datos)
        {
            chartReporte.Series.Clear();
            // Filtramos primero: Si no hay datos con ganancia > 0, no mostramos nada
            if (datos == null || !datos.Any(d => d.GananciaTotal > 0)) return;

            var series = chartReporte.Series.Add("Ganancia Total");
            series.ChartType = SeriesChartType.Bar; // Barras horizontales
            series.Color = Color.Teal;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "C2"; // Muestra decimales

            var topRentables = datos
                .Where(d => d.GananciaTotal > 0)
                .OrderByDescending(d => d.GananciaTotal)
                .Take(10)
                .Reverse()
                .ToList();

            foreach (var dato in topRentables)
            {
                series.Points.AddXY(dato.Platillo, dato.GananciaTotal);
            }

            // Mejoras visuales en los ejes
            chartReporte.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartReporte.ChartAreas[0].AxisX.Interval = 1;
            chartReporte.Legends[0].Enabled = false;
        }

        // --- CONFIGURACIÓN DE GRIDS ---

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

        private void ConfigurarGridRentabilidad(List<ReporteRentabilidadDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Platillo"] is DataGridViewColumn colP) colP.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvReporte.Columns["PrecioVenta"] is DataGridViewColumn colPV) { colPV.HeaderText = "Precio Venta"; colPV.DefaultCellStyle.Format = "C2"; }
            if (dgvReporte.Columns["CostoReceta"] is DataGridViewColumn colCR) { colCR.HeaderText = "Costo Receta"; colCR.DefaultCellStyle.Format = "C2"; }
            if (dgvReporte.Columns["GananciaBruta"] is DataGridViewColumn colGB) { colGB.HeaderText = "Ganancia Bruta"; colGB.DefaultCellStyle.Format = "C2"; }
            if (dgvReporte.Columns["UnidadesVendidas"] is DataGridViewColumn colUV) colUV.HeaderText = "Unidades";
            if (dgvReporte.Columns["GananciaTotal"] is DataGridViewColumn colGT) { colGT.HeaderText = "Ganancia Total"; colGT.DefaultCellStyle.Format = "C2"; }
        }

        private void ConfigurarGridVentasHora(List<ReporteVentasHoraDto> datos)
        {
            dgvReporte.DataSource = datos;
            if (dgvReporte.Columns["Hora"] is DataGridViewColumn colH) colH.HeaderText = "Hora del Día";
            if (dgvReporte.Columns["TotalVendido"] is DataGridViewColumn colTV) { colTV.HeaderText = "Total Vendido"; colTV.DefaultCellStyle.Format = "C2"; }
            if (dgvReporte.Columns["NumeroVentas"] is DataGridViewColumn colNV) colNV.HeaderText = "N° de Ventas";

            dgvReporte.CellFormatting += (sender, e) => {
                if (dgvReporte.Columns[e.ColumnIndex].Name == "Hora" && e.Value is int hora)
                {
                    e.Value = $"{hora:00}:00";
                    e.FormattingApplied = true;
                }
            };
        }
    }
}