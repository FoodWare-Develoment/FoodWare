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

            IConfiguracionRepository repo = new ConfiguracionSqlRepository();
            _controller = new ConfiguracionController(repo);

            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            panelMain.BackColor = EstilosApp.ColorFondo;

            EstilosApp.EstiloLabelTitulo(lblTitulo);

            gbDatosGenerales.ForeColor = EstilosApp.ColorTextoOscuro;
            gbFinanciero.ForeColor = EstilosApp.ColorTextoOscuro;

            EstilosApp.EstiloLabelModulo(lblNombre);
            EstilosApp.EstiloLabelModulo(lblDireccion);
            EstilosApp.EstiloLabelModulo(lblMensaje);
            EstilosApp.EstiloLabelModulo(lblImpuesto);
            EstilosApp.EstiloLabelModulo(lblMoneda);

            EstilosApp.EstiloTextBoxModulo(txtNombre);
            EstilosApp.EstiloTextBoxModulo(txtDireccion);
            EstilosApp.EstiloTextBoxModulo(txtMensaje);
            EstilosApp.EstiloTextBoxModulo(txtImpuesto);
            EstilosApp.EstiloTextBoxModulo(txtMoneda);

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

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión actual y volver al login?",
                "Cerrar Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea salir de FoodWare?",
                "Salir del Sistema",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}