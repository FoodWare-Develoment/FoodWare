using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.UserControls
{
    public partial class UC_Configuracion : UserControl
    {
        public UC_Configuracion()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Configuracion",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
