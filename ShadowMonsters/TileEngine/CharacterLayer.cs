using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Characters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class CharacterLayer
    {
        #region Field Region

        private readonly Dictionary<Point, Character> characters;

        #endregion

        #region Property Region

        public Dictionary<Point, Character> Characters => characters;

        #endregion

        #region Constructor Region

        public CharacterLayer()
        {
            characters = new Dictionary<Point, Character>();
        }
        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            foreach (Character c in characters.Values)
            {
                c.Update(gameTime);
            }
        }

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

            foreach (Character c in characters.Values)
            {
                c.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public bool Save(BinaryWriter writer)
        {
            writer.Write(characters.Count);

            foreach (Point p in characters.Keys)
            {
                if (characters[p] is Merchant)
                {
                    writer.Write(2);
                }
                else
                {
                    writer.Write(1);
                }

                writer.Write(p.X);
                writer.Write(p.Y);
                
                characters[p].Save(writer);
            }

            return true;
        }

        public static CharacterLayer Load(ContentManager content, BinaryReader reader)
        {
            CharacterLayer layer = new CharacterLayer();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                Character c = null;
                int charType = reader.ReadInt32();
                Point position = new Point(reader.ReadInt32(), reader.ReadInt32());

                if (charType == 1)
                {
                    c = Character.Load(content, reader);
                }
                else if (charType == 2)
                {
                    c = Merchant.Load(content, reader);
                }

                layer.characters.Add(position, c);
            }
            return layer;
        }

        public Character GetCharacter(string v)
        {
            foreach (Character c in characters.Values)
            {
                if (c.Name == v)
                {
                    return c;
                }

            }

            return null;
        }
        #endregion
    }
}
