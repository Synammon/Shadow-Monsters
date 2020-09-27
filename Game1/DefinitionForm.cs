using ShadowMonsters.ShadowMonsters;
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
    public partial class DefinitionForm : Form
    {
        public string ShadowMonster { get; private set; }
        public bool OkPressed { get; private set; }

        public DefinitionForm()
        {
            InitializeComponent();

            this.Load += DefinitionForm_Load;

            foreach (var v in Enum.GetNames(typeof(ShadowMonsterElement)))
            {
                cboElement.Items.Add(v);
            }

            cboElement.SelectedIndex = 0;

            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnAdd.Click += BtnAdd_Click;
            BtnRemove.Click += BtnRemove_Click;
        }

        public DefinitionForm(ShadowMonster m) : this()
        {
            TxtKey.Text = m.Name;
            TxtName.Text = m.DisplayName;
            cboElement.SelectedItem = m.Element;
            TxtCost.Text = m.Cost.ToString();
            TxtLevel.Text = m.Level.ToString();
            TxtAttack.Text = m.BaseAttack.ToString();
            TxtDefense.Text = m.BaseDefense.ToString();
            TxtSpeed.Text = m.BaseSpeed.ToString();
            TxtHealth.Text = m.BaseHealth.ToString();

            foreach (IMove move in m.KnownMoves.Values)
            {
                LBMoves.Items.Add(move.Name + ":" + move.UnlockedAt);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                MessageBox.Show("You must enter a name for the shadow monster.");
                return;
            }

            if (string.IsNullOrEmpty(TxtKey.Text))
            {
                MessageBox.Show("You must enter the name of the shadow monster.");
                return;
            }

            if (string.IsNullOrEmpty(TxtCost.Text)
                || !int.TryParse(TxtCost.Text, out int cost))
            {
                MessageBox.Show("You must enter a numeric value for cost");
                return;
            }

            if (string.IsNullOrEmpty(TxtLevel.Text)
                || !int.TryParse(TxtLevel.Text, out int level))
            {
                MessageBox.Show("You must enter a numeric value for level,");
                return;
            }

            if (string.IsNullOrEmpty(TxtAttack.Text)
                || !int.TryParse(TxtAttack.Text, out int attack))
            {
                MessageBox.Show("You must enter a numeric value for attack,");
                return;
            }

            if (string.IsNullOrEmpty(TxtDefense.Text)
                || !int.TryParse(TxtDefense.Text, out int defense))
            {
                MessageBox.Show("You must enter a numeric value for defense.");
                return;
            }

            if (string.IsNullOrEmpty(TxtSpeed.Text)
                || !int.TryParse(TxtSpeed.Text, out int speed))
            {
                MessageBox.Show("You must enter a numeric value for speed,");
                return;
            }

            if (string.IsNullOrEmpty(TxtHealth.Text)
                || !int.TryParse(TxtHealth.Text, out int health))
            {
                MessageBox.Show("You must enter a numeric value for health,");
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(TxtKey.Text + "," + TxtName.Text + "," + cboElement.SelectedItem + "," + cost + ",");
            sb.Append(level + "," + attack + "," + defense + "," + speed + "," + health + ",0,0");

            foreach (object o in LBMoves.Items)
            {
                string s = o.ToString();
                sb.Append("," + s);
            }

            ShadowMonster = sb.ToString();
            OkPressed = true;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            LBMoves.Items.Add(CboMoves.SelectedItem.ToString() + ":" + TxtMoveLevel.Text);

        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (LBMoves.SelectedIndex < 0)
                return;

            LBMoves.Items.RemoveAt(LBMoves.SelectedIndex);
        }

        private void DefinitionForm_Load(object sender, EventArgs e)
        {
            MoveManager.FillMoves();

            foreach (IMove move in MoveManager.Moves.Values)
            {
                CboMoves.Items.Add(move.Name);
            }

            CboMoves.SelectedIndex = 1;
        }
    }
}
