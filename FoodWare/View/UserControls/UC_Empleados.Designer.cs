using System.Drawing;
using System.Windows.Forms;

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
            btnResetPassword = new Button();
            lblRol = new Label();
            cmbRol = new ComboBox();
            btnLimpiar = new Button();
            chkActivo = new CheckBox();
            btnEliminar = new Button();
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
            tlpInputs.ColumnCount = 4;
            tlpInputs.ColumnStyles.Add(new ColumnStyle());
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpInputs.Controls.Add(lblNombreCompleto, 0, 0);
            tlpInputs.Controls.Add(txtNombreCompleto, 1, 0);
            tlpInputs.Controls.Add(btnGuardar, 3, 0);
            tlpInputs.Controls.Add(lblNombreUsuario, 0, 1);
            tlpInputs.Controls.Add(txtNombreUsuario, 1, 1);
            tlpInputs.Controls.Add(btnActualizar, 3, 1);
            tlpInputs.Controls.Add(lblPassword, 0, 2);
            tlpInputs.Controls.Add(txtPassword, 1, 2);
            tlpInputs.Controls.Add(lblRol, 0, 3);
            tlpInputs.Controls.Add(cmbRol, 1, 3);
            tlpInputs.Controls.Add(btnResetPassword, 3, 3);
            tlpInputs.Controls.Add(chkActivo, 1, 4);
            tlpInputs.Controls.Add(btnLimpiar, 3, 2);
            tlpInputs.Controls.Add(btnEliminar, 3, 4);
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
            lblNombreCompleto.Location = new Point(23, 35);
            lblNombreCompleto.Name = "lblNombreCompleto";
            lblNombreCompleto.Size = new Size(134, 20);
            lblNombreCompleto.TabIndex = 0;
            lblNombreCompleto.Text = "Nombre Completo";
            // 
            // txtNombreCompleto
            // 
            txtNombreCompleto.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNombreCompleto.Location = new Point(163, 31);
            txtNombreCompleto.Name = "txtNombreCompleto";
            txtNombreCompleto.Size = new Size(644, 27);
            txtNombreCompleto.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.None;
            btnGuardar.Location = new Point(823, 23);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(114, 44);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Anchor = AnchorStyles.Right;
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(39, 85);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(118, 20);
            lblNombreUsuario.TabIndex = 2;
            lblNombreUsuario.Text = "Nombre Usuario";
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNombreUsuario.Location = new Point(163, 81);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(644, 27);
            txtNombreUsuario.TabIndex = 3;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.None;
            btnActualizar.Location = new Point(823, 73);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(114, 44);
            btnActualizar.TabIndex = 10;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += BtnActualizar_Click;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.Right;
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(68, 135);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(89, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Contraseña*";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.Location = new Point(163, 131);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(644, 27);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnResetPassword
            // 
            btnResetPassword.Anchor = AnchorStyles.None;
            btnResetPassword.Location = new Point(823, 173);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(114, 44);
            btnResetPassword.TabIndex = 11;
            btnResetPassword.Text = "Reset Pass";
            btnResetPassword.UseVisualStyleBackColor = true;
            btnResetPassword.Click += BtnResetPassword_Click;
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.Right;
            lblRol.AutoSize = true;
            lblRol.Location = new Point(126, 185);
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
            cmbRol.Location = new Point(163, 181);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(644, 28);
            cmbRol.TabIndex = 7;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.None;
            btnLimpiar.Location = new Point(823, 123);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(114, 44);
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
            chkActivo.Location = new Point(163, 233);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(73, 24);
            chkActivo.TabIndex = 8;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Anchor = AnchorStyles.None;
            btnEliminar.Location = new Point(823, 223);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(114, 44);
            btnEliminar.TabIndex = 13;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += BtnEliminar_Click;
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
            contextMenuEmpleados.Size = new Size(190, 28);
            // 
            // itemEditar
            // 
            itemEditar.Name = "itemEditar";
            itemEditar.Size = new Size(189, 24);
            itemEditar.Text = "Editar Empleado";
            itemEditar.Click += ItemEditar_Click;
            // 
            // UC_Empleados
            // 
            AutoScaleMode = AutoScaleMode.None;
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
        private Button btnResetPassword;
        private Button btnLimpiar;
        private Button btnEliminar;
        private DataGridView dgvEmpleados;
        private ContextMenuStrip contextMenuEmpleados;
        private ToolStripMenuItem itemEditar;
    }
}