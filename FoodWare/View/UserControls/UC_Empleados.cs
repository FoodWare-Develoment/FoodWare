using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.View.UserControls
{
    public partial class UC_Empleados : UserControl
    {
        public UC_Empleados()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Empleados",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
