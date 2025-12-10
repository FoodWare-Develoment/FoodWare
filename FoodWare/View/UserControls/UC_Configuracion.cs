using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Shared.Interfaces;
using FoodWare.View.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Configuracion : UserControl
    {
        private readonly ConfiguracionController _controller;

        public UC_Configuracion()
        {
            InitializeComponent();

            // Inyección manual de dependencias
            IConfiguracionRepository repo = new ConfiguracionSqlRepository();
            _controller = new ConfiguracionController(repo);

            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            panelMain.BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloLabelTitulo(lblTitulo);

            // Estilos para GroupBoxes
            gbDatosGenerales.ForeColor = EstilosApp.ColorTextoOscuro;
            gbFinanciero.ForeColor = EstilosApp.ColorTextoOscuro;

            // Labels
            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblDireccion);
            EstilosApp.EstiloLabelModulo(lblMensaje);
            EstilosApp.EstiloLabelModulo(lblImpuesto);
            EstilosApp.EstiloLabelModulo(lblMoneda);

            // TextBoxes
            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtDireccion);
            EstilosApp.EstiloTextBoxModulo(txtMensaje);
            EstilosApp.EstiloTextBoxModulo(txtImpuesto);
            EstilosApp.EstiloTextBoxModulo(txtMoneda);

            // Botón
            EstilosApp.EstiloBotonModulo(btnGuardar);
        }

        private async void UC_Configuracion_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var config = await _controller.CargarConfiguracionAsync();

                txtNombre.Text = config.NombreRestaurante;
                txtDireccion.Text = config.Direccion;
                txtMensaje.Text = config.MensajeTicket;
                txtImpuesto.Text = config.PorcentajeImpuesto.ToString("0.00");
                txtMoneda.Text = config.Moneda;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar configuración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación básica de UI para decimal
                if (!decimal.TryParse(txtImpuesto.Text, out decimal impuesto))
                {
                    MessageBox.Show("El impuesto debe ser un número válido.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                await _controller.GuardarConfiguracionAsync(
                    txtNombre.Text.Trim(),
                    txtDireccion.Text.Trim(),
                    impuesto,
                    txtMoneda.Text.Trim(),
                    txtMensaje.Text.Trim()
                );

                MessageBox.Show("Configuración actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await CargarDatosAsync();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error guardar config: {ex.Message}");
                MessageBox.Show("Ocurrió un error al guardar los cambios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}