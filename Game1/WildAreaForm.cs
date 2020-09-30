using ShadowMonsters.ShadowMonsters;
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
    public partial class WildAreaForm : Form
    {
        public bool OkPressed { get; set; }
        public WildArea WildArea { get; set; }

        public WildAreaForm()
        {
            InitializeComponent();

            foreach (var m in ShadowMonsterManager.ShadowMonsterList.Keys)
            {
                LstAvailable.Items.Add(m);
            }

            BtnRight.Click += BtnRight_Click;
            BtnLeft.Click += BtnLeft_Click;
            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (LstAvailable.SelectedIndex >= 0)
            {
                LstSelected.Items.Add(LstAvailable.SelectedItem);
                LstAvailable.Items.RemoveAt(LstAvailable.SelectedIndex);
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (LstSelected.SelectedIndex >= 0)
            {
                LstAvailable.Items.Add(LstSelected.SelectedItem.ToString().Split()[0]);
                LstSelected.Items.RemoveAt(LstSelected.SelectedIndex);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtTopLeft.Text))
            {
                MessageBox.Show("You must enter the coordinates of the top left corner.");
                return;
            }
            string[] parts = TxtTopLeft.Text.Split(':');
            if (parts.Length != 2)
            {
                MessageBox.Show("Coordinates for top left corner must be separated by a colon.");
                return;
            }
            if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                MessageBox.Show("Coordinates for top left corner must be numeric.");
                return;
            }
            if (string.IsNullOrEmpty(TxtBottomRight.Text))
            {
                MessageBox.Show("You must enter the coordinates for the bottom right corner.");
                return;
            }
            parts = TxtBottomRight.Text.Split(':');
            if (parts.Length != 2)
            {
                MessageBox.Show("Coordinates for the bottom right corner must be separated by a colon.");
                return;
            }
            if (!int.TryParse(parts[0], out int x2) || !int.TryParse(parts[1], out int y2))
            {
                MessageBox.Show("Coordinates for the bottom right corner must be numeric.");
                return;
            }
            if (LstSelected.Items.Count < 1)
            {
                MessageBox.Show("You must select at least one shadow monster for the area.");
                return;
            }

            List<string> monsters = new List<string>();

            foreach (var v in LstSelected.Items)
            {
                monsters.Add(v.ToString());
            }
            WildArea = new WildArea()
            {
                TopLeft = new Microsoft.Xna.Framework.Point(x, y),
                BottomRight = new Microsoft.Xna.Framework.Point(x2, y2),
                Monsters = monsters
            };
            OkPressed = true;
            Close();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
