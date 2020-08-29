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
    public partial class PortalListForm : Form
    {
        private readonly TileMap map;
        private readonly Editor game;

        public PortalListForm(TileMap map, Editor game)
        {
            InitializeComponent();
            this.map = map;
            this.game = game;

            foreach (var p in map.PortalLayer.Portals.Keys)
            {
                LBPortals.Items.Add(p);
            }

            BtnAdd.Click += BtnAdd_Click;
            BtnEdit.Click += BtnEdit_Click;
            BtnDelete.Click += BtnDelete_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            PortalForm form = new PortalForm();
            form.ShowDialog();

            if (form.OkPressed)
            {
                map.PortalLayer.AddPortal(form.PortalName, form.Portal);
                LBPortals.Items.Add(form.PortalName);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (LBPortals.SelectedIndex >= 0)
            {
                PortalForm form = new PortalForm(
                    LBPortals.SelectedItem.ToString(),
                    map.PortalLayer.Portals[LBPortals.SelectedItem.ToString()]);

                form.ShowDialog();

                if (form.OkPressed)
                {
                    map.PortalLayer.Portals[form.PortalName] = form.Portal;
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBPortals.SelectedIndex >= 0)
            {
                map.PortalLayer.Portals.Remove(LBPortals.SelectedItem.ToString());
                LBPortals.Items.RemoveAt(LBPortals.SelectedIndex);
            }
        }
    }
}
