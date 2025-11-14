using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;    // Añadir
using FoodWare.Controller.Logic; // Añadir
using FoodWare.Model.DataAccess; // Añadir
using FoodWare.Model.Interfaces; // Añadir
using FoodWare.View.Helpers;     // Añadir
using FoodWare.Model.Entities;   // Añadir

namespace FoodWare.View.UserControls
{
    public partial class UC_Empleados : UserControl
    {
        private readonly EmpleadosController _controller;
        private UsuarioDto? _usuarioSeleccionado;

        public UC_Empleados()
        {
            InitializeComponent();

            // 1. Inyectar repositorios reales al controlador
            _controller = new EmpleadosController(
                new UsuarioSqlRepository(),
                new RolSqlRepository()
            );

            // 2. Aplicar Estilos (Necesitarás crear estos controles en el diseñador)
            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            this.BackColor = EstilosApp.ColorFondo;

            // Estilo al nuevo TableLayoutPanel
            EstilosApp.EstiloPanel(tlpInputs, EstilosApp.ColorFondo);

            // Labels
            EstilosApp.EstiloLabelModulo(lblNombreCompleto);
            EstilosApp.EstiloLabelModulo(lblNombreUsuario);
            EstilosApp.EstiloLabelModulo(lblPassword);
            EstilosApp.EstiloLabelModulo(lblRol);

            // Controles
            EstilosApp.EstiloTextBoxModulo(txtNombreCompleto);
            EstilosApp.EstiloTextBoxModulo(txtNombreUsuario);
            EstilosApp.EstiloTextBoxModulo(txtPassword);
            EstilosApp.EstiloComboBoxModulo(cmbRol);
            // (chkActivo no necesita estilo especial)

            // Botones
            EstilosApp.EstiloBotonModulo(btnGuardar);
            EstilosApp.EstiloBotonModuloSecundario(btnActualizar);
            EstilosApp.EstiloBotonModuloAlerta(btnDesactivar); // El botón de "Eliminar"
            EstilosApp.EstiloBotonModuloSecundario(btnLimpiar);

            // Grid
            EstilosApp.EstiloDataGridView(dgvEmpleados);
        }

        private async void UC_Empleados_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // Cargamos el ComboBox de Roles
            await CargarRolesDropdownAsync();
            // Cargamos el grid principal
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
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Error al cargar empleados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // La contraseña solo es obligatoria al crear
            txtPassword.Enabled = true;
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
                // 1. Recoger datos
                string nombreCompleto = txtNombreCompleto.Text;
                string nombreUsuario = txtNombreUsuario.Text;
                string password = txtPassword.Text;
                int idRol = (int)(cmbRol.SelectedValue ?? 0);
                bool activo = chkActivo.Checked;

                // 2. Llamar al controlador
                this.Cursor = Cursors.WaitCursor;
                await _controller.GuardarNuevoEmpleadoAsync(nombreCompleto, nombreUsuario, password, idRol, activo);

                // 3. Actualizar UI
                MessageBox.Show("¡Empleado guardado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
            }
            catch (ArgumentException aex) // Errores de validación
            {
                MessageBox.Show(aex.Message, "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Errores inesperados
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Por favor, selecciona un empleado de la lista para actualizar.", "Ningún empleado seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // 2. Llamar al controlador
                this.Cursor = Cursors.WaitCursor;
                await _controller.ActualizarEmpleadoAsync(idUsuario, nombreCompleto, nombreUsuario, idRol, activo);

                // 3. Actualizar UI
                MessageBox.Show("¡Empleado actualizado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                int idActualizado = idUsuario; // Guardamos el ID
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
                // (Opcional) Re-seleccionar la fila
            }
            catch (ArgumentException aex) // Errores de validación
            {
                MessageBox.Show(aex.Message, "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Errores inesperados
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnDesactivar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado de la lista para desactivar.", "Ningún empleado seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"¿Seguro que deseas desactivar al usuario '{_usuarioSeleccionado.NombreUsuario}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                await _controller.DesactivarEmpleadoAsync(_usuarioSeleccionado.IdUsuario);

                MessageBox.Show("¡Empleado desactivado!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                await CargarGridEmpleadosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error al desactivar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // --- Evento del Menú Contextual ---

        private void ItemEditar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow != null && dgvEmpleados.CurrentRow.DataBoundItem is UsuarioDto usuario)
            {
                // 1. Guardar el DTO seleccionado
                _usuarioSeleccionado = usuario;

                // 2. Rellenar el formulario
                txtNombreCompleto.Text = usuario.NombreCompleto;
                txtNombreUsuario.Text = usuario.NombreUsuario;
                cmbRol.SelectedValue = usuario.IdRol;
                chkActivo.Checked = usuario.Activo;

                // 3. Lógica de UI: No se puede cambiar la contraseña desde aquí.
                txtPassword.Clear();
                txtPassword.Enabled = false;
                lblPassword.Text = "(Contraseña no se modifica)";
            }
        }

        // (Asegúrate de conectar este evento al ContextMenuStrip en el diseñador)
        private void DgvEmpleados_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvEmpleados.CurrentCell = dgvEmpleados.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }
    }
}