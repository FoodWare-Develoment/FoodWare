namespace FoodWare.View.Forms
{
    partial class FormCorteCaja
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblInstruccion = new Label();
            txtMonto = new TextBox();
            btnConfirmar = new Button();
            SuspendLayout();
            // 
            // lblInstruccion
            // 
            lblInstruccion.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblInstruccion.ForeColor = Color.White;
            lblInstruccion.Location = new Point(20, 20);
            lblInstruccion.Name = "lblInstruccion";
            lblInstruccion.Size = new Size(340, 70);
            lblInstruccion.TabIndex = 0;
            lblInstruccion.Text = "Ingrese el TOTAL de efectivo\r\ncontado en caja:";
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtMonto
            // 
            txtMonto.Font = new Font("Segoe UI", 14F);
            txtMonto.Location = new Point(100, 100);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new Size(180, 39);
            txtMonto.TabIndex = 1;
            txtMonto.TextAlign = HorizontalAlignment.Center;
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.OrangeRed;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(100, 160);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(180, 45);
            btnConfirmar.TabIndex = 2;
            btnConfirmar.Text = "FINALIZAR CORTE";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += BtnConfirmar_Click;
            // 
            // FormCorteCaja
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(382, 233);
            ControlBox = false;
            Controls.Add(btnConfirmar);
            Controls.Add(txtMonto);
            Controls.Add(lblInstruccion);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormCorteCaja";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CORTE DE CAJA CIEGO";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblInstruccion;
        private TextBox txtMonto;
        private Button btnConfirmar;
    }
}