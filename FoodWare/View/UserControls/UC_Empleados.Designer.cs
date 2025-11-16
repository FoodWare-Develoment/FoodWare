namespace FoodWare.View.UserControls
{
    partial class UC_Empleados
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tlpInputs = new TableLayoutPanel();
            lblNombreCompleto = new Label();
            txtNombreCompleto = new TextBox();
            btnGuardar = new Button();
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            btnActualizar = new Button();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnDesactivar = new Button();
            lblRol = new Label();
            cmbRol = new ComboBox();
            btnLimpiar = new Button();
            chkActivo = new CheckBox();
            dgvEmpleados = new DataGridView();
            contextMenuEmpleados = new ContextMenuStrip(components);
            itemEditar = new ToolStripMenuItem();
            tlpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).BeginInit();
            contextMenuEmpleados.SuspendLayout();
            SuspendLayout();
            // 
            // tlpInputs
            // 
            tlpInputs.ColumnCount = 4; // --- CAMBIO: 3 a 4 ---
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F)); // --- NUEVA COLUMNA DE PADDING ---
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpInputs.Controls.Add(lblNombreCompleto, 0, 0);
            tlpInputs.Controls.Add(txtNombreCompleto, 1, 0);
            tlpInputs.Controls.Add(btnGuardar, 3, 0); // --- CAMBIO: Col 2 a 3 ---
            tlpInputs.Controls.Add(lblNombreUsuario, 0, 1);
            tlpInputs.Controls.Add(txtNombreUsuario, 1, 1);
            tlpInputs.Controls.Add(btnActualizar, 3, 1); // --- CAMBIO: Col 2 a 3 ---
            tlpInputs.Controls.Add(lblPassword, 0, 2);
            tlpInputs.Controls.Add(txtPassword, 1, 2);
            tlpInputs.Controls.Add(btnDesactivar, 3, 2); // --- CAMBIO: Col 2 a 3 ---
            tlpInputs.Controls.Add(lblRol, 0, 3);
            tlpInputs.Controls.Add(cmbRol, 1, 3);
            tlpInputs.Controls.Add(btnLimpiar, 3, 3); // --- CAMBIO: Col 2 a 3 ---
            tlpInputs.Controls.Add(chkActivo, 1, 4);
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Location = new Point(0, 0);
            tlpInputs.Name = "tlpInputs";
            tlpInputs.Padding = new Padding(20);
            tlpInputs.RowCount = 5;
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpInputs.Size = new Size(960, 270);
            tlpInputs.TabIndex = 0;
            // 
            // lblNombreCompleto
            // 
            lblNombreCompleto.Anchor = AnchorStyles.Right;
            lblNombreCompleto.AutoSize = true;
            lblNombreCompleto.Name = "lblNombreCompleto";
            lblNombreCompleto.Size = new Size(131, 20);
            lblNombreCompleto.TabIndex = 0;
            lblNombreCompleto.Text = "Nombre Completo";
            // 
            // txtNombreCompleto
            // 
            txtNombreCompleto.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNombreCompleto.Location = new Point(160, 31);
            txtNombreCompleto.Name = "txtNombreCompleto";
            txtNombreCompleto.Size = new Size(647, 27); // Ancho se ajustará
            txtNombreCompleto.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.None;
            btnGuardar.Location = new Point(823, 22);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(114, 45);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Anchor = AnchorStyles.Right;
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(110, 20);
            lblNombreUsuario.TabIndex = 2;
            lblNombreUsuario.Text = "Nombre Usuario";
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNombreUsuario.Location = new Point(160, 81);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(647, 27);
            txtNombreUsuario.TabIndex = 3;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.None;
            btnActualizar.Location = new Point(823, 72);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(114, 45);
            btnActualizar.TabIndex = 10;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += BtnActualizar_Click;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.Right;
            lblPassword.AutoSize = true;
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(88, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Contraseña*";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.Location = new Point(160, 131);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(647, 27);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnDesactivar
            // 
            btnDesactivar.Anchor = AnchorStyles.None;
            btnDesactivar.Location = new Point(823, 122);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(114, 45);
            btnDesactivar.TabIndex = 11;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += BtnDesactivar_Click;
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.Right;
            lblRol.AutoSize = true;
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(31, 20);
            lblRol.TabIndex = 6;
            lblRol.Text = "Rol";
            // 
            // cmbRol
            // 
            cmbRol.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(160, 181);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(647, 28);
            cmbRol.TabIndex = 7;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.None;
            btnLimpiar.Location = new Point(823, 172);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(114, 45);
            btnLimpiar.TabIndex = 12;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += BtnLimpiar_Click;
            // 
            // chkActivo
            // 
            chkActivo.Anchor = AnchorStyles.Left;
            chkActivo.AutoSize = true;
            chkActivo.Checked = true;
            chkActivo.CheckState = CheckState.Checked;
            chkActivo.Location = new Point(160, 233);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(73, 24);
            chkActivo.TabIndex = 8;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // dgvEmpleados
            // 
            dgvEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmpleados.ContextMenuStrip = contextMenuEmpleados;
            dgvEmpleados.Dock = DockStyle.Fill;
            dgvEmpleados.Location = new Point(0, 270);
            dgvEmpleados.Name = "dgvEmpleados";
            dgvEmpleados.RowHeadersWidth = 51;
            dgvEmpleados.Size = new Size(960, 394);
            dgvEmpleados.TabIndex = 1;
            dgvEmpleados.CellMouseDown += DgvEmpleados_CellMouseDown;
            // 
            // contextMenuEmpleados
            // 
            contextMenuEmpleados.ImageScalingSize = new Size(20, 20);
            contextMenuEmpleados.Items.AddRange(new ToolStripItem[] { itemEditar });
            contextMenuEmpleados.Name = "contextMenuEmpleados";
            contextMenuEmpleados.Size = new Size(181, 28);
            // 
            // itemEditar
            // 
            itemEditar.Name = "itemEditar";
            itemEditar.Size = new Size(180, 24);
            itemEditar.Text = "Editar Empleado";
            itemEditar.Click += ItemEditar_Click;
            // 
            // UC_Empleados
            // 
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvEmpleados);
            Controls.Add(tlpInputs);
            Name = "UC_Empleados";
            Size = new Size(960, 664);
            Load += UC_Empleados_Load;
            tlpInputs.ResumeLayout(false);
            tlpInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).EndInit();
            contextMenuEmpleados.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TableLayoutPanel tlpInputs;
        private Label lblNombreCompleto;
        private TextBox txtNombreCompleto;
        private Label lblNombreUsuario;
        private TextBox txtNombreUsuario;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblRol;
        private ComboBox cmbRol;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnDesactivar;
        private Button btnLimpiar;
        private DataGridView dgvEmpleados;
        private ContextMenuStrip contextMenuEmpleados;
        private ToolStripMenuItem itemEditar;
    }
}