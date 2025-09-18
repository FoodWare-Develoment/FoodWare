using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Reportes : UserControl
    {
        public UC_Reportes()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Reportes",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
