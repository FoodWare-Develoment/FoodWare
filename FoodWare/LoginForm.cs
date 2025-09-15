using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodWare
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
            // TODO: Reemplazar esta simulación con la lógica real de validación contra la BD.
            bool loginValido = (txtUsuario.Text == "admin" && txtPassword.Text == "123");

            if (loginValido)
            {
                // ÉXITO: Informa al Program.cs que el resultado es OK y cierra.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // FRACASO (Orden Lógico Corregido para evitar el bug de TextChanged):
                // Limpia la contraseña (disparando TextChanged) y enfoca ANTES de mostrar el error.
                txtPassword.Text = "";
                txtPassword.Focus();
                MostrarError("Usuario o contraseña incorrectos.");
            }
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario 
        /// intenta corregir el nombre de usuario.
        /// </summary>
        private void TxtUsuario_TextChanged(object sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }

        /// <summary>
        /// (Buena práctica UX): Oculta el mensaje de error tan pronto como el usuario
        /// intenta corregir la contraseña.
        /// </summary>
        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            lblMensajeError.Visible = false;
        }
    }
}