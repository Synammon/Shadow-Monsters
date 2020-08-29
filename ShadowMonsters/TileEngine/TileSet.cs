using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace ShadowMonsters.TileEngine
{
    public class TileSet
    {
        public int TilesWide = 8;
        public int TilesHigh = 10;
        public int TileWidth = 16;
        public int TileHeight = 16;

        #region Fields and Properties

        private List<Texture2D> image;
        private List<string> imageName;
        private Rectangle[] sourceRectangles;

        #endregion

        #region Property Region

        public List<Texture2D> Textures
        {
            get { return image; }
        }

        public List<string> TextureNames
        {
            get { return imageName; }
            set { imageName = value; }
        }
         
        public Rectangle[] SourceRectangles
        {
            get { return sourceRectangles; }
        }

        #endregion

        #region Constructor Region

        public TileSet()
        {
            image = new List<Texture2D>();
            imageName = new List<string>();

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight);
                    tile++;
                }
        }

        public TileSet(int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
            : this()
        {
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
            {
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight);
                    tile++;
                }
            }
        }

        #endregion

        #region Method Region
        public void Save(BinaryWriter writer)
        {
            writer.Write(imageName.Count);

            foreach (string s in imageName)
            {
                writer.Write(s);
                writer.Write(-1);
            }
            writer.Write(TilesWide);
            writer.Write(TilesHigh);
            writer.Write(TileWidth);
            writer.Write(TileHeight);
        }

        public static TileSet Load(ContentManager content, BinaryReader reader)
        {
            TileSet t = new TileSet();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                string texture = reader.ReadString();
                reader.ReadInt32();
                t.image.Add(content.Load<Texture2D>(@"Tiles\" + texture));
                t.imageName.Add(texture);
            }

            t.TilesWide = reader.ReadInt32();
            t.TilesHigh = reader.ReadInt32();
            t.TileWidth = reader.ReadInt32();
            t.TileHeight = reader.ReadInt32();

            t.sourceRectangles = new Rectangle[t.TilesWide * t.TilesHigh];

            int tile = 0;

            for (int y = 0; y < t.TilesHigh; y++)
            {
                for (int x = 0; x < t.TilesWide; x++)
                {
                    t.sourceRectangles[tile] = new Rectangle(
                        x * t.TileWidth,
                        y * t.TileHeight,
                        t.TileWidth,
                        t.TileHeight);
                    tile++;
                }
            }

            return t;
        }
        #endregion

    }
}
