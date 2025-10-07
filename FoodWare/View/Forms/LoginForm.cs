using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoodWare.Controller;
using FoodWare.Controller.Logic;
using FoodWare.View.Helpers;

namespace FoodWare.View.Forms
{
    /// <summary>
    /// Formulario de Inicio de Sesión. Valida al usuario y, si tiene éxito,
    /// devuelve DialogResult.OK para que Program.cs inicie el FormMain.
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Inicializa el formulario de Login.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            AplicarEstilosDeLogin();
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
        /// Evento principal del botón Ingresar. Valida las credenciales.
        /// </summary>
        private void BtnIngresar_Click(object sender, EventArgs e)
        {

            // 1. La Vista crea una instancia del Controlador.
            LoginController loginCtrl = new();

            // 2. La Vista recoge los datos de los campos de texto y se los pasa al Controlador.
            LoginResult resultado = loginCtrl.ValidarLogin(this.txtUsuario.Text, this.txtPassword.Text);

            // 3. La Vista reacciona a la respuesta del Controlador usando un 'switch'
            // para manejar de forma clara y ordenada cada posible resultado.
            switch (resultado)
            {
                case LoginResult.Exitoso:
                    // Caso de éxito: Cerramos el login y devolvemos 'OK' para que el programa principal continúe.
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;

                case LoginResult.CredencialesInvalidas:
                    MostrarError("Usuario o contraseña incorrectos.");

                    // 1. Desactivamos temporalmente el evento para que no se dispare al limpiar el texto.
                    this.txtPassword.TextChanged -= TxtPassword_TextChanged;

                    // 2. Ahora sí, borramos el texto sin el efecto secundario de ocultar el mensaje.
                    txtPassword.Clear();

                    // 3. Reactivamos el evento para que el mensaje se oculte si el usuario empieza a escribir de nuevo.
                    this.txtPassword.TextChanged += TxtPassword_TextChanged;
                    
                    txtPassword.Focus();
                    break;

                case LoginResult.ErrorDeBaseDeDatos:
                    // Caso de error de sistema: Informamos al usuario que hay un problema técnico, sin dar detalles.
                    MostrarError("Error al conectar con el servidor. Intente más tarde.");
                    break;
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