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
using Microsoft.Xna.Framework;

namespace ShadowEditor
{
    public partial class CharacterListForm : Form
    {
        private readonly TileMap map;
        private readonly Editor game;

        public CharacterListForm(TileMap map, Editor game)
        {
            InitializeComponent();

            this.map = map;
            this.game = game;

            foreach (var c in map.CharacterLayer.Characters.Keys)
            {
                if (!(map.CharacterLayer.Characters[c] is Merchant))
                {
                    if (map.CharacterLayer.Characters[c].Name != null)
                    {
                        LBCharacters.Items.Add(map.CharacterLayer.Characters[c].Name);
                    }
                    else
                    {
                        LBCharacters.Items.Add("Unknown");
                    }
                }
            }

            BtnAdd.Click += BtnAdd_Click;
            BtnEdit.Click += BtnEdit_Click;
            BtnDelete.Click += BtnDelete_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            CharacterForm character = new CharacterForm();
            character.ShowDialog();

            if (character.OKPressed)
            {
                Character c = Character.FromString(game, character.Character);
                string[] parts = character.Character.Split(',');

                map.CharacterLayer.Characters.Add(
                    c.SourceTile,
                    c);
                LBCharacters.Items.Add(c.Name);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (LBCharacters.SelectedIndex < 0 || LBCharacters.Items.Count == 0)
            {
                return;
            }

            if (LBCharacters.SelectedItem.ToString() == "Unknown")
            {
                return;
            }

            Character c = map.CharacterLayer.GetCharacter(LBCharacters.SelectedItem.ToString());

            CharacterForm formCharacter = new CharacterForm(c);

            formCharacter.TxtName.Enabled = false;
            formCharacter.ShowDialog();

            if (formCharacter.OKPressed)
            {
                string[] parts = formCharacter.Character.Split(',');
                map.CharacterLayer.Characters[c.SourceTile] =
                    Character.FromString(game, formCharacter.Character);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBCharacters.SelectedIndex < 0)
            {
                return;
            }

            map.CharacterLayer.Characters.Remove(
                map.CharacterLayer.GetCharacter(LBCharacters.SelectedItem.ToString()).SourceTile);
            LBCharacters.Items.RemoveAt(LBCharacters.SelectedIndex);
        }
    }
}
