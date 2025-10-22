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
            components = new System.ComponentModel.Container();
            panelInputs = new Panel();
            cmbCategoria = new ComboBox();
            btnActualizar = new Button();
            btnGuardar = new Button();
            btnEliminar = new Button();
            txtPrecio = new TextBox();
            lblPrecio = new Label();
            lblCategoria = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            dgvMenu = new DataGridView();
            contextMenuPlatillo = new ContextMenuStrip(components);
            itemGestionarReceta = new ToolStripMenuItem();
            panelEdicionReceta = new Panel();
            btnVolverAlMenu = new Button();
            btnEliminarIngrediente = new Button();
            btnAgregarIngrediente = new Button();
            txtCantidadReceta = new TextBox();
            lblCantidadReceta = new Label();
            lblProducto = new Label();
            cmbProductos = new ComboBox();
            dgvReceta = new DataGridView();
            lblTituloReceta = new Label();
            itemEditarPlatillo = new ToolStripMenuItem();
            panelInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMenu).BeginInit();
            contextMenuPlatillo.SuspendLayout();
            panelEdicionReceta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceta).BeginInit();
            SuspendLayout();
            // 
            // panelInputs
            // 
            panelInputs.Controls.Add(cmbCategoria);
            panelInputs.Controls.Add(btnActualizar);
            panelInputs.Controls.Add(btnGuardar);
            panelInputs.Controls.Add(btnEliminar);
            panelInputs.Controls.Add(txtPrecio);
            panelInputs.Controls.Add(lblPrecio);
            panelInputs.Controls.Add(lblCategoria);
            panelInputs.Controls.Add(lblNombre);
            panelInputs.Controls.Add(txtNombre);
            panelInputs.Dock = DockStyle.Top;
            panelInputs.Location = new Point(0, 0);
            panelInputs.Name = "panelInputs";
            panelInputs.Size = new Size(960, 180);
            panelInputs.TabIndex = 0;
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(164, 75);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(590, 28);
            cmbCategoria.TabIndex = 15;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActualizar.Location = new Point(802, 119);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(104, 49);
            btnActualizar.TabIndex = 14;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += BtnActualizar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.Location = new Point(802, 13);
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
            btnEliminar.Location = new Point(802, 66);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(104, 49);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += BtnEliminar_Click;
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
            dgvMenu.ContextMenuStrip = contextMenuPlatillo;
            dgvMenu.Location = new Point(0, 180);
            dgvMenu.Name = "dgvMenu";
            dgvMenu.RowHeadersWidth = 51;
            dgvMenu.Size = new Size(960, 484);
            dgvMenu.TabIndex = 1;
            dgvMenu.CellMouseDown += DgvMenu_CellMouseDown;
            // 
            // contextMenuPlatillo
            // 
            contextMenuPlatillo.ImageScalingSize = new Size(20, 20);
            contextMenuPlatillo.Items.AddRange(new ToolStripItem[] { itemGestionarReceta, itemEditarPlatillo });
            contextMenuPlatillo.Name = "contextMenuPlatillo";
            contextMenuPlatillo.Size = new Size(211, 80);
            // 
            // itemGestionarReceta
            // 
            itemGestionarReceta.Name = "itemGestionarReceta";
            itemGestionarReceta.Size = new Size(211, 24);
            itemGestionarReceta.Text = "Gestionar Receta";
            itemGestionarReceta.Click += ItemGestionarReceta_Click;
            // 
            // panelEdicionReceta
            // 
            panelEdicionReceta.Controls.Add(btnVolverAlMenu);
            panelEdicionReceta.Controls.Add(btnEliminarIngrediente);
            panelEdicionReceta.Controls.Add(btnAgregarIngrediente);
            panelEdicionReceta.Controls.Add(txtCantidadReceta);
            panelEdicionReceta.Controls.Add(lblCantidadReceta);
            panelEdicionReceta.Controls.Add(lblProducto);
            panelEdicionReceta.Controls.Add(cmbProductos);
            panelEdicionReceta.Controls.Add(dgvReceta);
            panelEdicionReceta.Controls.Add(lblTituloReceta);
            panelEdicionReceta.Location = new Point(0, 0);
            panelEdicionReceta.Name = "panelEdicionReceta";
            panelEdicionReceta.Size = new Size(960, 664);
            panelEdicionReceta.TabIndex = 3;
            panelEdicionReceta.Visible = false;
            // 
            // btnVolverAlMenu
            // 
            btnVolverAlMenu.Location = new Point(802, 141);
            btnVolverAlMenu.Name = "btnVolverAlMenu";
            btnVolverAlMenu.Size = new Size(104, 55);
            btnVolverAlMenu.TabIndex = 9;
            btnVolverAlMenu.Text = "Volver";
            btnVolverAlMenu.UseVisualStyleBackColor = true;
            btnVolverAlMenu.Click += BtnVolverAlMenu_Click;
            // 
            // btnEliminarIngrediente
            // 
            btnEliminarIngrediente.Location = new Point(802, 79);
            btnEliminarIngrediente.Name = "btnEliminarIngrediente";
            btnEliminarIngrediente.Size = new Size(104, 55);
            btnEliminarIngrediente.TabIndex = 7;
            btnEliminarIngrediente.Text = "Eliminar";
            btnEliminarIngrediente.UseVisualStyleBackColor = true;
            btnEliminarIngrediente.Click += BtnEliminarIngrediente_Click;
            // 
            // btnAgregarIngrediente
            // 
            btnAgregarIngrediente.Location = new Point(802, 15);
            btnAgregarIngrediente.Name = "btnAgregarIngrediente";
            btnAgregarIngrediente.Size = new Size(104, 55);
            btnAgregarIngrediente.TabIndex = 6;
            btnAgregarIngrediente.Text = "Agregar";
            btnAgregarIngrediente.UseVisualStyleBackColor = true;
            btnAgregarIngrediente.Click += BtnAgregarIngrediente_Click;
            // 
            // txtCantidadReceta
            // 
            txtCantidadReceta.Location = new Point(164, 123);
            txtCantidadReceta.Name = "txtCantidadReceta";
            txtCantidadReceta.Size = new Size(590, 27);
            txtCantidadReceta.TabIndex = 5;
            // 
            // lblCantidadReceta
            // 
            lblCantidadReceta.AutoSize = true;
            lblCantidadReceta.Location = new Point(44, 124);
            lblCantidadReceta.Name = "lblCantidadReceta";
            lblCantidadReceta.Size = new Size(69, 20);
            lblCantidadReceta.TabIndex = 4;
            lblCantidadReceta.Text = "Cantidad";
            // 
            // lblProducto
            // 
            lblProducto.AutoSize = true;
            lblProducto.Location = new Point(44, 65);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(69, 20);
            lblProducto.TabIndex = 3;
            lblProducto.Text = "Producto";
            // 
            // cmbProductos
            // 
            cmbProductos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductos.FormattingEnabled = true;
            cmbProductos.Location = new Point(164, 65);
            cmbProductos.Name = "cmbProductos";
            cmbProductos.Size = new Size(590, 28);
            cmbProductos.TabIndex = 2;
            // 
            // dgvReceta
            // 
            dgvReceta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReceta.Location = new Point(0, 214);
            dgvReceta.Name = "dgvReceta";
            dgvReceta.RowHeadersWidth = 51;
            dgvReceta.Size = new Size(960, 450);
            dgvReceta.TabIndex = 1;
            // 
            // lblTituloReceta
            // 
            lblTituloReceta.Dock = DockStyle.Top;
            lblTituloReceta.Font = new Font("Segoe UI", 13F);
            lblTituloReceta.Location = new Point(0, 0);
            lblTituloReceta.Name = "lblTituloReceta";
            lblTituloReceta.Size = new Size(960, 40);
            lblTituloReceta.TabIndex = 0;
            lblTituloReceta.Text = "label1";
            lblTituloReceta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // itemEditarPlatillo
            // 
            itemEditarPlatillo.Name = "itemEditarPlatillo";
            itemEditarPlatillo.Size = new Size(210, 24);
            itemEditarPlatillo.Text = "Editar Platillo";
            itemEditarPlatillo.Click += ItemEditarPlatillo_Click;
            // 
            // UC_Menu
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(panelEdicionReceta);
            Controls.Add(panelInputs);
            Controls.Add(dgvMenu);
            Name = "UC_Menu";
            Size = new Size(960, 664);
            Load += UC_Menu_Load;
            panelInputs.ResumeLayout(false);
            panelInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMenu).EndInit();
            contextMenuPlatillo.ResumeLayout(false);
            panelEdicionReceta.ResumeLayout(false);
            panelEdicionReceta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceta).EndInit();
            ResumeLayout(false);
        }
        private Panel panelInputs;
        private TextBox txtNombre;
        private Label lblCategoria;
        private Label lblNombre;
        private TextBox txtPrecio;
        private Label lblPrecio;
        private Button btnGuardar;
        private Button btnEliminar;
        private DataGridView dgvMenu;
        private ComboBox cmbCategoria;
        private Button btnActualizar;
        private ContextMenuStrip contextMenuPlatillo;
        private ToolStripMenuItem itemGestionarReceta;
        private Panel panelEdicionReceta;
        private Label lblTituloReceta;
        private DataGridView dgvReceta;
        private ComboBox cmbProductos;
        private Label lblProducto;
        private Label lblCantidadReceta;
        private TextBox txtCantidadReceta;
        private Button btnVolverAlMenu;
        private Button btnEliminarIngrediente;
        private Button btnAgregarIngrediente;
        private ToolStripMenuItem itemEditarPlatillo;
    }
}
