﻿namespace FoodWare.View.UserControls
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
            cmbUnidadMedida = new ComboBox();
            btnActualizar = new Button();
            lblUnidad = new Label();
            txtStockMinimo = new TextBox();
            lblStockMinimo = new Label();
            btnGuardar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            lblStock = new Label();
            lblCategoria = new Label();
            lblNombre = new Label();
            txtStock = new TextBox();
            txtNombre = new TextBox();
            dgvInventario = new DataGridView();
            cmbCategoria = new ComboBox();
            panelInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            SuspendLayout();
            // 
            // panelInputs
            // 
            panelInputs.Controls.Add(cmbCategoria);
            panelInputs.Controls.Add(cmbUnidadMedida);
            panelInputs.Controls.Add(btnActualizar);
            panelInputs.Controls.Add(lblUnidad);
            panelInputs.Controls.Add(txtStockMinimo);
            panelInputs.Controls.Add(lblStockMinimo);
            panelInputs.Controls.Add(btnGuardar);
            panelInputs.Controls.Add(btnEliminar);
            panelInputs.Controls.Add(btnLimpiar);
            panelInputs.Controls.Add(txtPrecio);
            panelInputs.Controls.Add(lblPrecio);
            panelInputs.Controls.Add(lblStock);
            panelInputs.Controls.Add(lblCategoria);
            panelInputs.Controls.Add(lblNombre);
            panelInputs.Controls.Add(txtStock);
            panelInputs.Controls.Add(txtNombre);
            panelInputs.Dock = DockStyle.Top;
            panelInputs.Location = new Point(0, 0);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(960, 269);
            panelInputs.TabIndex = 0;
            // 
            // cmbUnidadMedida
            // 
            cmbUnidadMedida.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnidadMedida.FormattingEnabled = true;
            cmbUnidadMedida.Location = new Point(164, 99);
            cmbUnidadMedida.Name = "cmbUnidadMedida";
            cmbUnidadMedida.Size = new Size(590, 28);
            cmbUnidadMedida.TabIndex = 19;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActualizar.Location = new Point(800, 140);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(104, 49);
            btnActualizar.TabIndex = 18;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // lblUnidad
            // 
            lblUnidad.AutoSize = true;
            lblUnidad.Location = new Point(44, 97);
            lblUnidad.Name = "lblUnidad";
            lblUnidad.Size = new Size(57, 20);
            lblUnidad.TabIndex = 16;
            lblUnidad.Text = "Unidad";
            lblUnidad.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStockMinimo
            // 
            txtStockMinimo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtStockMinimo.Location = new Point(164, 183);
            txtStockMinimo.Name = "txtStockMinimo";
            txtStockMinimo.Size = new Size(590, 27);
            txtStockMinimo.TabIndex = 15;
            // 
            // lblStockMinimo
            // 
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.Location = new Point(44, 183);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.Size = new Size(100, 20);
            lblStockMinimo.TabIndex = 14;
            lblStockMinimo.Text = "Stock Minimo";
            lblStockMinimo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.Location = new Point(800, 17);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(104, 49);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEliminar.Location = new Point(800, 79);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(104, 49);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += BtnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiar.Location = new Point(800, 202);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(104, 49);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += BtnLimpiar_Click;
            // 
            // txtPrecio
            // 
            txtPrecio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPrecio.Location = new Point(164, 224);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(590, 27);
            txtPrecio.TabIndex = 7;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(44, 224);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(50, 20);
            lblPrecio.TabIndex = 6;
            lblPrecio.Text = "Precio";
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(44, 140);
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
            txtStock.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtStock.Location = new Point(164, 140);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(590, 27);
            txtStock.TabIndex = 2;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.Location = new Point(164, 17);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(590, 27);
            txtNombre.TabIndex = 0;
            // 
            // dgvInventario
            // 
            dgvInventario.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventario.Location = new Point(0, 269);
            dgvInventario.Name = "dgvInventario";
            dgvInventario.RowHeadersWidth = 51;
            dgvInventario.Size = new Size(960, 399);
            dgvInventario.TabIndex = 1;
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(164, 56);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(590, 28);
            cmbCategoria.TabIndex = 20;
            // 
            // UC_Inventario
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(panelInputs);
            Controls.Add(dgvInventario);
            Name = "UC_Inventario";
            Size = new Size(960, 664);
            Load += UC_Inventario_Load;
            panelInputs.ResumeLayout(false);
            panelInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ResumeLayout(false);
        }
        private Panel panelInputs;
        private TextBox txtStock;
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
        private TextBox txtStockMinimo;
        private Label lblStockMinimo;
        private Label lblUnidad;
        private Button btnActualizar;
        private ComboBox cmbUnidadMedida;
        private ComboBox cmbCategoria;
    }
}
