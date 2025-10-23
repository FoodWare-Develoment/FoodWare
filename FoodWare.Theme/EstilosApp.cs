using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.Helpers
{
    /// <summary>
    /// Clase estática de utilidad para centralizar la paleta de colores y los estilos 
    /// de los controles de Windows Forms para toda la aplicación FoodWare.
    /// </summary>
    public static class EstilosApp
    {
        // --- DECLARAR LA CONSTANTE AQUÍ ---
        private const string FuentePrincipal = "Segoe UI";

        // --- PALETA DE COLORES PRINCIPAL ---
        public static readonly Color ColorMenu = ColorTranslator.FromHtml("#2C3E50");
        public static readonly Color ColorBarra = ColorTranslator.FromHtml("#34495E");
        public static readonly Color ColorFondo = ColorTranslator.FromHtml("#ECF0F1");
        public static readonly Color ColorTextoOscuro = ColorTranslator.FromHtml("#2C3E50");
        public static readonly Color ColorActivo = ColorTranslator.FromHtml("#1ABC9C");
        public static readonly Color ColorAccion = ColorTranslator.FromHtml("#27AE60");
        public static readonly Color ColorAlerta = ColorTranslator.FromHtml("#E74C3C");
        public static readonly Color ColorSubmenuBG = Color.FromArgb(0, 100, 100);

        // --- MÉTODOS DE ESTILO (MENÚ PRINCIPAL) ---

        /// <summary>
        /// Aplica un color de fondo estándar a un control Panel.
        /// </summary>
        public static void EstiloPanel(Panel panel, Color color)
        {
            panel.BackColor = color;
        }

        /// <summary>
        /// Aplica un estilo visual estandarizado a un botón de Menú Principal (Nivel 1).
        /// </summary>

        public static void EstiloBotonMenu(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // Requerido para FlatStyle para quitar el borde
            btn.ForeColor = Color.White;
            btn.BackColor = ColorMenu;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.Height = 40;
            btn.Dock = DockStyle.Top;
        }

        /// <summary>
        /// Aplica un estilo visual estandarizado a un botón de Submenú (Nivel 2).
        /// </summary>

        public static void EstiloBotonSubmenu(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = ColorSubmenuBG;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0); // Mayor indentación para Nivel 2
            btn.Height = 35;
            btn.Dock = DockStyle.Top;
        }

        // --- NUEVOS MÉTODOS DE ESTILO (LOGIN FORM) ---

        /// <summary>
        /// Aplica el estilo de acción principal (color Activo) a un botón, 
        /// usado para el botón "INGRESAR" del Login. (Spec: #1ABC9C)
        /// </summary>
        public static void EstiloBotonAccionPrincipal(Button btn)
        {
            btn.BackColor = ColorActivo;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }

        /// <summary>
        /// Aplica el estilo estándar para campos de texto de Login (Usuario/Contraseña).
        /// </summary>
        public static void EstiloTextBoxLogin(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTextoOscuro; // Spec: #2C3E50
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Aplica el estilo estándar para las etiquetas de mensaje de error.
        /// Las oculta por defecto y asigna el color de Alerta.
        /// </summary>
        public static void EstiloLabelError(Label lbl)
        {
            lbl.ForeColor = ColorAlerta; // Spec: #E74C3C
            lbl.BackColor = Color.Transparent;
            lbl.Visible = false; // Oculto por defecto
        }

        // --- MÉTODOS DE ESTILO (CONTROLES DE MÓDULO) ---

        /// <summary>
        /// Aplica un estilo visual estándar a un DataGridView.
        /// </summary>
        public static void EstiloDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = ColorFondo;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;

            // Estilo del Header
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorBarra;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(FuentePrincipal, 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 30;

            // Estilo de las Filas
            dgv.DefaultCellStyle.BackColor = ColorFondo;
            dgv.DefaultCellStyle.ForeColor = ColorTextoOscuro;
            dgv.DefaultCellStyle.Font = new Font(FuentePrincipal, 9);
            dgv.DefaultCellStyle.SelectionBackColor = ColorActivo;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 25;

            // Comportamiento
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Positiva" (Guardar, Crear).
        /// </summary>
        public static void EstiloBotonModulo(Button btn)
        {
            btn.BackColor = ColorAccion; // Verde
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(FuentePrincipal, 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Peligro" (Eliminar).
        /// </summary>
        public static void EstiloBotonModuloAlerta(Button btn)
        {
            btn.BackColor = ColorAlerta; // Rojo
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(FuentePrincipal, 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a un botón de acción "Secundaria" (Limpiar, Cancelar).
        /// </summary>
        public static void EstiloBotonModuloSecundario(Button btn)
        {
            btn.BackColor = Color.White;
            btn.ForeColor = ColorTextoOscuro; // Color gris/azul
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorTextoOscuro;
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new Font(FuentePrincipal, 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a una etiqueta de formulario.
        /// </summary>
        public static void EstiloLabelTitulo(Label lbl)
        {
            lbl.ForeColor = ColorTextoOscuro;
            lbl.BackColor = Color.Transparent;
            lbl.Font = new Font(FuentePrincipal, 13, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a una etiqueta de formulario.
        /// </summary>
        public static void EstiloLabelModulo(Label lbl)
        {
            lbl.ForeColor = ColorTextoOscuro;
            lbl.BackColor = Color.Transparent;
            lbl.Font = new Font(FuentePrincipal, 9, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica un estilo estándar a una caja de texto de formulario.
        /// </summary>
        public static void EstiloTextBoxModulo(TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTextoOscuro;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font(FuentePrincipal, 9);
        }

        /// <summary>
        /// Aplica un estilo estándar a un ComboBox de formulario.
        /// </summary>
        public static void EstiloComboBoxModulo(ComboBox cmb)
        {
            cmb.BackColor = Color.White;
            cmb.ForeColor = ColorTextoOscuro;
            cmb.FlatStyle = FlatStyle.Flat; // Un borde más moderno
            cmb.Font = new Font(FuentePrincipal, 9);
        }
    }
}