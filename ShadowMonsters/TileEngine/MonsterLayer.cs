using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class MonsterLayer
    {
        #region Field Region

        private Dictionary<Point, ShadowMonster> monsters = new Dictionary<Point, ShadowMonster>();

        #endregion

        #region Property Region

        public Dictionary<Point, ShadowMonster> Monsters
        {
            get { return monsters; }
        }

        #endregion

        #region Constructor Region
        #endregion

        #region Method Region

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            foreach (Point p in monsters.Keys)
            {
                spriteBatch.Draw(
                    monsters[p].Texture,
                    new Rectangle(
                        p.X * Engine.TileWidth,
                        p.Y * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight),
                    null,
                    Color.White);
            }

            spriteBatch.End();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(monsters.Count);

            foreach (Point p in monsters.Keys)
            {
                writer.Write(p.X);
                writer.Write(p.Y);
                monsters[p].Save(writer);
            }
        }

        public static MonsterLayer Load(ContentManager content, BinaryReader reader)
        {
            MonsterLayer layer = new MonsterLayer();

            int count = reader.ReadInt32();
            
            for (int i = 0; i < count; i++)
            {
                Point p = new Point(reader.ReadInt32(), reader.ReadInt32());
                ShadowMonster monster = ShadowMonster.Load(content, reader.ReadString());

                layer.Monsters.Add(p, monster);
            }

            return layer;
        }
        #endregion
    }
}
