using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowMonsters.TileEngine
{
    public class Tile
    {
        #region Property Region

        public int TileSet { get; set; }
        public int TileIndex { get; set; }

        #endregion

        #region Constructor Region

        public Tile()
        {
            TileSet = -1;
            TileIndex = -1;
        }

        public Tile(int set, int index)
        {
            TileSet = set;
            TileIndex = index;
        }

        #endregion
    }

    public class TileLayer
    {
        #region Field Region

        readonly Tile[] tiles;

        int width;
        int height;

        Point cameraPoint;
        Point viewPoint;
        Point min;
        Point max;
        Rectangle destination;

        #endregion

        #region Property Region

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        #endregion

        #region Constructor Region

        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }

        public TileLayer(Tile[] tiles, int width, int height)
            : this()
        {
            this.tiles = (Tile[])tiles.Clone();
            this.width = width;
            this.height = height;
        }

        public TileLayer(int width, int height)
            : this()
        {
            tiles = new Tile[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = new Tile();
                }
            }
        }

        public TileLayer(int width, int height, int set, int index)
            : this()
        {
            tiles = new Tile[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = new Tile(set, index);
                }
            }
        }

        #endregion

        #region Method Region

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return new Tile();

            if (x >= width || y >= height)
                return new Tile();

            return tiles[y * width + x];
        }

        public void SetTile(int x, int y, int tileSet, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= width || y >= height)
                return;

            tiles[y * width + x] = new Tile(tileSet, tileIndex);
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, TileSet tileSet, Camera camera)
        {
            if (!Visible)
                return;

            cameraPoint = Engine.VectorToCell(camera.Position);
            viewPoint = Engine.VectorToCell(
                new Vector2(
                    (camera.Position.X + Engine.ViewportRectangle.Width),
                    (camera.Position.Y + Engine.ViewportRectangle.Height)));

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);

            destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            Tile tile;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    tile = GetTile(x, y);

                    if (tile.TileSet == -1 || tile.TileIndex == -1)
                        continue;

                    destination.X = x * Engine.TileWidth;

                    spriteBatch.Draw(
                        tileSet.Textures[tile.TileSet],
                        destination,
                        tileSet.SourceRectangles[tile.TileIndex],
                        Color.White);

                }
            }

            spriteBatch.End();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(width);
            writer.Write(height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    writer.Write(tiles[y * width + x].TileSet);
                    writer.Write(tiles[y * width + x].TileIndex);
                }
            }
        }

        public static TileLayer Load(BinaryReader reader)
        {
            TileLayer data = new TileLayer(reader.ReadInt32(), reader.ReadInt32());

            for (int y = 0; y < data.Height; y++)
            {
                for (int x = 0; x < data.Width; x++)
                {
                    data.SetTile(x, y, reader.ReadInt32(), reader.ReadInt32());
                }
            }

            return data;
        }

        #endregion

    }
}
