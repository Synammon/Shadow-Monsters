using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Items;
using ShadowMonsters.ShadowMonsters;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Characters
{
    public class Merchant : Character
    {
        private Backpack backpack;
        public Backpack Backpack => backpack;

        private Merchant()
            : base()
        {
        }
        public new static Merchant FromString(Game game, string characterString)
        {
            if (gameRef == null)
            {
                gameRef = game;
            }

            Merchant character = new Merchant();
            character.backpack = new Backpack();
            string[] parts = characterString.Split(',');

            character.name = parts[0];
            character.textureName = parts[1];
            character.sprite = new AnimatedSprite(
                game.Content.Load<Texture2D>(@"CharacterSprites\" + parts[1]),
                Game1.Animations)
            {
                CurrentAnimation = (AnimationKey)Enum.Parse(typeof(AnimationKey), parts[2])
            };
            character.conversation = parts[3];
            character.currentMonster = int.Parse(parts[4]);
            string[] items = parts[5].Split(':');
            character.SourceTile = new Point(int.Parse(items[0]), int.Parse(items[1]));
            character.sprite.Position = new Vector2(
                character.SourceTile.X * Engine.TileWidth,
                character.SourceTile.Y * Engine.TileHeight);

            for (int i = 6; i < 12 && i < parts.Length - 1; i++)
            {
                ShadowMonster monster = ShadowMonsterManager.GetShadowMonster(parts[i].ToLowerInvariant());
                character.monsters[i - 6] = monster;
            }

            character.givingMonster = ShadowMonsterManager.GetShadowMonster(parts[parts.Length - 1].ToLowerInvariant());
            return character;
        }

        public override bool Save(BinaryWriter writer)
        {
            base.Save(writer);
            backpack.Save(writer);

            return true;
        }
        new public static Merchant Load(ContentManager content, BinaryReader reader)
        {
            Merchant c = new Merchant
            {
                backpack = new Backpack()
            };

            string data = reader.ReadString();
            string[] parts = data.Split(',');
            reader.ReadInt32();

            c.name = parts[0];
            c.textureName = parts[1];
            c.sprite = new AnimatedSprite(
                content.Load<Texture2D>(
                    @"CharacterSprites\" + parts[1]),
                Game1.Animations);

            c.sprite.CurrentAnimation = (AnimationKey)Enum.Parse(typeof(AnimationKey), parts[2]);
            c.conversation = parts[3];
            c.currentMonster = int.Parse(parts[4]);
            c.Battled = bool.Parse(parts[5]);
            string[] items = parts[6].Split(':');
            c.SourceTile = new Point(int.Parse(items[0]), int.Parse(items[1]));
            c.sprite.Position = new Vector2(
                c.SourceTile.X * Engine.TileWidth,
                c.SourceTile.Y * Engine.TileHeight);

            for (int i = 0; i < 6; i++)
            {
                string avatar = reader.ReadString();

                if (avatar != "*")
                {
                    c.monsters[i] = ShadowMonster.Load(content, avatar);
                }

                reader.ReadInt32();
            }

            string giving = reader.ReadString();

            if (giving != "*")
            {
                c.givingMonster = ShadowMonster.Load(content, giving);
            }

            c.backpack = Backpack.Load(reader);
            return c;
        }
    }
}
