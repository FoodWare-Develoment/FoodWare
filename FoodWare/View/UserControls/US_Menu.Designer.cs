namespace FoodWare.View.UserControls
{
    partial class UC_Menu
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
            lblCategoria = new Label();
            lblNombre = new Label();
            txtCategoria = new TextBox();
            txtNombre = new TextBox();
            dgvMenu = new DataGridView();
            panelInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMenu).BeginInit();
            SuspendLayout();
            // 
            // panelInputs
            // 
            panelInputs.Controls.Add(btnGuardar);
            panelInputs.Controls.Add(btnEliminar);
            panelInputs.Controls.Add(btnLimpiar);
            panelInputs.Controls.Add(txtPrecio);
            panelInputs.Controls.Add(lblPrecio);
            panelInputs.Controls.Add(lblCategoria);
            panelInputs.Controls.Add(lblNombre);
            panelInputs.Controls.Add(txtCategoria);
            panelInputs.Controls.Add(txtNombre);
            panelInputs.Dock = DockStyle.Top;
            panelInputs.Location = new Point(0, 0);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(960, 179);
            panelInputs.TabIndex = 0;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.Location = new Point(802, 16);
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
            btnEliminar.Location = new Point(802, 67);
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
            btnLimpiar.Location = new Point(802, 119);
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
            txtPrecio.Location = new Point(164, 121);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(590, 27);
            txtPrecio.TabIndex = 7;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(44, 121);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(50, 20);
            lblPrecio.TabIndex = 6;
            lblPrecio.Text = "Precio";
            lblPrecio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(44, 75);
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
            lblNombre.Location = new Point(44, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 3;
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtCategoria
            // 
            txtCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCategoria.Location = new Point(164, 75);
            txtCategoria.Name = "txtCategoria";
            txtCategoria.Size = new Size(590, 27);
            txtCategoria.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.Location = new Point(164, 28);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(590, 27);
            txtNombre.TabIndex = 0;
            // 
            // dgvMenu
            // 
            dgvMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMenu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMenu.Location = new Point(0, 179);
            dgvMenu.Name = "dgvMenu";
            dgvMenu.RowHeadersWidth = 51;
            dgvMenu.Size = new Size(960, 485);
            dgvMenu.TabIndex = 1;
            // 
            // UC_Menu
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(panelInputs);
            Controls.Add(dgvMenu);
            Name = "UC_Menu";
            Size = new Size(960, 664);
            Load += UC_Menu_Load;
            panelInputs.ResumeLayout(false);
            panelInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMenu).EndInit();
            ResumeLayout(false);
        }
        private Panel panelInputs;
        private TextBox txtCategoria;
        private TextBox txtNombre;
        private Label lblCategoria;
        private Label lblNombre;
        private TextBox txtPrecio;
        private Label lblPrecio;
        private Button btnGuardar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvMenu;
    }
}
