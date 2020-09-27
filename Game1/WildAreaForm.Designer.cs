namespace ShadowEditor
{
    partial class WildAreaForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTopLeft = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtBottomRight = new System.Windows.Forms.TextBox();
            this.BtnRight = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this.LstAvailable = new System.Windows.Forms.ListBox();
            this.LstSelected = new System.Windows.Forms.ListBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NudLevel = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NudLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Top left:";
            // 
            // TxtTopLeft
            // 
            this.TxtTopLeft.Location = new System.Drawing.Point(95, 9);
            this.TxtTopLeft.Name = "TxtTopLeft";
            this.TxtTopLeft.Size = new System.Drawing.Size(100, 29);
            this.TxtTopLeft.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bottom right:";
            // 
            // TxtBottomRight
            // 
            this.TxtBottomRight.Location = new System.Drawing.Point(336, 6);
            this.TxtBottomRight.Name = "TxtBottomRight";
            this.TxtBottomRight.Size = new System.Drawing.Size(100, 29);
            this.TxtBottomRight.TabIndex = 3;
            // 
            // BtnRight
            // 
            this.BtnRight.Location = new System.Drawing.Point(186, 175);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(75, 39);
            this.BtnRight.TabIndex = 4;
            this.BtnRight.Text = ">>";
            this.BtnRight.UseVisualStyleBackColor = true;
            // 
            // BtnLeft
            // 
            this.BtnLeft.Location = new System.Drawing.Point(186, 220);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(75, 39);
            this.BtnLeft.TabIndex = 5;
            this.BtnLeft.Text = "<<";
            this.BtnLeft.UseVisualStyleBackColor = true;
            // 
            // LstAvailable
            // 
            this.LstAvailable.FormattingEnabled = true;
            this.LstAvailable.ItemHeight = 24;
            this.LstAvailable.Location = new System.Drawing.Point(21, 87);
            this.LstAvailable.Name = "LstAvailable";
            this.LstAvailable.Size = new System.Drawing.Size(147, 340);
            this.LstAvailable.TabIndex = 6;
            // 
            // LstSelected
            // 
            this.LstSelected.FormattingEnabled = true;
            this.LstSelected.ItemHeight = 24;
            this.LstSelected.Location = new System.Drawing.Point(300, 87);
            this.LstSelected.Name = "LstSelected";
            this.LstSelected.Size = new System.Drawing.Size(136, 340);
            this.LstSelected.TabIndex = 7;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(482, 13);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(99, 39);
            this.BtnOK.TabIndex = 8;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(482, 70);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(99, 39);
            this.BtnCancel.TabIndex = 9;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Available";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Selected";
            // 
            // NudLevel
            // 
            this.NudLevel.Location = new System.Drawing.Point(174, 124);
            this.NudLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NudLevel.Name = "NudLevel";
            this.NudLevel.Size = new System.Drawing.Size(120, 29);
            this.NudLevel.TabIndex = 12;
            this.NudLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // WildAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 450);
            this.Controls.Add(this.NudLevel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.LstSelected);
            this.Controls.Add(this.LstAvailable);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.TxtBottomRight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtTopLeft);
            this.Controls.Add(this.label1);
            this.Name = "WildAreaForm";
            this.Text = "WildAreaForm";
            ((System.ComponentModel.ISupportInitialize)(this.NudLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtTopLeft;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtBottomRight;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.ListBox LstAvailable;
        private System.Windows.Forms.ListBox LstSelected;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NudLevel;
    }
}