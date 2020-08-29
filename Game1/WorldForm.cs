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
    public partial class WorldForm : Form
    {
        public World World { get; set; }
        public bool OKPressed { get; set; }

        public WorldForm()
        {
            InitializeComponent();
        }

        private void WorldForm_Load(object sender, EventArgs e)
        {
            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSourceTile.Text))
            {
                MessageBox.Show("You must enter a source tile.");
                return;
            }

            if (string.IsNullOrEmpty(TxtDestinationTile.Text))
            {
                MessageBox.Show("You must enter a destination tile.");
                return;
            }

            if (string.IsNullOrEmpty(TxtDestinationLevel.Text))
            {
                MessageBox.Show("YOu must enter a destination level.");
                return;
            }

            string[] src = TxtSourceTile.Text.Split(':');
            string[] dest = TxtDestinationTile.Text.Split(':');

            if (!int.TryParse(src[0], out int xcoord))
            {
                MessageBox.Show("Source tile x-coordinate must be numeric.");
                return;
            }

            if (!int.TryParse(src[1], out int ycoord))
            {
                MessageBox.Show("Source tile y-coordinate must be numeric.");
                return;
            }

            Microsoft.Xna.Framework.Point s = new Microsoft.Xna.Framework.Point(xcoord, ycoord);

            if (!int.TryParse(dest[0], out xcoord))
            {
                MessageBox.Show("Destinationt tile x-coordinate must be numeric.");
                return;
            }

            if (!int.TryParse(dest[1], out ycoord))
            {
                MessageBox.Show("Destination tile y-coordinate myst be numeric.");
                return;
            }

            Microsoft.Xna.Framework.Point d = new Microsoft.Xna.Framework.Point(xcoord, ycoord);

            Portal p = new Portal(s, d, TxtDestinationLevel.Text);
            World = new World(p);

            OKPressed = true;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }
    }
}
