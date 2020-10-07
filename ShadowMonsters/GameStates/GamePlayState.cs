using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.Characters;
using ShadowMonsters.ShadowMonsters;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class GamePlayState : BaseGameState
    {
        private Engine engine = new Engine(
            new Rectangle(
                0, 
                0, 
                Settings.Resolution.X, 
                Settings.Resolution.Y),
            Settings.TileSize.X,
            Settings.TileSize.Y);
        private World world;
        private Vector2 motion;
        private bool inMotion;
        private bool lastInMotion;
        private Rectangle collision;
        private ShadowMonsterManager monsterManager = new ShadowMonsterManager();
        private int frameCount = 0;
        private bool subscribed;

        public GamePlayState(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 16, 16);
            Color[] buffer = new Color[16 * 16];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Color(255, 0, 0, 128);
            }

            texture.SetData(buffer);

            CollisionLayer.Texture = texture;

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Color(0, 0, 255, 128);
            }

            texture.SetData(buffer);

            PortalLayer.Texture = texture;
            base.LoadContent();
        }

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

        public override void Update(GameTime gameTime)
        {
            if (lastInMotion != inMotion && motion == Vector2.Zero)
            {
                Game1.Player.Tile = Engine.VectorToCell(Engine.VectorFromOrigin(Game1.Player.Sprite.Center));
                WildArea area = world.Map.WildLayer.Enter(Game1.Player.Tile);

                if (area != null)
                {
                    lastInMotion = inMotion;

                    if (random.Next(0, 100) < 20)
                    {
                        string m = area.Monsters[
                                    random.Next(
                                    0,
                                    area.Monsters.Count)];

                        ShadowMonster monster =
                            (ShadowMonster)ShadowMonsterManager.GetShadowMonster(
                                m).Clone();

                        if (monster == null)
                        {
                            return;
                        }

                        GameRef.StartMonsterBattleState.SetCombatants(Game1.Player, monster);
                        manager.PushState(GameRef.StartMonsterBattleState);
                        ActionSelectionState.IsTrainerBattle = false;

                        return;
                    }
                }
            }

            lastInMotion = inMotion;

            frameCount++;
            HandleMovement(gameTime);

            if (Xin.CheckKeyReleased(Keys.Escape))
            {
                manager.PushState(GameRef.YesNoState);
                GameRef.YesNoState.Message = "Are you sure you want to quit? Unsaved progress will be lost.";
                Visible = true;
                GameRef.YesNoState.Show();

                if (!subscribed)
                {
                    GameRef.YesNoState.YesButton.Click += YesButton_Click;
                    GameRef.YesNoState.NoButton.Click += NoButton_Click;
                    subscribed = true;
                }
            }

            if ((Xin.CheckKeyReleased(Keys.Space) ||
                Xin.CheckKeyReleased(Keys.Enter)) && frameCount >= 5)
            {
                frameCount = 0;
                StartInteraction();
                HandlePortals();
            }

            if (Xin.CheckKeyReleased(Keys.I))
            {
                manager.PushState(GameRef.ItemSelectionState);
            }

            if ((Xin.CheckKeyReleased(Keys.B)) && frameCount >= 5)
            {
                frameCount = 0;
                StartBattle();
            }

            if (Xin.CheckKeyReleased(Keys.F1))
            {
                SaveGame();
            }
            if (Xin.CheckKeyReleased(Keys.F2))
            {
                LoadGame();
            }

            Engine.Camera.LockToSprite(
                world.Maps[world.CurrentMapName], 
                Game1.Player.Sprite,
                new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
            world.Update(gameTime);
            Game1.Player.Update(gameTime);

            base.Update(gameTime);
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            manager.PopState();
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            manager.ChangeState(GameRef.MainMenuState);
        }

        private void HandlePortals()
        {
            foreach (var r in world.Map.PortalLayer.Portals.Keys)
            {
                Portal p = world.Map.PortalLayer.Portals[r];
                Rectangle pRect = new Rectangle(
                    (int)Game1.Player.Sprite.Position.X,
                    (int)Game1.Player.Sprite.Position.Y,
                    Engine.TileWidth,
                    Engine.TileHeight);

                if (pRect.Intersects(
                    new Rectangle(
                        p.SourceTile.X * Engine.TileWidth,
                        p.SourceTile.Y * Engine.TileHeight,
                        Engine.TileHeight,
                        Engine.TileHeight)) ||
                        (Xin.CheckMouseReleased(MouseButtons.Left) &&
                            new Rectangle(
                                p.SourceTile.X,
                                p.SourceTile.Y,
                                Engine.TileWidth,
                                Engine.TileHeight).Contains(Xin.MouseAsPoint)))
                {
                    world.ChangeMap(p);

                    Game1.Player.Position = new Vector2(
                        p.DestinationTile.X * Engine.TileWidth,
                        p.DestinationTile.Y * Engine.TileHeight);
                    Engine.Camera.LockToSprite(
                        world.Map,
                        Game1.Player.Sprite,
                        new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y));
                    Game1.Player.MapName = world.CurrentMapName;
                    return;
                }
            }
        }

        private void HandleMovement(GameTime gameTime)
        {
            if (Xin.KeyboardState.IsKeyDown(Keys.W) && !inMotion)
            {
                motion.Y = -1;
                Game1.Player.Sprite.CurrentAnimation = AnimationKey.WalkUp;
                Game1.Player.Sprite.IsAnimating = true;
                inMotion = true;
                collision = new Rectangle(
                    (int)Game1.Player.Sprite.Position.X,
                    (int)Game1.Player.Sprite.Position.Y - Engine.TileHeight * 2,
                    Engine.TileWidth,
                    Engine.TileHeight);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S) && !inMotion)
            {
                motion.Y = 1;
                Game1.Player.Sprite.CurrentAnimation = AnimationKey.WalkDown;
                Game1.Player.Sprite.IsAnimating = true;
                inMotion = true;
                collision = new Rectangle(
                    (int)Game1.Player.Sprite.Position.X,
                    (int)Game1.Player.Sprite.Position.Y + Engine.TileHeight * 2,
                    Engine.TileWidth,
                    Engine.TileHeight);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.A) && !inMotion)
            {
                motion.X = -1;
                Game1.Player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
                Game1.Player.Sprite.IsAnimating = true;
                inMotion = true;
                collision = new Rectangle(
                    (int)Game1.Player.Sprite.Position.X - Engine.TileWidth * 2,
                    (int)Game1.Player.Sprite.Position.Y,
                    Engine.TileWidth,
                    Engine.TileHeight);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.D) && !inMotion)
            {
                motion.X = 1;
                Game1.Player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
                Game1.Player.Sprite.IsAnimating = true;
                inMotion = true;
                collision = new Rectangle(
                    (int)Game1.Player.Sprite.Position.X + Engine.TileWidth * 2,
                    (int)Game1.Player.Sprite.Position.Y,
                    Engine.TileWidth,
                    Engine.TileHeight);
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                motion *= 
                    (Game1.Player.Sprite.Speed * 
                    (float)gameTime.ElapsedGameTime.TotalSeconds
                    * Settings.Multiplier);
                Rectangle pRect = new Rectangle(
                        (int)(Game1.Player.Sprite.Position.X + motion.X),
                        (int)(Game1.Player.Sprite.Position.Y + motion.Y),
                        Engine.TileWidth,
                        Engine.TileHeight);

                if (pRect.Intersects(collision))
                {
                    Game1.Player.Sprite.IsAnimating = false;
                    inMotion = false;
                    motion = Vector2.Zero;
                }

                foreach (Point p in world.Map.CharacterLayer.Characters.Keys)
                {
                    Rectangle r = new Rectangle(
                        p.X * Engine.TileWidth,
                        p.Y * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight);

                    if (r.Intersects(pRect))
                    {
                        motion = Vector2.Zero;
                        Game1.Player.Sprite.IsAnimating = false;
                        inMotion = false;
                    }
                }

                foreach (Point p in world.Map.CollisionLayer.Collisions.Keys)
                {
                    Rectangle r = new Rectangle(
                        p.X * Engine.TileWidth,
                        p.Y * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight);

                    if (r.Intersects(pRect))
                    {
                        motion = Vector2.Zero;
                        Game1.Player.Sprite.IsAnimating = false;
                        inMotion = false;
                    }
                }
                Vector2 newPosition = Game1.Player.Sprite.Position + motion;
                newPosition.X = (int)newPosition.X;
                newPosition.Y = (int)newPosition.Y;

                Game1.Player.Sprite.Position = newPosition;
                motion = Game1.Player.Sprite.LockToMap(
                    new Point(
                        world.Map.WidthInPixels,
                        world.Map.HeightInPixels),
                    motion);

                if (motion == Vector2.Zero)
                {
                    Vector2 origin = new Vector2(
                            Game1.Player.Sprite.Position.X + Game1.Player.Sprite.Origin.X,
                            Game1.Player.Sprite.Position.Y + Game1.Player.Sprite.Origin.Y);
                    Game1.Player.Sprite.Position = Engine.VectorFromOrigin(origin);
                    inMotion = false;
                    Game1.Player.Sprite.IsAnimating = false;
                }
            }
        }

        private void StartInteraction()
        {
            foreach (Point s in world.Map.CharacterLayer.Characters.Keys)
            {
                Character c = world.Map.CharacterLayer.Characters[s];

                AnimationKey animation = Game1.Player.Sprite.CurrentAnimation;

                if (animation == AnimationKey.WalkLeft &&
                    ((int)c.Sprite.Position.X > (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y != (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkUp &&
                    ((int)c.Sprite.Position.X != (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y > (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkRight &&
                    ((int)c.Sprite.Position.X < (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y != (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkDown &&
                    ((int)c.Sprite.Position.X != (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y < (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                float distance = Vector2.Distance(
                    Game1.Player.Sprite.Origin + Game1.Player.Sprite.Position,
                    c.Sprite.Origin + c.Sprite.Position);

                if (Math.Abs(distance) < Engine.TileWidth + Engine.TileWidth / 2)
                {
                    manager.PushState(
                        (ConversationState)GameRef.ConversationState);

                    GameRef.ConversationState.SetConversation(c);
                    GameRef.ConversationState.StartConversation();
                    break;
                }
            }
        }

        private void StartBattle()
        {
            foreach (Point s in world.Map.CharacterLayer.Characters.Keys)
            {
                Character c = world.Map.CharacterLayer.Characters[s];

                AnimationKey animation = Game1.Player.Sprite.CurrentAnimation;

                if (animation == AnimationKey.WalkLeft &&
                    ((int)c.Sprite.Position.X > (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y != (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkUp &&
                    ((int)c.Sprite.Position.X != (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y > (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkRight &&
                    ((int)c.Sprite.Position.X < (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y != (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                if (animation == AnimationKey.WalkDown &&
                    ((int)c.Sprite.Position.X != (int)Game1.Player.Sprite.Position.X ||
                        (int)c.Sprite.Position.Y < (int)Game1.Player.Sprite.Position.Y))
                {
                    continue;
                }

                float distance = Vector2.Distance(
                    Game1.Player.Sprite.Origin + Game1.Player.Sprite.Position,
                    c.Sprite.Origin + c.Sprite.Position);

                if (Math.Abs(distance) < Engine.TileWidth + Engine.TileWidth / 2 && c.Alive())
                {
                    GameRef.StartBattleState.SetCombatants(Game1.Player, c);
                    manager.PushState(GameRef.StartBattleState);
                    break;
                }
            }
        }

        internal void SaveGame()
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                string path = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData);
                path += "\\ShadowMonsters\\";

                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch
                {
                    // uh oh
                }

                try
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                    FileStream stream = new FileStream(
                        path + "ShadowMonsters.sav",
                        FileMode.Create,
                        FileAccess.Write);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        encryptor,
                        CryptoStreamMode.Write))
                    {
                        BinaryWriter writer = new BinaryWriter(cryptoStream);
                        world.Save(writer);
                        Game1.Player.Save(writer);
                        writer.Close();
                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch
                {
                    // uh oh
                }
            }
        }

        internal void LoadGame()
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                string path = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData);
                path += "\\ShadowMonsters\\";

                try
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                    FileStream stream = new FileStream(
                        path + "ShadowMonsters.sav",
                        FileMode.Open,
                        FileAccess.Read);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        decryptor,
                        CryptoStreamMode.Read))
                    {
                        BinaryReader reader = new BinaryReader(cryptoStream);
                        world = World.Load(content, reader);
                        world.ChangeMap(world.CurrentMapName);
                        Game1.Player = Player.Load(GameRef, reader);
                        reader.Close();
                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception exc)
                {
                    exc.ToString();
                }
            }
        }

        internal void SetUpNewGame()
        {
            Task.Factory.StartNew(() => HandleNew());
        }

        private void HandleNew()
        {
            MoveManager.FillMoves();
            ShadowMonsterManager.FromFile(@".\Content\ShadowMonsters.txt", content);

            Game1.Player.AddShadowMonster(ShadowMonsterManager.GetShadowMonster("Water1"));
            Game1.Player.SetCurrentShadowMonster(0);
            Game1.Player.BattleShadowMonsters[0] = Game1.Player.GetShadowMonster(0);

            TileSet set = new TileSet(10, 10, 16, 16);

            set.TextureNames.Add("tiny-16");
            set.Textures.Add(content.Load<Texture2D>(@"Tiles\tiny-16"));

            TileLayer groundLayer = new TileLayer(100, 100, 0, 1);
            TileLayer edgeLayer = new TileLayer(100, 100);
            TileLayer buildingLayer = new TileLayer(100, 100);
            TileLayer decorationLayer = new TileLayer(100, 100);

            for (int i = 0; i < 1000; i++)
            {
                decorationLayer.SetTile(random.Next(0, 100), random.Next(0, 100), 0, 0);
            }

            TileMap map = new TileMap(set, groundLayer, edgeLayer, buildingLayer, decorationLayer, "level1");

            Character c = Character.FromString(GameRef, "Paul,ninja_m,WalkDown,PaulHello,0,2:2,fire1,fire1,,,,,,dark1");
            c.Sprite.Position = new Vector2(
                c.SourceTile.X * Engine.TileWidth,
                c.SourceTile.Y * Engine.TileHeight);

            map.CharacterLayer.Characters.Add(c.SourceTile, c);

            Merchant m = Merchant.FromString(GameRef, "Bonnie,ninja_f,WalkLeft,BonnieHello,0,4:4,earth1,earth1,,,,,,");
            m.Sprite.Position = new Vector2(
                m.SourceTile.X * Engine.TileWidth,
                m.SourceTile.Y * Engine.TileHeight);

            m.Backpack.AddItem("Potion", 99);
            m.Backpack.AddItem("Antidote", 10);
            m.Backpack.AddItem("Binding Scroll", 99);

            map.CharacterLayer.Characters.Add(m.SourceTile, m);

            map.CollisionLayer.Collisions.Add(new Point(5, 5), CollisionType.Impassable);
            map.CollisionLayer.Collisions.Add(new Point(6, 6), CollisionType.Impassable);
            
            WildArea area = new WildArea
            {
                TopLeft = new Point(0, 0),
                BottomRight = new Point(3, 3)
            };

            area.Monsters.Add("Dark1");
            area.Monsters.Add("Earth1");

            map.WildLayer.Areas.Add(area);

            world = new World(new Portal(new Point(10, 10), new Point(10, 10), map.MapName));
            world.Maps.Add(map.MapName, map);
            world.ChangeMap(map.MapName);
            Game1.Player.Sprite.Position = new Vector2(
                world.StartingMap.SourceTile.X * Engine.TileWidth,
                world.StartingMap.SourceTile.Y * Engine.TileHeight);
            manager.PopState();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

#if DEBUG
            world.Draw(gameTime, GameRef.SpriteBatch, Engine.Camera, true);
#else
            world.Draw(gameTime, GameRef.SpriteBatch, Engine.Camera);
#endif

            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Engine.Camera.Transformation);
            Game1.Player.Draw(gameTime);
            GameRef.SpriteBatch.End();
        }

        public void ResetEngine()
        {
            engine = new Engine(
                new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y),
                Settings.TileSize.X,
                Settings.TileSize.Y);
        }

        public override void Hide()
        {
            base.Hide();

            if (subscribed)
            {
                GameRef.YesNoState.YesButton.Click -= YesButton_Click;
                GameRef.YesNoState.NoButton.Click -= NoButton_Click;
                subscribed = false;
            }
        }
    }
}
