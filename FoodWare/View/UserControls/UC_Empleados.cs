using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using FoodWare.Controller.Logic;
using FoodWare.Model.DataAccess;
using FoodWare.Shared.Interfaces;
using FoodWare.View.Helpers;
using FoodWare.Shared.Entities;
using Microsoft.VisualBasic;

namespace FoodWare.View.UserControls
{
    public partial class UC_Empleados : UserControl
    {
        private const string TituloExito = "Éxito";
        private const string TituloError = "Error";
        private const string TituloDatoInvalido = "Datos Inválidos";
        private const string TituloNoSeleccionado = "Ningún empleado seleccionado";
        private const string TituloAccionBloqueada = "Acción Bloqueada";
        private const string MsgErrorInesperado = "Ocurrió un error inesperado. Contacte al administrador.";

        private readonly EmpleadosController _controller;
        private UsuarioDto? _usuarioSeleccionado;
        private bool _modoEdicion = false;

        public UC_Empleados()
        {
            InitializeComponent();
            _controller = new EmpleadosController(new UsuarioSqlRepository(), new RolSqlRepository());
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
            EstilosApp.EstiloBotonModuloAlerta(btnEliminar);

            EstilosApp.EstiloDataGridView(dgvEmpleados);

            bool esAdmin = UserSession.NombreRol == "Administrador";
            btnResetPassword.Visible = esAdmin;
            btnEliminar.Visible = esAdmin;
        }

        private async void UC_Empleados_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            await CargarRolesDropdownAsync();
            await CargarGridEmpleadosAsync();
            EstablecerModoEdicion(false);
        }

        private void EstablecerModoEdicion(bool activo)
        {
            _modoEdicion = activo;
            if (activo)
            {
                btnGuardar.Visible = false;
                btnActualizar.Visible = true;
                btnResetPassword.Visible = true;
                btnEliminar.Visible = true;
                btnLimpiar.Text = "Cancelar";

                txtPassword.Enabled = false;
                lblPassword.Visible = false;
                txtPassword.Visible = false;
            }
            else
            {
                btnGuardar.Visible = true;
                btnActualizar.Visible = false;
                btnResetPassword.Visible = false;
                btnEliminar.Visible = false;
                btnLimpiar.Text = "Limpiar";

                txtPassword.Enabled = true;
                lblPassword.Visible = true;
                txtPassword.Visible = true;
                lblPassword.Text = "Contraseña*";

                _usuarioSeleccionado = null;
            }
        }

        private void LimpiarCampos()
        {
            txtNombreCompleto.Clear();
            txtNombreUsuario.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = true;
            EstablecerModoEdicion(false);
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
                EstablecerModoEdicion(true);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_modoEdicion) return;

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
            catch (ArgumentException aex) { MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error guardar empleado: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _usuarioSeleccionado == null) return;

            try
            {
                int idUsuario = _usuarioSeleccionado.IdUsuario;
                string nombreCompleto = txtNombreCompleto.Text;
                string nombreUsuario = txtNombreUsuario.Text;
                int idRol = (int)(cmbRol.SelectedValue ?? 0);
                bool activo = chkActivo.Checked;
                string rolDelEditado = _usuarioSeleccionado.NombreRol;

                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarEmpleadoAsync(idUsuario, nombreCompleto, nombreUsuario, idRol, activo, rolDelEditado);

                MessageBox.Show("¡Empleado actualizado!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
                SeleccionarEmpleadoEnGrid(idUsuario);
            }
            catch (ArgumentException aex) { MessageBox.Show(aex.Message, TituloDatoInvalido, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (InvalidOperationException ioex) { MessageBox.Show(ioex.Message, TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error actualizar empleado: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _usuarioSeleccionado == null) return;

            var confirm = MessageBox.Show($"¿Está seguro que desea eliminar a '{_usuarioSeleccionado.NombreUsuario}'?\n\nEsta acción es irreversible.",
                                          "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.No) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                await _controller.EliminarEmpleadoAsync(_usuarioSeleccionado.IdUsuario, _usuarioSeleccionado.NombreRol);

                MessageBox.Show("¡Empleado eliminado correctamente!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
            }
            catch (InvalidOperationException ioex)
            {
                MessageBox.Show(ioex.Message, TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al eliminar empleado: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private async void BtnResetPassword_Click(object sender, EventArgs e)
        {
            if (!_modoEdicion || _usuarioSeleccionado == null)
            {
                MessageBox.Show("Selecciona un empleado para editar primero.", TituloNoSeleccionado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (UserSession.NombreRol == "Gerente" && _usuarioSeleccionado.NombreRol == "Administrador")
            {
                MessageBox.Show("Un Gerente no puede cambiar la contraseña de un Admin.", TituloAccionBloqueada, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string? nuevaPassword = Interaction.InputBox($"Nueva contraseña para {_usuarioSeleccionado.NombreUsuario}:", "Reset Password", "");
            if (string.IsNullOrWhiteSpace(nuevaPassword)) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                await _controller.ResetearPasswordAsync(_usuarioSeleccionado.IdUsuario, nuevaPassword);
                MessageBox.Show("¡Contraseña cambiada!", TituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reset password: {ex.Message}");
                MessageBox.Show(MsgErrorInesperado, TituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
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
            catch {/*log*/ }
        }

        private async Task CargarGridEmpleadosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvEmpleados.DataSource = null;
                var empleados = await _controller.CargarEmpleadosAsync();
                dgvEmpleados.DataSource = empleados;

                if (dgvEmpleados.Columns["IdUsuario"] != null) dgvEmpleados.Columns["IdUsuario"]!.Visible = false;
                if (dgvEmpleados.Columns["IdRol"] != null) dgvEmpleados.Columns["IdRol"]!.Visible = false;
                if (dgvEmpleados.Columns["Activo"] != null) dgvEmpleados.Columns["Activo"]!.Visible = false;
            }
            catch {/*log*/ }
            finally { this.Cursor = Cursors.Default; }
        }

        private void DgvEmpleados_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvEmpleados.CurrentCell = dgvEmpleados.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void SeleccionarEmpleadoEnGrid(int idUsuario)
        {
            foreach (DataGridViewRow row in dgvEmpleados.Rows)
            {
                if (row.DataBoundItem is UsuarioDto usuario && usuario.IdUsuario == idUsuario)
                {
                    row.Selected = true;
                    dgvEmpleados.CurrentCell = row.Cells[0];
                    dgvEmpleados.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }
    }
}