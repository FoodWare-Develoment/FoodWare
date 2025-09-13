using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWare.UserControls
{
    public partial class UC_Finanzas : UserControl
    {
        public UC_Finanzas()
        {
            InitializeComponent();

            var lbl = new Label
            {
                Text = "Finanzas",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            Controls.Add(lbl);
        }
    }
}
