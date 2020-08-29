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
    public partial class PortalForm : Form
    {
        public Portal Portal { get; set; }
        public bool OkPressed { get; private set; }
        public string PortalName { get; internal set; }

        public PortalForm(string name = null, Portal portal = null)
        {
            InitializeComponent();

            if (name != null && portal != null)
            {
                TxtName.Enabled = false;
                TxtName.Text = name;
                TxtDestination.Text = portal.DestinationLevel;
                TxtDestinationTile.Text = portal.DestinationTile.X + ":" + portal.DestinationTile.Y;
                TxtSourceTile.Text = portal.SourceTile.X + ":" + portal.SourceTile.Y;
            }
        }

        private void FormPortal_Load(object sender, EventArgs e)
        {
            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OkPressed = false;
            Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                MessageBox.Show("You must enter the name of the portal.");
                return;
            }

            if (string.IsNullOrEmpty(TxtDestination.Text))
            {
                MessageBox.Show("You must enter a destination level.");
                return;
            }

            if (string.IsNullOrEmpty(TxtDestinationTile.Text))
            {
                MessageBox.Show("You must enter the destination tile.");

                return;
            }

            if (string.IsNullOrEmpty(TxtSourceTile.Text))
            {
                MessageBox.Show("You must enter the source3 tile.");
                return;
            }

            string[] source = TxtSourceTile.Text.Split(':');
            string[] dest = TxtDestinationTile.Text.Split(':');

            Portal = new Portal(
                new Microsoft.Xna.Framework.Point(
                    int.Parse(source[0]),
                    int.Parse(source[1])
                    ),
                new Microsoft.Xna.Framework.Point(
                    int.Parse(dest[0]),
                    int.Parse(dest[1])
                    ),
                TxtDestination.Text);

            PortalName = TxtName.Text;
            OkPressed = true;

            Close();
        }
    }
}
