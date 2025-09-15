using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Clase estática de utilidad para centralizar la paleta de colores y los estilos 
/// de los controles de Windows Forms para toda la aplicación FoodWare.
/// </summary>
public static class EstilosApp
{
    // --- PALETA DE COLORES PRINCIPAL ---
    public static Color ColorMenu = ColorTranslator.FromHtml("#2C3E50");
    public static Color ColorBarra = ColorTranslator.FromHtml("#34495E");
    public static Color ColorFondo = ColorTranslator.FromHtml("#ECF0F1");
    public static Color ColorTextoOscuro = ColorTranslator.FromHtml("#2C3E50");
    public static Color ColorActivo = ColorTranslator.FromHtml("#1ABC9C");
    public static Color ColorAccion = ColorTranslator.FromHtml("#27AE60");
    public static Color ColorAlerta = ColorTranslator.FromHtml("#E74C3C");
    public static Color ColorSubmenuBG = Color.FromArgb(0, 100, 100);

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
        btn.FlatAppearance.BorderSize = 0;
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
        btn.Padding = new Padding(30, 0, 0, 0);
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
}