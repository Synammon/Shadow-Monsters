using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.ShadowMonsters;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShadowMonsters.Characters
{
    public class Character
    {
        #region Constant

        public const float SpeakingRadius = 40f;
        public const int MonsterLimit = 6;

        #endregion

        #region Field Region

        protected string name;
        protected string textureName;
        protected ShadowMonster[] monsters = new ShadowMonster[MonsterLimit];
        protected int currentMonster;
        protected ShadowMonster givingMonster;
        protected AnimatedSprite sprite;
        protected Point sourceTile;

        protected string conversation;

        protected static Game gameRef;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SpriteName
        {
            get { return textureName; }
        }
        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        public ShadowMonster BattleMonster
        {
            get { return monsters[currentMonster]; }
        }

        public ShadowMonster GiveMonster
        {
            get { return givingMonster; }
        }

        public string Conversation
        {
            get { return conversation; }
        }

        public bool Battled
        {
            get;
            set;
        }

        public Point SourceTile
        {
            get { return sourceTile; }
            set { sourceTile = value; }
        }
        public List<ShadowMonster> BattleMonsters => monsters.ToList<ShadowMonster>();

        #endregion

        #region Constructor Region

        protected Character()
        {
        }

        #endregion

        #region Method Region

        private static void BuildAnimations()
        {
        }

        public bool NextMonster()
        {
            currentMonster++;

            return currentMonster < MonsterLimit && monsters[currentMonster] != null;
        }

        public static Character FromString(Game game, string characterString)
        {
            if (gameRef == null)
            {
                gameRef = game;
            }

            Character character = new Character();
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

        public void ChangeMonster(int index)
        {
            if (index < 0 || index >= MonsterLimit)
            {
                currentMonster = index;
            }
        }

        public void SetConversation(string newConversation)
        {
            this.conversation = newConversation;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }

        #endregion

        public bool Alive()
        {
            for (int i = 0; i < MonsterLimit; i++)
            {
                if (BattleMonsters[i] != null && BattleMonsters[i].CurrentHealth > 0)
                {
                    return true;
                }
            }

            return false;
        }
        public virtual bool Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();

            b.Append(name);
            b.Append(",");
            b.Append(textureName);
            b.Append(",");
            b.Append(sprite.CurrentAnimation);
            b.Append(",");
            b.Append(conversation);
            b.Append(",");
            b.Append(currentMonster);
            b.Append(",");
            b.Append(Battled);
            b.Append(",");
            b.Append(SourceTile.X + ":" + SourceTile.Y);

            string s = b.ToString();

            writer.Write(s);
            writer.Write(-1);

            foreach (ShadowMonster a in monsters)
            {
                if (a != null)
                {
                    a.Save(writer);
                    writer.Write(-1);
                }
                else
                {
                    writer.Write("*"); 
                    writer.Write(-1);
                }
            }

            if (givingMonster != null)
            {
                givingMonster.Save(writer);
                writer.Write(-1);
            }
            else
            {
                writer.Write("*");
                writer.Write(-1);
            }

            return true;
        }

        public static Character Load(ContentManager content, BinaryReader reader)
        {
            Character c = new Character();
            
            string data = reader.ReadString();
            string[] parts = data.Split(',');
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

            reader.ReadInt32();

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

            reader.ReadInt32();

            return c;
        }
    }
}
