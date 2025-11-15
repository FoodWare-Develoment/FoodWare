using System;
using System.Windows.Forms;
using FoodWare.Controller;
using FoodWare.Controller.Logic;
using FoodWare.View.Helpers;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;

namespace FoodWare.View.Forms
{
    public partial class LoginForm : Form
    {
        private readonly LoginController _loginCtrl;

        public LoginForm()
        {
            InitializeComponent();
            AplicarEstilosDeLogin();

            IUsuarioRepository repositorio = new UsuarioSqlRepository();
            _loginCtrl = new LoginController(repositorio);
        }

        private void AplicarEstilosDeLogin()
        {
            this.BackColor = EstilosApp.ColorMenu;
            EstilosApp.EstiloTextBoxLogin(this.txtUsuario);
            EstilosApp.EstiloTextBoxLogin(this.txtPassword);
            EstilosApp.EstiloBotonAccionPrincipal(this.btnIngresar);
            EstilosApp.EstiloLabelError(this.lblMensajeError);
        }

        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = "     * " + mensaje;
            lblMensajeError.Visible = true;
        }

        // --- EVENT HANDLERS ---

        private async void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.btnIngresar.Enabled = false;

                // 2. Llamamos al controlador, que ahora devuelve solo el resultado
                var resultado = await _loginCtrl.ValidarLoginAsync(this.txtUsuario.Text, this.txtPassword.Text);

                // 3. Reaccionamos
                switch (resultado)
                {
                    case LoginResult.Exitoso:
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;

                    case LoginResult.CredencialesInvalidas:
                        MostrarError("Usuario o contraseña incorrectos.");
                        // 1. Desconectamos el evento temporalmente
                        this.txtPassword.TextChanged -= TxtPassword_TextChanged;

                        // 2. Limpiamos el campo (ahora no disparará el evento)
                        txtPassword.Clear();

                        // 3. Reconectamos el evento
                        this.txtPassword.TextChanged += TxtPassword_TextChanged;
                        txtPassword.Focus();
                        break;

                    case LoginResult.ErrorDeBaseDeDatos:
                        MostrarError("Error al conectar con el servidor. Intente más tarde.");
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error inesperado: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.btnIngresar.Enabled = true;
            }
        }

        private void TxtUsuario_TextChanged(object? sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }

        private void TxtPassword_TextChanged(object? sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }
    }
}