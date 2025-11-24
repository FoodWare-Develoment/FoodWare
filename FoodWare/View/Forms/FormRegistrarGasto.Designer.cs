namespace FoodWare.View.Forms
{
    partial class FormRegistrarGasto
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblConcepto = new Label();
            txtConcepto = new TextBox();
            lblMonto = new Label();
            txtMonto = new TextBox();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            btnGuardar = new Button();
            SuspendLayout();
            // 
            // lblConcepto
            // 
            lblConcepto.AutoSize = true;
            lblConcepto.Location = new Point(20, 20);
            lblConcepto.Name = "lblConcepto";
            lblConcepto.Size = new Size(137, 20);
            lblConcepto.TabIndex = 0;
            lblConcepto.Text = "Concepto (ej. Luz):";
            // 
            // txtConcepto
            // 
            txtConcepto.Location = new Point(20, 45);
            txtConcepto.Name = "txtConcepto";
            txtConcepto.Size = new Size(340, 27);
            txtConcepto.TabIndex = 1;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(20, 85);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(75, 20);
            lblMonto.TabIndex = 2;
            lblMonto.Text = "Monto ($):";
            // 
            // txtMonto
            // 
            txtMonto.Location = new Point(20, 110);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new Size(340, 27);
            txtMonto.TabIndex = 3;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(20, 150);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(77, 20);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoría:";
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(20, 175);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(340, 28);
            cmbCategoria.TabIndex = 5;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.Teal;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(100, 230);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(180, 40);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar Gasto";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // FormRegistrarGasto
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 303);
            Controls.Add(btnGuardar);
            Controls.Add(cmbCategoria);
            Controls.Add(lblCategoria);
            Controls.Add(txtMonto);
            Controls.Add(lblMonto);
            Controls.Add(txtConcepto);
            Controls.Add(lblConcepto);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRegistrarGasto";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar Gasto Operativo";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblConcepto;
        private TextBox txtConcepto;
        private Label lblMonto;
        private TextBox txtMonto;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Button btnGuardar;
    }
}