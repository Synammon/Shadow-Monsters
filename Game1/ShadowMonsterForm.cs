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
    public partial class ShadowMonsterForm : Form
    {
        public string ShadowMonster { get; set; }
        public string Source { get; set; }
        public bool OKPressed { get; internal set; }

        public ShadowMonsterForm()
        {
            InitializeComponent();
        }

        public ShadowMonsterForm(string avatar, string source)
        {
            InitializeComponent();

            TxtShadowMonster.Text = avatar;
            TxtSource.Text = source;
        }

        private void FormShadowMonster_Load(object sender, EventArgs e)
        {
            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtShadowMonster.Text))
            {
                MessageBox.Show("You must enter the name of the avatar.");
                return;
            }

            if (string.IsNullOrEmpty(TxtSource.Text))
            {
                MessageBox.Show("You must enter the source tile.");
                return;
            }

            if (!TxtSource.Text.Contains(':'))
            {
                MessageBox.Show("Source tile must be seperated by a :.");
                return;
            }

            string[] parts = TxtSource.Text.Split(':');

            if (!int.TryParse(parts[0], out int xcoord))
            {
                MessageBox.Show("X-coordinate must be numeric.");
                return;
            }

            if (xcoord < 0)
            {
                MessageBox.Show("X coordinate must not be negative.");
                return;
            }

            if (!int.TryParse(parts[1], out int ycoord))
            {
                MessageBox.Show("Y coordinate must be numeric.");
                return;
            }

            if (ycoord < 0)
            {
                MessageBox.Show("Y coordinate must not be negative.");
                return;
            }

            ShadowMonster = TxtShadowMonster.Text;
            Source = TxtSource.Text;
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
