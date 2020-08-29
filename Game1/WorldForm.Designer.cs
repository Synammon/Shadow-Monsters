namespace ShadowEditor
{
    partial class WorldForm
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
            this.TxtSourceTile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtDestinationTile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtDestinationLevel = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Tile:";
            // 
            // TxtSourceTile
            // 
            this.TxtSourceTile.Location = new System.Drawing.Point(119, 10);
            this.TxtSourceTile.Name = "TxtSourceTile";
            this.TxtSourceTile.Size = new System.Drawing.Size(100, 20);
            this.TxtSourceTile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destination Tile:";
            // 
            // TxtDestinationTile
            // 
            this.TxtDestinationTile.Location = new System.Drawing.Point(119, 36);
            this.TxtDestinationTile.Name = "TxtDestinationTile";
            this.TxtDestinationTile.Size = new System.Drawing.Size(100, 20);
            this.TxtDestinationTile.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Destination Level:";
            // 
            // TxtDestinationLevel
            // 
            this.TxtDestinationLevel.Location = new System.Drawing.Point(119, 62);
            this.TxtDestinationLevel.Name = "TxtDestinationLevel";
            this.TxtDestinationLevel.Size = new System.Drawing.Size(100, 20);
            this.TxtDestinationLevel.TabIndex = 6;
            // 
            // btnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(241, 8);
            this.BtnOK.Name = "btnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 7;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(241, 37);
            this.BtnCancel.Name = "btnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 8;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // WorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 100);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TxtDestinationLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtDestinationTile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtSourceTile);
            this.Controls.Add(this.label1);
            this.Name = "World Form";
            this.Text = "World Form";
            this.Load += new System.EventHandler(this.WorldForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtSourceTile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtDestinationTile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtDestinationLevel;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
    }
}