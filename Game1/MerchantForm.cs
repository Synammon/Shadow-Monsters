using ShadowMonsters.Characters;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowEditor
{
    public partial class MerchantForm : Form
    {
        public bool OKPressed { get; internal set; }
        public string Character { get; internal set; }
        public string Backpack { get; internal set; }

        public MerchantForm()
        {
            InitializeComponent();
        }

        public MerchantForm(Merchant c)
        {
            InitializeComponent();

            if (c != null)
            {
                TxtName.Text = c.Name;
                TxtConversation.Text = c.Conversation;
                TxtSprite.Text = c.SpriteName;
                if (c.GiveMonster != null)
                {
                    TxtTeach.Text = c.GiveMonster.Name;
                }

                Point source = new Point(
                    (int)c.Sprite.Position.X / Engine.TileWidth,
                    (int)c.Sprite.Position.Y / Engine.TileHeight);

                TxtSourceTile.Text = source.X + "," + source.Y;

                foreach (var a in c.BattleMonsters)
                {
                    if (a != null)
                    {
                        LBShadowMonsters.Items.Add(a.Name);
                    }
                }

                foreach (var i in c.Backpack.Items)
                {
                    lbBackpack.Items.Add(i.Name + ":" + i.Count);
                }
            }

        }

        private void FormMerchant_Load(object sender, EventArgs e)
        {
            BtnAdd.Click += BtnAdd_Click;
            BtnRemove.Click += BtnRemove_Click;
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (LBShadowMonsters.SelectedIndex >= 0)
            {
                LBShadowMonsters.Items.RemoveAt(LBShadowMonsters.SelectedIndex);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (LBShadowMonsters.Items.Count <= 6 && !string.IsNullOrEmpty(TxtShadowMonster.Text))
            {
                LBShadowMonsters.Items.Add(TxtShadowMonster.Text);
            }
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            lbBackpack.Items.Add(TxtItem.Text + ":" + TxtCount.Text);
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lbBackpack.SelectedIndex >= 0)
            {
                lbBackpack.Items.RemoveAt(lbBackpack.SelectedIndex);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                MessageBox.Show("You must enter a name for the character.");
                return;
            }

            if (string.IsNullOrEmpty(TxtSourceTile.Text))
            {
                MessageBox.Show("You must enter the source tile for the character");
                return;
            }
            if (string.IsNullOrEmpty(TxtSprite.Text))
            {
                MessageBox.Show("YOu must enter the name of the sprite for the character.");
                return;
            }

            if (string.IsNullOrEmpty(TxtConversation.Text))
            {
                MessageBox.Show("YOu mut enter the name of the conversation.");
                return;
            }

            Character = TxtName.Text + "," +
                TxtSprite.Text + "," +
                "WalkDown," +
                TxtConversation.Text + ",0," + 
                TxtSourceTile.Text + ",";

            foreach (var v in LBShadowMonsters.Items)
            {
                Character += v.ToString() + ",";
            }

            for (int i = LBShadowMonsters.Items.Count; i < 6; i++)
                Character += ",";

            Character += TxtTeach.Text + "," +
                TxtSourceTile.Text;

            for (int i = 0; i < lbBackpack.Items.Count; i++)
            {
                if (i > 0)
                {
                    Backpack += ",";
                }
                Backpack += lbBackpack.Items[i];
            }
            OKPressed = true;
            Close();
        }

        private void lbBackpack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
