using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.UserControls
{
    public partial class UC_Ventas : UserControl
    {
        public UC_Ventas()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Ventas",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
