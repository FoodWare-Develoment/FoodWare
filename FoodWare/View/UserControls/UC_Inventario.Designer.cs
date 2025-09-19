namespace FoodWare.View.UserControls
{
    partial class UC_Inventario
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelInputs = new Panel();
            btnGuardar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            lblStock = new Label();
            lblCategoria = new Label();
            lblNombre = new Label();
            txtStock = new TextBox();
            txtCategoria = new TextBox();
            txtNombre = new TextBox();
            dgvInventario = new DataGridView();
            panelInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            SuspendLayout();
            // 
            // panelInputs
            // 
            panelInputs.Controls.Add(btnGuardar);
            panelInputs.Controls.Add(btnEliminar);
            panelInputs.Controls.Add(btnLimpiar);
            panelInputs.Controls.Add(txtPrecio);
            panelInputs.Controls.Add(lblPrecio);
            panelInputs.Controls.Add(lblStock);
            panelInputs.Controls.Add(lblCategoria);
            panelInputs.Controls.Add(lblNombre);
            panelInputs.Controls.Add(txtStock);
            panelInputs.Controls.Add(txtCategoria);
            panelInputs.Controls.Add(txtNombre);
            panelInputs.Dock = DockStyle.Top;
            panelInputs.Location = new Point(0, 0);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(800, 179);
            panelInputs.TabIndex = 0;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(654, 14);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(104, 49);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(654, 67);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(104, 49);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(654, 119);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(104, 49);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // txtPrecio
            // 
            txtPrecio.Location = new Point(164, 137);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(428, 27);
            txtPrecio.TabIndex = 7;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(44, 137);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(50, 20);
            lblPrecio.TabIndex = 6;
            lblPrecio.Text = "Precio";
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(44, 96);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(45, 20);
            lblStock.TabIndex = 5;
            lblStock.Text = "Stock";
            lblStock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(44, 56);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(74, 20);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoria";
            lblCategoria.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.ImageAlign = ContentAlignment.BottomCenter;
            lblNombre.Location = new Point(44, 16);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(164, 96);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(428, 27);
            txtStock.TabIndex = 2;
            // 
            // txtCategoria
            // 
            txtCategoria.Location = new Point(164, 56);
            txtCategoria.Name = "txtCategoria";
            txtCategoria.Size = new Size(428, 27);
            txtCategoria.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(164, 17);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(428, 27);
            txtNombre.TabIndex = 0;
            // 
            // dgvInventario
            // 
            dgvInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventario.Dock = DockStyle.Bottom;
            dgvInventario.Location = new Point(0, 199);
            dgvInventario.Name = "dgvInventario";
            dgvInventario.RowHeadersWidth = 51;
            dgvInventario.Size = new Size(800, 251);
            dgvInventario.TabIndex = 1;
            // 
            // UC_Inventario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvInventario);
            Controls.Add(panelInputs);
            Name = "UC_Inventario";
            Size = new Size(800, 450);
            panelInputs.ResumeLayout(false);
            panelInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ResumeLayout(false);
        }
        private Panel panelInputs;
        private TextBox txtStock;
        private TextBox txtCategoria;
        private TextBox txtNombre;
        private Label lblStock;
        private Label lblCategoria;
        private Label lblNombre;
        private TextBox txtPrecio;
        private Label lblPrecio;
        private Button btnGuardar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvInventario;
    }
}
