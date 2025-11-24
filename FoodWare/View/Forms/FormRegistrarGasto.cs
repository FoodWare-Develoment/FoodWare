using System;
using System.Windows.Forms;

namespace FoodWare.View.Forms
{
    public partial class FormRegistrarGasto : Form
    {
        // Propiedades públicas para recuperar los datos desde fuera
        public string Concepto { get; private set; } = string.Empty;
        public decimal Monto { get; private set; }
        public string Categoria { get; private set; } = "Servicios";

        public FormRegistrarGasto()
        {
            InitializeComponent();

            // Llenamos el combo aquí en la lógica
            cmbCategoria.Items.AddRange(["Servicios", "Nómina", "Mantenimiento", "Renta", "Insumos Extra", "Otros"]);
            cmbCategoria.SelectedIndex = 0;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones simples de UI
            if (string.IsNullOrWhiteSpace(txtConcepto.Text))
            {
                MessageBox.Show("Por favor, ingrese un concepto.", "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("El monto debe ser un número mayor a 0.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asignar valores
            Concepto = txtConcepto.Text;
            Monto = monto;
            Categoria = cmbCategoria.SelectedItem?.ToString() ?? "Otros";

            // Cerrar con éxito
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}