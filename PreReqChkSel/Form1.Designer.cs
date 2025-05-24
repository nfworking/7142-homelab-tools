namespace PreReqChkSel
{
    partial class MainApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            moeBut = new Button();
            mobBut = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // moeBut
            // 
            moeBut.Font = new Font("Segoe UI", 14F);
            moeBut.Location = new Point(12, 261);
            moeBut.Name = "moeBut";
            moeBut.Size = new Size(111, 34);
            moeBut.TabIndex = 2;
            moeBut.Text = "MOE";
            moeBut.UseVisualStyleBackColor = true;
            moeBut.Click += moeBut_Click;
            // 
            // mobBut
            // 
            mobBut.Font = new Font("Segoe UI", 14F);
            mobBut.Location = new Point(149, 261);
            mobBut.Name = "mobBut";
            mobBut.Size = new Size(164, 34);
            mobBut.TabIndex = 3;
            mobBut.Text = "Portable Device";
            mobBut.UseVisualStyleBackColor = true;
            mobBut.Click += MobBut_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label1.Location = new Point(5, 107);
            label1.Name = "label1";
            label1.Size = new Size(308, 28);
            label1.TabIndex = 4;
            label1.Text = "Corp Device Joiner, caution use";
         
            // 
            // MainApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(325, 327);
            Controls.Add(label1);
            Controls.Add(mobBut);
            Controls.Add(moeBut);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainApp";
            Text = "Deployment Selector";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox installChromeB;
        private CheckBox rdpEN1;
        private Button moeBut;
        private Button mobBut;
        private Label label1;
    }
}
