using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Model.Interfaces;
using FoodWare.View.Helpers;
using FoodWare.Model.Entities;
using Microsoft.VisualBasic; // Para InputBox

namespace FoodWare.View.UserControls
{
    public partial class UC_Empleados : UserControl
    {
        // --- Constantes (S1192) ---
        private const string TituloExito = "Éxito";
        private const string TituloError = "Error";
        private const string TituloDatoInvalido = "Datos Inválidos";
        private const string TituloNoSeleccionado = "Ningún empleado seleccionado";
        private const string TituloAccionBloqueada = "Acción Bloqueada";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado. Contacte al administrador.";

        private readonly EmpleadosController _controller;
        private UsuarioDto? _usuarioSeleccionado;

        public UC_Empleados()
        {
            InitializeComponent();

            _controller = new EmpleadosController(
                new UsuarioSqlRepository(),
                new RolSqlRepository()
            );

            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;
            EstilosApp.EstiloPanel(tlpInputs, EstilosApp.ColorFondo);
            EstilosApp.EstiloLabelModulo(lblNombreCompleto);
            EstilosApp.EstiloLabelModulo(lblNombreUsuario);
            EstilosApp.EstiloLabelModulo(lblPassword);
            EstilosApp.EstiloLabelModulo(lblRol);
            EstilosApp.EstiloTextBoxModulo(txtNombreCompleto);
            EstilosApp.EstiloTextBoxModulo(txtNombreUsuario);
            EstilosApp.EstiloTextBoxModulo(txtPassword);
            EstilosApp.EstiloComboBoxModulo(cmbRol);
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizar);
            EstilosApp.EstiloBotonModuloAlerta(btnResetPassword);
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);
            EstilosApp.EstiloDataGridView(dgvEmpleados);

            btnResetPassword.Visible = (UserSession.NombreRol == "Administrador");
        }

        private async void UC_Empleados_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            await CargarRolesDropdownAsync();
            await CargarGridEmpleadosAsync();
        }

        private async Task CargarRolesDropdownAsync()
        {
            try
            {
                var roles = await _controller.CargarRolesAsync();
                cmbRol.DataSource = roles;
                cmbRol.DisplayMember = "NombreRol";
                cmbRol.ValueMember = "IdRol";
                cmbRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar roles: {ex.Message}");
                MessageBox.Show("Error al cargar roles. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarGridEmpleadosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvEmpleados.DataSource = null;
                var empleados = await _controller.CargarEmpleadosAsync();
                dgvEmpleados.DataSource = empleados;

                // Configurar columnas
                if (dgvEmpleados.Columns["IdUsuario"] is DataGridViewColumn colId) colId.Visible = false;
                if (dgvEmpleados.Columns["IdRol"] is DataGridViewColumn colIdRol) colIdRol.Visible = false;

                if (dgvEmpleados.Columns["NombreCompleto"] is DataGridViewColumn colNom)
                {
                    colNom.HeaderText = "Nombre Completo";
                    colNom.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dgvEmpleados.Columns["NombreUsuario"] is DataGridViewColumn colUser)
                {
                    colUser.HeaderText = "Usuario";
                    colUser.Width = 150;
                }
                if (dgvEmpleados.Columns["NombreRol"] is DataGridViewColumn colRol)
                {
                    colRol.HeaderText = "Rol";
                    colRol.Width = 150;
                }
                if (dgvEmpleados.Columns["Estado"] is DataGridViewColumn colEstado)
                {
                    colEstado.Width = 100;
                }
                if (dgvEmpleados.Columns["Activo"] is DataGridViewColumn colAct)
                {
                    colAct.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar empleados: {ex.Message}");
                MessageBox.Show("Error al cargar empleados. Contacte al administrador.", TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LimpiarCampos()
        {
            _usuarioSeleccionado = null;
            txtNombreCompleto.Clear();
            txtNombreUsuario.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = true;

            txtPassword.Enabled = true;
            lblPassword.Visible = true;
            lblPassword.Text = "Contraseña*";
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreCompleto = txtNombreCompleto.Text;
                string nombreUsuario = txtNombreUsuario.Text;
                string password = txtPassword.Text;
                int idRol = (int)(cmbRol.SelectedValue ?? 0);
                bool activo = chkActivo.Checked;

                this.Cursor = Cursors.WaitCursor;
                await _controller.GuardarNuevoEmpleadoAsync(nombreCompleto, nombreUsuario, password, idRol, activo);

                MessageBox.Show("¡Empleado guardado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar empleado: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado de la lista para actualizar.", TituloNoSeleccionado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 1. Recoger datos
                int idUsuario = _usuarioSeleccionado.IdUsuario;
                string nombreCompleto = txtNombreCompleto.Text;
                string nombreUsuario = txtNombreUsuario.Text;
                int idRol = (int)(cmbRol.SelectedValue ?? 0);
                bool activo = chkActivo.Checked;
                string rolDelEditado = _usuarioSeleccionado.NombreRol; // <-- Se pasa el Rol

                // 2. Llamar al controlador
                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarEmpleadoAsync(idUsuario, nombreCompleto, nombreUsuario, idRol, activo, rolDelEditado);

                // 3. Actualizar UI
                MessageBox.Show("¡Empleado actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
            }
            catch (ArgumentException aex) // Errores de validación
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ioex) // Errores de Regla de Negocio
            {
                MessageBox.Show(ioex.Message, TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex) // Errores inesperados
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar empleado: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnResetPassword_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado de la lista primero.", TituloNoSeleccionado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (UserSession.NombreRol == "Gerente" && _usuarioSeleccionado.NombreRol == "Administrador")
            {
                MessageBox.Show("Un Gerente no tiene permisos para resetear la contraseña de un Administrador.", TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string? nuevaPassword = Interaction.InputBox($"Resetear contraseña para:\n{_usuarioSeleccionado.NombreUsuario}", "Resetear Contraseña", "");

            if (string.IsNullOrWhiteSpace(nuevaPassword))
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                await _controller.ResetearPasswordAsync(_usuarioSeleccionado.IdUsuario, nuevaPassword);
                MessageBox.Show("¡Contraseña actualizada exitosamente!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al resetear password: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ItemEditar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow != null && dgvEmpleados.CurrentRow.DataBoundItem is UsuarioDto usuario)
            {
                _usuarioSeleccionado = usuario;

                txtNombreCompleto.Text = usuario.NombreCompleto;
                txtNombreUsuario.Text = usuario.NombreUsuario;
                cmbRol.SelectedValue = usuario.IdRol;
                chkActivo.Checked = usuario.Activo;

                txtPassword.Clear();
                txtPassword.Enabled = false;
                lblPassword.Visible = false;
            }
        }

        private void DgvEmpleados_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvEmpleados.CurrentCell = dgvEmpleados.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }
    }
}