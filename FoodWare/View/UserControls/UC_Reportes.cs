using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Model.Interfaces;
using FoodWare.View.Helpers;
using System.Collections.Generic; // Para List<>
using FoodWare.Model.Entities;    // Para DTOs

namespace FoodWare.View.UserControls
{
    public partial class UC_Reportes : UserControl
    {
        private readonly ReportesController _controller;

        // Constantes para los tipos de reporte (S1192)
        private const string RPT_VENTAS_DIARIAS = "Reporte de Ventas Diarias";
        private const string RPT_TOP_PLATILLOS = "Top Platillos Vendidos";
        private const string RPT_STOCK_BAJO = "Reporte de Stock Bajo";
        private const string RPT_MERMAS = "Reporte de Mermas (Pérdidas)";

        // Constantes para mensajes
        private const string TituloError = "Error";
        private const string TituloInfo = "Información";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado al generar el reporte.";

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
        }

        private void UC_Reportes_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // Llenar el ComboBox con los reportes disponibles
            cmbTipoReporte.Items.Add(RPT_VENTAS_DIARIAS);
            cmbTipoReporte.Items.Add(RPT_TOP_PLATILLOS);
            cmbTipoReporte.Items.Add(RPT_STOCK_BAJO);
            cmbTipoReporte.Items.Add(RPT_MERMAS);
            cmbTipoReporte.SelectedIndex = 0;

            // Configurar fechas por defecto (ej. el mes actual)
            dtpFechaInicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.Value = DateTime.Now;
        }

        private void CmbTipoReporte_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Habilitar/deshabilitar filtros de fecha
            bool habilitarFechas = cmbTipoReporte.SelectedItem?.ToString() != RPT_STOCK_BAJO;
            dtpFechaInicio.Enabled = habilitarFechas;
            dtpFechaFin.Enabled = habilitarFechas;
            lblDesde.Enabled = habilitarFechas;
            lblHasta.Enabled = habilitarFechas;
        }

        private async void BtnGenerarReporte_Click(object sender, EventArgs e)
        {
            string reporteSeleccionado = cmbTipoReporte.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(reporteSeleccionado))
            {
                MessageBox.Show("Por favor, seleccione un tipo de reporte.", TituloInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ocultar todo por defecto
            dgvReporte.Visible = false;
            chartReporte.Visible = false;
            dgvReporte.DataSource = null;
            chartReporte.Series.Clear();

            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Obtener fechas (asegurando que Fin sea al final del día)
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddTicks(-1);

                // Ejecutar el reporte seleccionado
                switch (reporteSeleccionado)
                {
                    case RPT_VENTAS_DIARIAS:
                        var ventas = await _controller.CargarReporteVentasAsync(fechaInicio, fechaFin);
                        ConfigurarGridVentas(ventas);
                        break;

                    case RPT_TOP_PLATILLOS:
                        var platillos = await _controller.CargarTopPlatillosVendidosAsync(fechaInicio, fechaFin);
                        ConfigurarGridTopPlatillos(platillos);
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

        // --- MÉTODOS HELPER PARA CONFIGURAR CADA GRID ---

        private void ConfigurarGridVentas(List<ReporteVentasDto> datos)
        {
            dgvReporte.DataSource = datos;
            dgvReporte.Visible = true;

            if (dgvReporte.Columns["Dia"] is DataGridViewColumn colDia)
            {
                colDia.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["TotalDiario"] is DataGridViewColumn colTotal)
            {
                colTotal.HeaderText = "Total Vendido";
                colTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colTotal.DefaultCellStyle.Format = "C2"; // Formato de Moneda
            }
        }

        private void ConfigurarGridTopPlatillos(List<PlatilloVendidoDto> datos)
        {
            dgvReporte.DataSource = datos;
            dgvReporte.Visible = true;

            if (dgvReporte.Columns["Nombre"] is DataGridViewColumn colNombre)
            {
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["TotalVendido"] is DataGridViewColumn colTotal)
            {
                colTotal.HeaderText = "Unidades Vendidas";
                colTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ConfigurarGridStockBajo(List<ProductoBajoStockDto> datos)
        {
            dgvReporte.DataSource = datos;
            dgvReporte.Visible = true;

            if (dgvReporte.Columns["Nombre"] is DataGridViewColumn colNombre)
            {
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["StockActual"] is DataGridViewColumn colActual) colActual.HeaderText = "Stock Actual";
            if (dgvReporte.Columns["StockMinimo"] is DataGridViewColumn colMin) colMin.HeaderText = "Stock Mínimo";
            if (dgvReporte.Columns["CantidadAReordenar"] is DataGridViewColumn colReorden) colReorden.HeaderText = "Faltante";
            if (dgvReporte.Columns["Prioridad"] is DataGridViewColumn colPrio) colPrio.HeaderText = "Prioridad";
        }

        private void ConfigurarGridMermas(List<ReporteMermasDto> datos)
        {
            dgvReporte.DataSource = datos;
            dgvReporte.Visible = true;

            if (dgvReporte.Columns["Producto"] is DataGridViewColumn colProd)
            {
                colProd.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["Motivo"] is DataGridViewColumn colMot)
            {
                colMot.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["CantidadPerdida"] is DataGridViewColumn colCant)
            {
                colCant.HeaderText = "Cantidad Perdida";
                colCant.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvReporte.Columns["CostoPerdida"] is DataGridViewColumn colCosto)
            {
                colCosto.HeaderText = "Costo Perdido";
                colCosto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colCosto.DefaultCellStyle.Format = "C2"; // Formato de Moneda
            }
        }
    }
}