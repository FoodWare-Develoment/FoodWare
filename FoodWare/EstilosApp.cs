using System.Drawing;
using System.Windows.Forms;

public static class EstilosApp
{
    public static Color ColorMenu = ColorTranslator.FromHtml("#2C3E50");
    public static Color ColorBarra = ColorTranslator.FromHtml("#34495E");
    public static Color ColorFondo = ColorTranslator.FromHtml("#ECF0F1");
    public static Color ColorTextoOscuro = ColorTranslator.FromHtml("#2C3E50");
    public static Color ColorActivo = ColorTranslator.FromHtml("#1ABC9C");
    public static Color ColorAccion = ColorTranslator.FromHtml("#27AE60");
    public static Color ColorAlerta = ColorTranslator.FromHtml("#E74C3C");

    public static void EstiloPanel(Panel panel, Color color)
    {
        panel.BackColor = color;
    }

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

    public static void EstiloBotonSubmenu(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.ForeColor = Color.White;
        btn.BackColor = Color.FromArgb(0, 100, 100);
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.Padding = new Padding(30, 0, 0, 0);
        btn.Height = 35;
        btn.Dock = DockStyle.Top;
    }
}