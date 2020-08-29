namespace ShadowEditor
{
    partial class CharacterForm
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
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.TxtConversation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtSprite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtTeach = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LBShadowMonsters = new System.Windows.Forms.ListBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.TxtShadowMonster = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtSourceTile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(570, 24);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(138, 42);
            this.BtnOK.TabIndex = 15;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.Location = new System.Drawing.Point(570, 78);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(138, 42);
            this.BtnCancel.TabIndex = 16;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Conversation:";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(233, 28);
            this.TxtName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(312, 29);
            this.TxtName.TabIndex = 1;
            // 
            // TxtConversation
            // 
            this.TxtConversation.Location = new System.Drawing.Point(233, 133);
            this.TxtConversation.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtConversation.Name = "TxtConversation";
            this.TxtConversation.Size = new System.Drawing.Size(312, 29);
            this.TxtConversation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 186);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sprite:";
            // 
            // TxtSprite
            // 
            this.TxtSprite.Location = new System.Drawing.Point(233, 181);
            this.TxtSprite.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtSprite.Name = "TxtSprite";
            this.TxtSprite.Size = new System.Drawing.Size(312, 29);
            this.TxtSprite.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 234);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Teaching Monster:";
            // 
            // TxtTeach
            // 
            this.TxtTeach.Location = new System.Drawing.Point(233, 229);
            this.TxtTeach.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtTeach.Name = "TxtTeach";
            this.TxtTeach.Size = new System.Drawing.Size(312, 29);
            this.TxtTeach.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(83, 277);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "Battle Monsters:";
            // 
            // LBShadowMonsters
            // 
            this.LBShadowMonsters.FormattingEnabled = true;
            this.LBShadowMonsters.ItemHeight = 24;
            this.LBShadowMonsters.Location = new System.Drawing.Point(233, 277);
            this.LBShadowMonsters.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.LBShadowMonsters.Name = "LBShadowMonsters";
            this.LBShadowMonsters.Size = new System.Drawing.Size(312, 244);
            this.LBShadowMonsters.TabIndex = 11;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(249, 583);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(138, 42);
            this.BtnAdd.TabIndex = 13;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(400, 585);
            this.BtnRemove.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(138, 42);
            this.BtnRemove.TabIndex = 14;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = true;
            // 
            // TxtShadowMonster
            // 
            this.TxtShadowMonster.Location = new System.Drawing.Point(233, 535);
            this.TxtShadowMonster.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtShadowMonster.Name = "TxtShadowMonster";
            this.TxtShadowMonster.Size = new System.Drawing.Size(312, 29);
            this.TxtShadowMonster.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 87);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 25);
            this.label6.TabIndex = 2;
            this.label6.Text = "Source Tile:";
            // 
            // TxtSourceTile
            // 
            this.TxtSourceTile.Location = new System.Drawing.Point(233, 81);
            this.TxtSourceTile.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TxtSourceTile.Name = "TxtSourceTile";
            this.TxtSourceTile.Size = new System.Drawing.Size(301, 29);
            this.TxtSourceTile.TabIndex = 3;
            // 
            // CharacterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 645);
            this.Controls.Add(this.TxtSourceTile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TxtShadowMonster);
            this.Controls.Add(this.BtnRemove);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.LBShadowMonsters);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxtTeach);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtSprite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtConversation);
            this.Controls.Add(this.TxtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CharacterForm";
            this.Text = "FormCharacter";
            this.Load += new System.EventHandler(this.FormCharacter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.TextBox TxtConversation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtSprite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtTeach;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LBShadowMonsters;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.TextBox TxtShadowMonster;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtSourceTile;
    }
}