using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.TileEngine;
using System;
using System.IO;
using System.Windows.Forms;

namespace ShadowEditor
{
    public partial class NewMapForm : Form
    {
        public bool OkPressed { get; private set; }
        public TileSet TileSet { get; private set; }
        private GraphicsDevice graphics;
        public TileMap TileMap { get; private set; }

        public NewMapForm(GraphicsDevice graphics)
        {
            InitializeComponent();
            this.graphics = graphics;

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            btnAdd.Click += BtnAdd_Click;
            btnRemove.Click += BtnRemove_Click;

            FormClosing += FormNew_FormClosing;
        }

        private void FormNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender == btnOK)
            {


            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtWidth.Text, out int mapWidth))
            {
                MessageBox.Show("Map Width must be numeric");
                return;
            }

            if (mapWidth <= 0)
            {
                MessageBox.Show("Map Width must be greater than zero.");
                return;
            }

            if (!int.TryParse(txtHeight.Text, out int mapHeight))
            {
                MessageBox.Show("Map Height must be numeric");
                return;
            }

            if (mapHeight <= 0)
            {
                MessageBox.Show("Map Height must be greater than zero.");
                return;
            }

            if (!int.TryParse(txtTileSetWidth.Text, out int tileWidth))
            {
                MessageBox.Show("Tile Width must be numeric");
                return;
            }

            if (tileWidth <= 0)
            {
                MessageBox.Show("Tile Width must be greater than zero.");
                return;
            }

            if (!int.TryParse(txtTileSetHeight.Text, out int tileHeight))
            {
                MessageBox.Show("Tile Height must be numeric");
                return;
            }

            if (tileHeight <= 0)
            {
                MessageBox.Show("Tile Height must be greater than zero.");
                return;
            }

            if (!int.TryParse(txtTilesWide.Text, out int tilesWide))
            {
                MessageBox.Show("Tiles Wide must be numeric");
                return;
            }

            if (tilesWide <= 0)
            {
                MessageBox.Show("Tile Wide must be greater than zero.");
                return;
            }

            if (!int.TryParse(txtTilesHigh.Text, out int tilesHigh))
            {
                MessageBox.Show("Tiles High must be numeric");
                return;
            }

            if (tilesHigh <= 0)
            {
                MessageBox.Show("Tiles High must be greater than zero.");
                return;
            }

            if (lbTIleSetImages.Items.Count == 0)
            {
                MessageBox.Show("Map needs at least one image for the tile set");
                return;
            }

            TileLayer gr = new TileLayer(mapWidth, mapHeight, -1, -1);
            TileLayer ed = new TileLayer(mapWidth, mapHeight, -1, -1);
            TileLayer bu = new TileLayer(mapWidth, mapHeight, -1, -1);
            TileLayer de = new TileLayer(mapWidth, mapHeight, -1, -1);
            TileSet = new TileSet(tilesWide, tilesHigh, tileWidth, tileHeight);

            foreach (object o in lbTIleSetImages.Items)
            {
                string s = o.ToString();
                FileStream stream = new FileStream(s, FileMode.Open);
                Texture2D t = Texture2D.FromStream(graphics, stream);
                TileSet.Textures.Add(t);
                TileSet.TextureNames.Add(Path.GetFileNameWithoutExtension(s));
                stream.Close();
                stream.Dispose();
            }

            TileMap = new TileMap(TileSet, gr, ed, bu, de, txtName.Text);
            OkPressed = true;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lbTIleSetImages.SelectedIndex >= 0)
            {
                lbTIleSetImages.Items.RemoveAt(lbTIleSetImages.SelectedIndex);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.BMP;*.GIF;*.JPG;*.TGA;*.PNG"
            };
            ofd.Multiselect = true;

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                foreach (string s in ofd.FileNames)
                    lbTIleSetImages.Items.Add(s);
            }
        }
    }
}
