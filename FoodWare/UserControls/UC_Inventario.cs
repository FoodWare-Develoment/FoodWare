using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.UserControls
{
    public partial class UC_Inventario : UserControl
    {
        public UC_Inventario()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Inventario",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
