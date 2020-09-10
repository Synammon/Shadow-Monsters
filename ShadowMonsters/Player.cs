using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Items;
using ShadowMonsters.ShadowMonsters;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters
{
    public class Player : DrawableGameComponent
    {
        public const int MaxShadowMonsters = 6;

        #region Field Region

        private Game1 gameRef;
        private string name;
        private bool gender;
        private string mapName;
        private Point tile;
        private AnimatedSprite sprite;
        private Texture2D texture;
        private string textureName;
        private float speed = 256f;

        private Vector2 position;
        private Backpack backpack;

        private readonly List<ShadowMonster> shadowMonsters = new List<ShadowMonster>();
        private readonly ShadowMonster[] battleShadowMonsters = new ShadowMonster[MaxShadowMonsters];
        private int selected;
        private readonly Dictionary<string, string> _charactersMet = new Dictionary<string, string>();
        private readonly Dictionary<int, string> _keysFound = new Dictionary<int, string>();

        #endregion

        #region Property Region

        public ShadowMonster[] BattleShadowMonsters
        {
            get { return battleShadowMonsters; }
        }

        public Backpack Backpack { get => backpack; }
        public ShadowMonster Selected
        {
            get { return battleShadowMonsters[selected]; }
        }

        public Dictionary<int, string> KeysFound => _keysFound;

        public Dictionary<string, string> CharactersMet => _charactersMet;

        public int Gold { get; internal set; }
        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }

        #endregion

        #region Constructor Region

        private Player(Game game)
            : base(game)
        {
            backpack = new Backpack();
        }

        public Player(Game game, string name, bool gender, string textureName)
            : base(game)
        {
            gameRef = (Game1)game;
            this.name = name;
            this.gender = gender;

            if (gender)
                this.textureName = textureName + "_f";
            else
                this.textureName = textureName + "_m";

            sprite = new AnimatedSprite(
                game.Content.Load<Texture2D>(@"CharacterSprites\" + this.textureName), 
                Game1.Animations)
            {
                CurrentAnimation = AnimationKey.WalkDown
            };
            Gold = 1000;
            backpack = new Backpack();
        }

        #endregion

        #region Method Region
        public virtual void AddShadowMonster(ShadowMonster shadowMonster)
        {
            shadowMonsters.Add(shadowMonster);
        }

        public void SetCurrentShadowMonster(int index)
        {
            if (index < 0 || index >= MaxShadowMonsters)
                throw new IndexOutOfRangeException();

            if (battleShadowMonsters[index] != null)
                selected = index;
        }

        public ShadowMonster GetShadowMonster(int index)
        {
            if (index < 0 || index >= MaxShadowMonsters)
                throw new IndexOutOfRangeException();

            return shadowMonsters[index];
        }
        internal bool Alive()
        {
            for (int i = 0; i < MaxShadowMonsters; i++)
                if (battleShadowMonsters[i] != null && battleShadowMonsters[i].Alive)
                    return true;

            return false;
        }

        public ShadowMonster GetBattleShadowMonster(int index)
        {
            if (index < 0 || index > MaxShadowMonsters)
                throw new IndexOutOfRangeException();

            return battleShadowMonsters[index];
        }

        internal void HealBattleShadowMonsters()
        {
            foreach (ShadowMonster a in battleShadowMonsters)
            {
                if (a != null)
                {
                    a.Heal(a.GetHealth());
                    a.IsAsleep = false;
                    a.IsConfused = false;
                    a.IsFainted = false;
                    a.IsParalyzed = false;
                    a.IsPoisoned = false;
                }
            }
        }

        internal void AddKey(int key, string name)
        {
            if (!KeysFound.ContainsKey(key))
                KeysFound.Add(key, name);
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sprite.Draw(gameTime, gameRef.SpriteBatch);
            base.Draw(gameTime);
        }

        internal void Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();

            b.Append(name);
            b.Append(",");
            b.Append(gender);
            b.Append(",");
            b.Append(mapName);
            b.Append(",");
            b.Append(tile.X);
            b.Append(",");
            b.Append(tile.Y);
            b.Append(",");
            b.Append(textureName);
            b.Append(",");
            b.Append(speed);
            b.Append(",");
            b.Append(sprite.CurrentAnimation);
            b.Append(",");
            b.Append(sprite.Position.X);
            b.Append(",");
            b.Append(sprite.Position.Y);

            writer.Write(b.ToString());
            writer.Write(shadowMonsters.Count);

            foreach (ShadowMonster a in shadowMonsters)
            {
                a.Save(writer);
                writer.Write(-1);
            }

            writer.Write(selected);

            foreach (ShadowMonster a in battleShadowMonsters)
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

            writer.Write(-1);
            backpack.Save(writer);
        }

        public static Player Load(Game1 game, BinaryReader reader)
        {
            Player player = new Player(game);
            player.gameRef = game;
            string data = reader.ReadString();
            string[] parts = data.Split(',');

            player.name = parts[0];
            player.gender = bool.Parse(parts[1]);
            player.mapName = parts[2];
            player.tile = new Point(
                int.Parse(parts[3]),
                int.Parse(parts[4]));
            player.textureName = parts[5];
            player.speed = float.Parse(parts[6]);
            player.sprite = new AnimatedSprite(
                game.Content.Load<Texture2D>(parts[5]),
                Game1.Animations);
            player.sprite.CurrentAnimation = (AnimationKey)Enum.Parse(typeof(AnimationKey), parts[7]);
            player.sprite.Position = new Vector2(float.Parse(parts[8]), float.Parse(parts[9]));

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                ShadowMonster a = ShadowMonster.Load(game.Content, reader.ReadString());
                reader.ReadInt32();

                player.shadowMonsters.Add(a);
            }

            player.selected = reader.ReadInt32();

            for (int i = 0; i < 6; i++)
            {
                string monster = reader.ReadString();
                reader.ReadInt32();

                if (monster != "*")
                {
                    ShadowMonster a = ShadowMonster.Load(game.Content, monster);
                    player.battleShadowMonsters[i] = a;
                }
            }

            player.backpack = Backpack.Load(reader);

            return player;
        }

        #endregion
    }
}
