using Microsoft.Xna.Framework.Content;
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
    public partial class WildAreaListForm : Form
    {
        private TileMap map;

        public WildAreaListForm(TileMap map, ContentManager content)
        {
            InitializeComponent();

            this.map = map;

            foreach (var v in map.WildLayer.Areas)
            {
                LBAreas.Items.Add(v);
            }

            BtnAdd.Click += BtnAdd_Click;
            BtnEdit.Click += BtnEdit_Click;
            BtnDelete.Click += BtnDelete_Click;
            ShadowMonsterManager.FromFile(@".\Content\ShadowMonsters.mst", content);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            WildAreaForm frm = new WildAreaForm();
            frm.ShowDialog();

            if (frm.OkPressed)
            {
                LBAreas.Items.Add(frm.WildArea);
                map.WildLayer.Areas.Add(frm.WildArea);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBAreas.SelectedIndex < 0)
                return;

            LBAreas.Items.RemoveAt(LBAreas.SelectedIndex);
            map.WildLayer.Areas.RemoveAt(LBAreas.SelectedIndex);
        }
    }
}
