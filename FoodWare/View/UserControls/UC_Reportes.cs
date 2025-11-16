using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;    
using FoodWare.Controller.Logic; 
using FoodWare.Model.DataAccess; 
using FoodWare.Model.Interfaces; 
using FoodWare.View.Helpers;     

namespace FoodWare.View.UserControls
{
    public partial class UC_Reportes : UserControl
    {
        private readonly ReportesController _controller;

        public UC_Reportes()
        {
            InitializeComponent();

            // 1. Inicializar el controlador
            IReporteRepository repo = new ReporteSqlRepository();
            _controller = new ReportesController(repo);

            // 2. Aplicar estilos
            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            EstilosApp.EstiloLabelTitulo(lblTituloPlatillos);
            EstilosApp.EstiloDataGridView(dgvTopPlatillos);     

            EstilosApp.EstiloLabelTitulo(lblTituloStock);
            EstilosApp.EstiloDataGridView(dgvStockBajo);
        }

        // 3. Añadir el evento Load
        private async void UC_Reportes_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // Usamos Task.WhenAll para cargar ambos reportes en paralelo
            this.Cursor = Cursors.WaitCursor;
            try
            {
                await Task.WhenAll(
                    CargarGridTopPlatillosAsync(),
                    CargarGridStockBajoAsync()
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar reportes: {ex.Message}");
                MessageBox.Show("Error al cargar reportes. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // 4. Añadir métodos de carga para cada grid
        private async Task CargarGridTopPlatillosAsync()
        {
            var data = await _controller.CargarTopPlatillosVendidosAsync();
            dgvTopPlatillos.DataSource = data;

            // Configurar columnas
            if (dgvTopPlatillos.Columns["Nombre"] is DataGridViewColumn colNombre)
            {
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvTopPlatillos.Columns["TotalVendido"] is DataGridViewColumn colTotal)
            {
                colTotal.HeaderText = "Unidades Vendidas";
                colTotal.Width = 150;
            }
        }

        private async Task CargarGridStockBajoAsync()
        {
            var data = await _controller.CargarReporteStockBajoAsync();
            dgvStockBajo.DataSource = data;

            // Configurar columnas
            if (dgvStockBajo.Columns["Nombre"] is DataGridViewColumn colNombre)
            {
                colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvStockBajo.Columns["StockActual"] is DataGridViewColumn colActual)
            {
                colActual.HeaderText = "Stock Actual";
                colActual.Width = 120;
            }
            if (dgvStockBajo.Columns["StockMinimo"] is DataGridViewColumn colMin)
            {
                colMin.HeaderText = "Stock Mínimo";
                colMin.Width = 120;
            }
            if (dgvStockBajo.Columns["CantidadAReordenar"] is DataGridViewColumn colReorden)
            {
                colReorden.HeaderText = "Faltante";
                colReorden.Width = 100;
            }
            if (dgvStockBajo.Columns["Prioridad"] is DataGridViewColumn colPrio)
            {
                colPrio.HeaderText = "Prioridad";
                colPrio.Width = 80;
            }
        }
    }
}