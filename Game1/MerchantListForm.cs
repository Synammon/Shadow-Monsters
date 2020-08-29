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
    public partial class MerchantListForm : Form
    {
        private readonly TileMap map;
        private readonly Editor game1;

        public MerchantListForm()
        {
            InitializeComponent();
        }

        public MerchantListForm(TileMap map, Editor game1)
        {
            InitializeComponent();
            this.map = map;
            this.game1 = game1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MerchantForm frm = new MerchantForm();
            frm.ShowDialog();

            if (frm.OKPressed)
            {
                Character c = Merchant.FromString(game1, frm.Character);
                map.CharacterLayer.Characters.Add(c.SourceTile, c);
                LBCharacters.Items.Add(c.Name);

                string[] parts = frm.Backpack.Split(',');

                foreach (var p in parts)
                {
                    string[] item = p.Split(':');
                    ((Merchant)c).Backpack.AddItem(item[0], int.Parse(item[1]));
                }

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

            Merchant c = (Merchant)map.CharacterLayer.GetCharacter(LBCharacters.SelectedItem.ToString());

            MerchantForm formCharacter = new MerchantForm(c);

            formCharacter.TxtName.Enabled = false;
            formCharacter.ShowDialog();

            if (formCharacter.OKPressed)
            {
                Merchant m = Merchant.FromString(game1, formCharacter.Character);
                map.CharacterLayer.Characters[m.SourceTile] = m;

                string[] parts = formCharacter.Backpack.Split(',');

                m.Backpack.Items.Clear();

                foreach (var p in parts)
                {
                    string[] item = p.Split(':');
                    m.Backpack.AddItem(item[0], int.Parse(item[1]));
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBCharacters.SelectedIndex < 0)
            {
                return;
            }

            map.CharacterLayer.Characters.Remove(
                map.CharacterLayer.GetCharacter(
                    LBCharacters.SelectedItem.ToString()).SourceTile);
            LBCharacters.Items.RemoveAt(LBCharacters.SelectedIndex);
        }

        private void FormMerchantList_Load(object sender, EventArgs e)
        {

            foreach (var c in map.CharacterLayer.Characters.Values)
            {
                if (c is Merchant)
                {
                    if (c.Name != null)
                    {
                        LBCharacters.Items.Add(c.Name);
                    }
                    else
                    {
                        LBCharacters.Items.Add("Unknown");
                    }
                }
            }

        }
    }
}
