namespace FoodWare.UserControls
{
    partial class UC_Inicio
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBoxBienvenida = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBienvenida).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxBienvenida
            // 
            pictureBoxBienvenida.Dock = DockStyle.Fill;
            pictureBoxBienvenida.Image = Properties.Resources.InicioBienvenida;
            pictureBoxBienvenida.Location = new Point(0, 0);
            pictureBoxBienvenida.Name = "pictureBoxBienvenida";
            pictureBoxBienvenida.Size = new Size(800, 450);
            pictureBoxBienvenida.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBienvenida.TabIndex = 0;
            pictureBoxBienvenida.TabStop = false;
            // 
            // UC_Inicio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBoxBienvenida);
            Name = "UC_Inicio";
            Size = new Size(800, 450);
            ((System.ComponentModel.ISupportInitialize)pictureBoxBienvenida).EndInit();
            ResumeLayout(false);
        }
        private PictureBox pictureBoxBienvenida;
    }
}
