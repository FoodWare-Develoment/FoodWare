using System;
using System.Windows.Forms;

namespace FoodWare.View.Forms
{
    public partial class FormCorteCaja : Form
    {
        public decimal TotalContado { get; private set; }

        public FormCorteCaja()
        {
            InitializeComponent();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto < 0)
            {
                MessageBox.Show("Ingrese un monto válido (puede ser 0, pero no negativo).", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Confirma que contó FÍSICAMENTE un total de:\n\n{monto:C2}?\n\nEsta acción no se puede deshacer.",
                "Confirmar Corte Ciego",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                TotalContado = monto;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}