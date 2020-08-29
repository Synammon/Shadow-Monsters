using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public enum CollisionType { Passable, Impassable, Water }

    public class CollisionLayer
    {
        private readonly Dictionary<Point, CollisionType> collisions = new Dictionary<Point, CollisionType>();
        private static Texture2D texture;
        public static Texture2D Texture { get => texture; set => texture = value; }

        public CollisionLayer()
        {
        }

        public Dictionary<Point, CollisionType> Collisions
        {
            get { return collisions; }
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write(Collisions.Count);
            foreach (Point p in collisions.Keys)
            {
                writer.Write((int)Collisions[p]);
                writer.Write(p.X);
                writer.Write(p.Y);
            }
        }
        internal static CollisionLayer Load(BinaryReader reader)
        {
            CollisionLayer layer = new CollisionLayer();
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                CollisionType collision = (CollisionType)reader.ReadInt32();
                Point area = new Point(
                    reader.ReadInt32(),
                    reader.ReadInt32());

                layer.collisions.Add(area, collision);
            }

            return layer;
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            foreach (Point p in collisions.Keys)
            {
                spriteBatch.Draw(
                    texture,
                    new Rectangle(
                        p.X * Engine.TileWidth,
                        p.Y * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight),
                    Color.White);
            }

            spriteBatch.End();
        }
    }
}
