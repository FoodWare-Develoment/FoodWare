using System;
using System.Drawing;
using System.Windows.Forms;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Shared.Interfaces;
using FoodWare.View.Helpers;
using FoodWare.View.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace FoodWare.View.UserControls
{
    public partial class UC_Finanzas : UserControl
    {
        private readonly FinanzasController _controller;

        public UC_Finanzas()
        {
            InitializeComponent();

            IFinanzasRepository finanzasRepo = new FinanzasSqlRepository();

            _controller = new FinanzasController(finanzasRepo);

            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(panelHeader, EstilosApp.ColorFondo);
            EstilosApp.EstiloBotonAccionPrincipal(btnActualizar);

            EstilosApp.EstiloBotonModulo(btnRegistrarGasto);
            EstilosApp.EstiloBotonModuloAlerta(btnCorteCaja);

            chartGastos.BackColor = EstilosApp.ColorFondo;
            chartGastos.ChartAreas[0].BackColor = Color.White;
        }

        private async void UC_Finanzas_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // 1. Seguridad Adaptativa
            bool esCajero = (UserSession.NombreRol == "Cajero");

            if (esCajero)
            {
                AplicarModoCajero();
                return; // Detenemos la carga de datos sensibles
            }

            // Configuración Normal (Admin/Gerente)
            var hoy = DateTime.Today;
            dtpFechaInicio.Value = new DateTime(hoy.Year, hoy.Month, 1, 0, 0, 0, DateTimeKind.Local);
            dtpFechaFin.Value = hoy;

            await CargarDashboardAsync();
        }

        private void AplicarModoCajero()
        {
            // Ocultamos todo lo financiero
            panelKPIs.Visible = false;
            chartGastos.Visible = false;
            btnRegistrarGasto.Visible = false;

            // Ocultamos filtros
            dtpFechaInicio.Visible = false;
            dtpFechaFin.Visible = false;
            btnActualizar.Visible = false;
            lblTitulo.Text = "Cierre de Turno - Corte de Caja";

            // Destacamos el único botón permitido
            btnCorteCaja.Text = "REALIZAR CIERRE DE TURNO";
            btnCorteCaja.BackColor = Color.OrangeRed;
            btnCorteCaja.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnCorteCaja.Height = 60;
            btnCorteCaja.Width = 250;
            // Nota: Al estar en un FlowLayoutPanel, el botón se ajustará automáticamente.
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            await CargarDashboardAsync();
        }

        private async Task CargarDashboardAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DateTime inicio = dtpFechaInicio.Value.Date;
                DateTime fin = dtpFechaFin.Value.Date.AddDays(1).AddTicks(-1);

                var kpis = await _controller.CalcularDashboardAsync(inicio, fin);

                lblIngresosVal.Text = kpis.IngresosBrutos.ToString("C2");
                lblCostosVal.Text = kpis.CostoYPerdida.ToString("C2");
                lblGastosVal.Text = kpis.GastosOperativos.ToString("C2");
                lblUtilidadVal.Text = kpis.UtilidadNetaReal.ToString("C2");

                cardUtilidad.BackColor = kpis.UtilidadNetaReal >= 0 ? Color.RoyalBlue : Color.DarkRed;

                await CargarGraficoPastel(inicio, fin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar finanzas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async Task CargarGraficoPastel(DateTime inicio, DateTime fin)
        {
            var desglose = await _controller.ObtenerDatosGraficoPastelAsync(inicio, fin);

            chartGastos.Series.Clear();
            chartGastos.Titles.Clear();
            chartGastos.Titles.Add("Desglose de Gastos y Costos");

            var series = chartGastos.Series.Add("Gastos");
            series.ChartType = SeriesChartType.Doughnut;

            foreach (var item in desglose)
            {
                if (item.MontoTotal > 0)
                {
                    int p = series.Points.AddXY(item.Categoria, item.MontoTotal);
                    series.Points[p].Label = "#VALX (#PERCENT)";
                    series.Points[p].LegendText = $"{item.Categoria}: {item.MontoTotal:C0}";
                }
            }
        }

        private async void BtnRegistrarGasto_Click(object sender, EventArgs e)
        {
            using var form = new FormRegistrarGasto();
            var dialogResult = await Task.Run(() => form.ShowDialog());
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    await _controller.RegistrarGastoAsync(form.Concepto, form.Monto, form.Categoria, DateTime.Now);
                    MessageBox.Show("Gasto registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDashboardAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private async void BtnCorteCaja_Click(object sender, EventArgs e)
        {
            using var form = new FormCorteCaja();

            var dialogResult = await Task.Run(() => form.ShowDialog());
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var corte = await _controller.RealizarCorteCiegoAsync(form.TotalContado, DateTime.Today);

                string mensaje = $"Corte Finalizado.\n\n" +
                                 $"Esperado: {corte.TotalEfectivoEsperado:C2}\n" +
                                 $"Contado: {corte.TotalEfectivoContado:C2}\n" +
                                 $"Diferencia: {corte.Diferencia:C2} ({corte.SobranteFaltante})";

                MessageBoxIcon icon = corte.SobranteFaltante == "Faltante" ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                MessageBox.Show(mensaje, "Resultado de Corte", MessageBoxButtons.OK, icon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}