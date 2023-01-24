namespace VMClock
{
    partial class szivatasForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(szivatasForm));
            this.button1 = new System.Windows.Forms.Button();
            this.lblSzivatottTanuloNeve = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(-3, -4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(391, 525);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblSzivatottTanuloNeve
            // 
            this.lblSzivatottTanuloNeve.BackColor = System.Drawing.Color.Transparent;
            this.lblSzivatottTanuloNeve.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblSzivatottTanuloNeve.ForeColor = System.Drawing.Color.Maroon;
            this.lblSzivatottTanuloNeve.Location = new System.Drawing.Point(62, 142);
            this.lblSzivatottTanuloNeve.Name = "lblSzivatottTanuloNeve";
            this.lblSzivatottTanuloNeve.Size = new System.Drawing.Size(283, 151);
            this.lblSzivatottTanuloNeve.TabIndex = 4;
            this.lblSzivatottTanuloNeve.Text = "{tanuloNeve}";
            this.lblSzivatottTanuloNeve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // szivatasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(388, 520);
            this.Controls.Add(this.lblSzivatottTanuloNeve);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "szivatasForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dr. Evil says...";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblSzivatottTanuloNeve;
    }
}