using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowEditor
{
    public partial class DefinitionListForm : Form
    {
        private readonly byte[] IV = new byte[]
        {
            067, 197, 032, 010, 211, 090, 192, 076,
            054, 154, 111, 023, 243, 071, 132, 090
        };

        private readonly byte[] Key = new byte[]
        {
            067, 090, 197, 043, 049, 029, 178, 211,
            127, 255, 097, 233, 162, 067, 111, 022,
        };

        public DefinitionListForm()
        {
            InitializeComponent();

            BtnAdd.Click += BtnAdd_Click;
            BtnDelete.Click += BtnDelete_Click;
            BtnEdit.Click += BtnEdit_Click;

            BtnImport.Click += BtnImport_Click;
            BtnExport.Click += BtnExport_Click;

            foreach (var v in ShadowMonsterManager.ShadowMonsterList.Keys)
            {
                LBDefinitions.Items.Add(v);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Definitions (*.txt)|*.txt"
            };

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                LoadShadowMonsters(ofd.FileName);
            }
        }

        private void LoadShadowMonsters(string filename)
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                try
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                    FileStream stream = new FileStream(
                        filename,
                        FileMode.Open,
                        FileAccess.Read);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        decryptor,
                        CryptoStreamMode.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(cryptoStream))
                        {
                            int count = reader.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                string data = reader.ReadString();
                                reader.ReadInt32();
                                ShadowMonster monster = ShadowMonster.FromString(data);
                                ShadowMonsterManager.AddShadowMonster(monster.Name, monster);
                                LBDefinitions.Items.Add(monster.Name);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Definitions (*.txt)|*.txt"
            };

            DialogResult result = sfd.ShowDialog();

            if (result != DialogResult.OK)
                return;

            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                try
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                    FileStream stream = new FileStream(
                        sfd.FileName,
                        FileMode.Create,
                        FileAccess.Write);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        encryptor,
                        CryptoStreamMode.Write))
                    {
                        BinaryWriter writer = new BinaryWriter(cryptoStream);

                        writer.Write(ShadowMonsterManager.ShadowMonsterList.Count);

                        foreach (ShadowMonster shadowMonster in ShadowMonsterManager.ShadowMonsterList.Values)
                        {
                            writer.Write(shadowMonster.ToString());
                            writer.Write(-1);
                        }

                        writer.Close();
                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }

        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (LBDefinitions.SelectedIndex < 0)
                return;

            DefinitionForm frm = new DefinitionForm(
                ShadowMonsterManager.ShadowMonsterList[LBDefinitions.SelectedItem.ToString()]);
            frm.ShowDialog();

            if (!frm.OkPressed)
                return;

            ShadowMonsterManager.ShadowMonsterList[LBDefinitions.SelectedItem.ToString()] =
                ShadowMonster.FromString(frm.ShadowMonster);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (LBDefinitions.SelectedIndex < 0)
                return;

            ShadowMonsterManager.ShadowMonsterList.Remove(LBDefinitions.SelectedItem.ToString());
            LBDefinitions.Items.Remove(LBDefinitions.SelectedItem);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DefinitionForm frm = new DefinitionForm();
            frm.ShowDialog();

            if (frm.OkPressed)
            {
                ShadowMonster m = ShadowMonster.FromString(frm.ShadowMonster);
                ShadowMonsterManager.AddShadowMonster(m.Name.ToLowerInvariant(), m);
                LBDefinitions.Items.Add(m.Name);
            }
        }
    }
}
