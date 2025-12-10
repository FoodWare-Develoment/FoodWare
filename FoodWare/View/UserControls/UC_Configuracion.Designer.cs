namespace FoodWare.View.UserControls
{
    partial class UC_Configuracion
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            panelMain = new System.Windows.Forms.Panel();
            btnSalir = new System.Windows.Forms.Button();
            btnCerrarSesion = new System.Windows.Forms.Button();
            btnGuardar = new System.Windows.Forms.Button();
            gbFinanciero = new System.Windows.Forms.GroupBox();
            tlpFinanciero = new System.Windows.Forms.TableLayoutPanel();
            lblImpuesto = new System.Windows.Forms.Label();
            txtImpuesto = new System.Windows.Forms.TextBox();
            lblMoneda = new System.Windows.Forms.Label();
            txtMoneda = new System.Windows.Forms.TextBox();
            gbDatosGenerales = new System.Windows.Forms.GroupBox();
            tlpDatos = new System.Windows.Forms.TableLayoutPanel();
            lblNombre = new System.Windows.Forms.Label();
            txtNombre = new System.Windows.Forms.TextBox();
            lblDireccion = new System.Windows.Forms.Label();
            txtDireccion = new System.Windows.Forms.TextBox();
            lblMensaje = new System.Windows.Forms.Label();
            txtMensaje = new System.Windows.Forms.TextBox();
            lblTitulo = new System.Windows.Forms.Label();
            panelMain.SuspendLayout();
            gbFinanciero.SuspendLayout();
            tlpFinanciero.SuspendLayout();
            gbDatosGenerales.SuspendLayout();
            tlpDatos.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(btnSalir);
            panelMain.Controls.Add(btnCerrarSesion);
            panelMain.Controls.Add(btnGuardar);
            panelMain.Controls.Add(gbFinanciero);
            panelMain.Controls.Add(gbDatosGenerales);
            panelMain.Controls.Add(lblTitulo);
            panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            panelMain.Location = new System.Drawing.Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new System.Windows.Forms.Padding(20);
            panelMain.Size = new System.Drawing.Size(960, 664);
            panelMain.TabIndex = 0;
            // 
            // btnSalir
            // 
            btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSalir.ForeColor = System.Drawing.Color.White;
            btnSalir.Location = new System.Drawing.Point(760, 20);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(180, 40);
            btnSalir.TabIndex = 5;
            btnSalir.Text = "Salir del Sistema";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            btnCerrarSesion.Location = new System.Drawing.Point(574, 20);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new System.Drawing.Size(180, 40);
            btnCerrarSesion.TabIndex = 4;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += new System.EventHandler(this.BtnCerrarSesion_Click);
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnGuardar.Location = new System.Drawing.Point(760, 430);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new System.Drawing.Size(180, 50);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // gbFinanciero
            // 
            gbFinanciero.Controls.Add(tlpFinanciero);
            gbFinanciero.Dock = System.Windows.Forms.DockStyle.Top;
            gbFinanciero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            gbFinanciero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            gbFinanciero.Location = new System.Drawing.Point(20, 260);
            gbFinanciero.Name = "gbFinanciero";
            gbFinanciero.Padding = new System.Windows.Forms.Padding(10);
            gbFinanciero.Size = new System.Drawing.Size(920, 150);
            gbFinanciero.TabIndex = 2;
            gbFinanciero.TabStop = false;
            gbFinanciero.Text = "Configuración Financiera";
            // 
            // tlpFinanciero
            // 
            tlpFinanciero.ColumnCount = 2;
            tlpFinanciero.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            tlpFinanciero.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpFinanciero.Controls.Add(lblImpuesto, 0, 0);
            tlpFinanciero.Controls.Add(txtImpuesto, 1, 0);
            tlpFinanciero.Controls.Add(lblMoneda, 0, 1);
            tlpFinanciero.Controls.Add(txtMoneda, 1, 1);
            tlpFinanciero.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpFinanciero.Location = new System.Drawing.Point(10, 33);
            tlpFinanciero.Name = "tlpFinanciero";
            tlpFinanciero.RowCount = 2;
            tlpFinanciero.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpFinanciero.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpFinanciero.Size = new System.Drawing.Size(900, 107);
            tlpFinanciero.TabIndex = 0;
            // 
            // lblImpuesto
            // 
            lblImpuesto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblImpuesto.AutoSize = true;
            lblImpuesto.Location = new System.Drawing.Point(24, 13);
            lblImpuesto.Name = "lblImpuesto";
            lblImpuesto.Size = new System.Drawing.Size(123, 23);
            lblImpuesto.TabIndex = 0;
            lblImpuesto.Text = "Impuesto (%):";
            // 
            // txtImpuesto
            // 
            txtImpuesto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            txtImpuesto.Location = new System.Drawing.Point(153, 10);
            txtImpuesto.Name = "txtImpuesto";
            txtImpuesto.Size = new System.Drawing.Size(150, 30);
            txtImpuesto.TabIndex = 1;
            // 
            // lblMoneda
            // 
            lblMoneda.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblMoneda.AutoSize = true;
            lblMoneda.Location = new System.Drawing.Point(68, 63);
            lblMoneda.Name = "lblMoneda";
            lblMoneda.Size = new System.Drawing.Size(79, 23);
            lblMoneda.TabIndex = 2;
            lblMoneda.Text = "Moneda:";
            // 
            // txtMoneda
            // 
            txtMoneda.Anchor = System.Windows.Forms.AnchorStyles.Left;
            txtMoneda.Location = new System.Drawing.Point(153, 60);
            txtMoneda.Name = "txtMoneda";
            txtMoneda.Size = new System.Drawing.Size(150, 30);
            txtMoneda.TabIndex = 3;
            // 
            // gbDatosGenerales
            // 
            gbDatosGenerales.Controls.Add(tlpDatos);
            gbDatosGenerales.Dock = System.Windows.Forms.DockStyle.Top;
            gbDatosGenerales.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            gbDatosGenerales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            gbDatosGenerales.Location = new System.Drawing.Point(20, 60);
            gbDatosGenerales.Name = "gbDatosGenerales";
            gbDatosGenerales.Padding = new System.Windows.Forms.Padding(10);
            gbDatosGenerales.Size = new System.Drawing.Size(920, 200);
            gbDatosGenerales.TabIndex = 1;
            gbDatosGenerales.TabStop = false;
            gbDatosGenerales.Text = "Información del Restaurante";
            // 
            // tlpDatos
            // 
            tlpDatos.ColumnCount = 2;
            tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpDatos.Controls.Add(lblNombre, 0, 0);
            tlpDatos.Controls.Add(txtNombre, 1, 0);
            tlpDatos.Controls.Add(lblDireccion, 0, 1);
            tlpDatos.Controls.Add(txtDireccion, 1, 1);
            tlpDatos.Controls.Add(lblMensaje, 0, 2);
            tlpDatos.Controls.Add(txtMensaje, 1, 2);
            tlpDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpDatos.Location = new System.Drawing.Point(10, 33);
            tlpDatos.Name = "tlpDatos";
            tlpDatos.RowCount = 3;
            tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpDatos.Size = new System.Drawing.Size(900, 157);
            tlpDatos.TabIndex = 0;
            // 
            // lblNombre
            // 
            lblNombre.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblNombre.AutoSize = true;
            lblNombre.Location = new System.Drawing.Point(69, 13);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new System.Drawing.Size(78, 23);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            txtNombre.Location = new System.Drawing.Point(153, 10);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new System.Drawing.Size(744, 30);
            txtNombre.TabIndex = 1;
            // 
            // lblDireccion
            // 
            lblDireccion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new System.Drawing.Point(59, 63);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new System.Drawing.Size(88, 23);
            lblDireccion.TabIndex = 2;
            lblDireccion.Text = "Dirección:";
            // 
            // txtDireccion
            // 
            txtDireccion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            txtDireccion.Location = new System.Drawing.Point(153, 60);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new System.Drawing.Size(744, 30);
            txtDireccion.TabIndex = 3;
            // 
            // lblMensaje
            // 
            lblMensaje.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new System.Drawing.Point(17, 113);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new System.Drawing.Size(130, 23);
            lblMensaje.TabIndex = 4;
            lblMensaje.Text = "Mensaje Ticket:";
            // 
            // txtMensaje
            // 
            txtMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            txtMensaje.Location = new System.Drawing.Point(153, 110);
            txtMensaje.Name = "txtMensaje";
            txtMensaje.Size = new System.Drawing.Size(744, 30);
            txtMensaje.TabIndex = 5;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitulo.Location = new System.Drawing.Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            lblTitulo.Size = new System.Drawing.Size(193, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configuración del Sistema";
            // 
            // UC_Configuracion
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(panelMain);
            Name = "UC_Configuracion";
            Size = new System.Drawing.Size(960, 664);
            Load += new System.EventHandler(this.UC_Configuracion_Load);
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            gbFinanciero.ResumeLayout(false);
            tlpFinanciero.ResumeLayout(false);
            tlpFinanciero.PerformLayout();
            gbDatosGenerales.ResumeLayout(false);
            tlpDatos.ResumeLayout(false);
            tlpDatos.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbDatosGenerales;
        private System.Windows.Forms.TableLayoutPanel tlpDatos;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.GroupBox gbFinanciero;
        private System.Windows.Forms.TableLayoutPanel tlpFinanciero;
        private System.Windows.Forms.Label lblImpuesto;
        private System.Windows.Forms.TextBox txtImpuesto;
        private System.Windows.Forms.Label lblMoneda;
        private System.Windows.Forms.TextBox txtMoneda;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Button btnSalir;
    }
}