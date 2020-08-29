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
    public partial class ShadowMonsterListForm : Form
    {
        private readonly TileMap map;
        private readonly Editor game;

        public ShadowMonsterListForm(TileMap map, Editor game)
        {
            InitializeComponent();

            this.map = map;
            this.game = game;

            foreach (var m in map.MonsterLayer.Monsters.Values)
            {
                LBMonsters.Items.Add(m.Name);
            }

            BtnAdd.Click += BtnAdd_Click;
            BtnEdit.Click += BtnEdit_Click;
            BtnDelete.Click += BtnDelete_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShadowMonsterForm form = new ShadowMonsterForm();
            form.ShowDialog();

            if (form.OKPressed)
            {
                string[] parts = form.Source.Split(':');
                map.MonsterLayer.Monsters.Add(
                    new Microsoft.Xna.Framework.Point(
                        int.Parse(parts[0]),
                        int.Parse(parts[1])),
                    ShadowMonsterManager.GetShadowMonster(form.ShadowMonster.ToLowerInvariant()));
                LBMonsters.Items.Add(form.ShadowMonster);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (LBMonsters.SelectedIndex < 0 || LBMonsters.Items.Count == 0)
            {
                return;
            }

            if (LBMonsters.SelectedItem.ToString() == "Unknown")
            {
                return;
            }

            foreach (var p in map.MonsterLayer.Monsters.Keys)
            {
                if (map.MonsterLayer.Monsters[p].Name.ToLowerInvariant() == 
                    LBMonsters.SelectedItem.ToString().ToLowerInvariant())
                {
                    ShadowMonsterForm form = new ShadowMonsterForm(
                        LBMonsters.SelectedItem.ToString(),
                        p.X + ":" + p.Y);

                    form.ShowDialog();

                    if (form.OKPressed)
                    {
                        map.MonsterLayer.Monsters[p] = 
                            ShadowMonsterManager.GetShadowMonster(
                                form.ShadowMonster.ToLowerInvariant());                            
                        LBMonsters.Items[LBMonsters.SelectedIndex] = form.ShadowMonster;
                        break;
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBMonsters.SelectedIndex < 0)
            {
                return;
            }

            foreach (var p in map.MonsterLayer.Monsters.Keys)
            {
                if (map.MonsterLayer.Monsters[p].Name.ToLowerInvariant() == 
                    LBMonsters.SelectedItem.ToString().ToLowerInvariant())
                {
                    map.MonsterLayer.Monsters.Remove(p);
                    break;
                }
            }

            LBMonsters.Items.RemoveAt(LBMonsters.SelectedIndex);
        }
    }
}
