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
    public partial class CharacterForm : Form
    {
        public string Character;
        private readonly Character c;

        public bool OKPressed { get; private set; }

        public CharacterForm()
        {
            InitializeComponent();
        }

        public CharacterForm(Character c)
        {
            InitializeComponent();
            this.c = c;
        }

        private void FormCharacter_Load(object sender, EventArgs e)
        {
            BtnAdd.Click += BtnAdd_Click;
            BtnRemove.Click += BtnRemove_Click;

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
            }
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

            Character += TxtTeach.Text;

            OKPressed = true;
            Close();
        }

        private void TxtTeach_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
