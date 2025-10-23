using System;
using System.Windows.Forms;
using FoodWare.Controller;
using FoodWare.View.Helpers;
using FoodWare.Model.Interfaces; 
using FoodWare.Model.DataAccess;

namespace FoodWare.View.Forms
{
    /// <summary>
    /// Formulario de Inicio de Sesión. Valida al usuario y, si tiene éxito,
    /// devuelve DialogResult.OK para que Program.cs inicie el FormMain.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly LoginController _loginCtrl; // <-- Renombrado para claridad

        /// <summary>
        /// Inicializa el formulario de Login.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            AplicarEstilosDeLogin();

            // 1. Creamos la instancia del repositorio real
            IUsuarioRepository repositorio = new UsuarioSqlRepository();
            // 2. Se lo "inyectamos" al controlador
            _loginCtrl = new LoginController(repositorio);
        }

        /// <summary>
        /// Método de ayuda para aplicar los estilos de EstilosApp a este formulario.
        /// </summary>
        private void AplicarEstilosDeLogin()
        {
            // 1. Estilo de la Ventana
            this.BackColor = EstilosApp.ColorMenu;

            // 2. Campos de Entrada (TextBox)
            EstilosApp.EstiloTextBoxLogin(this.txtUsuario);
            EstilosApp.EstiloTextBoxLogin(this.txtPassword);

            // 3. Botón Principal (INGRESAR)
            EstilosApp.EstiloBotonAccionPrincipal(this.btnIngresar);

            // 4. Label de Mensajes de Error
            EstilosApp.EstiloLabelError(this.lblMensajeError);
        }

        /// <summary>
        /// Método de ayuda para mostrar mensajes de error usando la etiqueta estilizada.
        /// </summary>
        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = "     * " + mensaje; // Muestra el mensaje de error (con prefijo)
            lblMensajeError.Visible = true;
        }

        // --- LÓGICA DE VALIDACIÓN Y EVENTOS ---

        /// <summary>
        /// Evento principal del botón Ingresar. Ahora es 'async void'
        /// para permitir la validación asíncrona.
        /// </summary>
        private async void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Cambiamos el cursor y deshabilitamos el botón
                this.Cursor = Cursors.WaitCursor;
                this.btnIngresar.Enabled = false;

                // 2. La Vista recoge los datos y llama al Controlador asíncronamente
                LoginResult resultado = await _loginCtrl.ValidarLoginAsync(this.txtUsuario.Text, this.txtPassword.Text);

                // 3. La Vista reacciona a la respuesta (ya estamos en el hilo UI)
                switch (resultado)
                {
                    case LoginResult.Exitoso:
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break; // No es necesario, pero es buena práctica

                    case LoginResult.CredencialesInvalidas:
                        MostrarError("Usuario o contraseña incorrectos.");
                        txtPassword.Clear();
                        txtPassword.Focus();
                        break;

                    case LoginResult.ErrorDeBaseDeDatos:
                        MostrarError("Error al conectar con el servidor. Intente más tarde.");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Captura genérica por si algo más falla
                MostrarError("Error inesperado: " + ex.Message);
            }
            finally
            {
                // 4. Pase lo que pase, restauramos la UI
                this.Cursor = Cursors.Default;
                this.btnIngresar.Enabled = true;
            }
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario 
        /// intenta corregir el nombre de usuario.
        /// </summary>
        private void TxtUsuario_TextChanged(object? sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario
        /// intenta corregir la contraseña.
        /// </summary>
        private void TxtPassword_TextChanged(object? sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }
    }
}