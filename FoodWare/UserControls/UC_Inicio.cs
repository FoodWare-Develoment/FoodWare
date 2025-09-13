using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.UserControls
{
    public partial class UC_Inicio : UserControl
    {
        public UC_Inicio()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Inicio",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
