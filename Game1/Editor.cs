using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters;
using ShadowMonsters.Controls;
using ShadowMonsters.ShadowMonsters;
using ShadowMonsters.TileEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using WF = System.Windows.Forms;

namespace ShadowEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Editor : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int frameCount = 0;
        World world;
        Camera camera;
        Rectangle viewPort = new Rectangle(0, 0, 64 * 18, 1080);
        ControlManager controls;

        PictureBox pbTileset;
        PictureBox pbPreview;
        ListBox lbLayers;
        ListBox lbTileSets;
        ListBox lbWorld;
        int selectedTile;
        bool Paint = true;
        int brushSize = 1;

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

        public Editor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            new Engine(viewPort);
            camera = new Camera();

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Components.Add(new Xin(this));
            Game1.BuildAnimations();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MoveManager.FillMoves();
            ShadowMonsterManager.FromFile(@".\Content\ShadowMonsters.txt", Content);

            controls = new ControlManager(Content.Load<SpriteFont>(@"InterfaceFont"))
            {
                AcceptInput = true
            };

            Texture2D texture = new Texture2D(
                GraphicsDevice,
                64,
                64);

            Color[] area = new Color[64 * 64];

            for (int i = 0; i < area.Length; i++)
            {
                area[i] = new Color(255, 255, 255, 128);
            }

            texture.SetData(area);

            CollisionLayer.Texture = texture;

            area = new Color[64 * 64];

            for (int i = 0; i < area.Length; i++)
            {
                area[i] = new Color(0, 0, 255, 128);
            }

            texture.SetData(area);

            PortalLayer.Texture = texture;

            Texture2D background = new Texture2D(
                graphics.GraphicsDevice,
                100,
                150);

            Color[] buffer = new Color[100 * 150];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.White;
            }

            background.SetData(buffer);

            Texture2D cursor = new Texture2D(
                GraphicsDevice,
                100,
                12);

            buffer = new Color[100 * 12];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Blue;
            }

            cursor.SetData(buffer);

            pbTileset = new PictureBox(
                new Texture2D(
                    graphics.GraphicsDevice,
                    512,
                    512),
                new Rectangle(
                    64 * 18,
                    128,
                    1920 - 64 * 18,
                    512));
            pbPreview = new PictureBox(
                new Texture2D(
                    graphics.GraphicsDevice,
                    64,
                    64),
                new Rectangle(
                    64 * 18,
                    25,
                    64,
                    64));

            lbLayers = new ListBox(
                background,
                cursor)
            {
                Position = new Vector2(64 * 18, 800),
                TabStop = false
            };

            lbLayers.Items.Add("Ground");
            lbLayers.Items.Add("Edge");
            lbLayers.Items.Add("Decorations");
            lbLayers.Items.Add("Building");

            lbTileSets = new ListBox(
                background,
                cursor)
            {
                Position = new Vector2(64 * 18 + 150, 800),
                TabStop = false
            };

            lbTileSets.SelectionChanged += LbTileSets_SelectionChanged;

            lbWorld = new ListBox(
                background,
                cursor)
            {
                Position = new Vector2(64 * 18 + 325, 800),
                TabStop = true,
                Enabled = true,
                HasFocus = true
            };

            lbWorld.SelectionChanged += LbWorld_SelectionChanged;

            controls.Add(lbWorld);
            controls.Add(lbTileSets);
            controls.Add(lbLayers);
            controls.Add(pbTileset);
            controls.Add(pbPreview);
        }

        private void LbWorld_SelectionChanged(object sender, EventArgs e)
        {
            world.ChangeMap(lbWorld.SelectedItem);
            pbTileset.Image = world.Map.TileSet.Textures[0];
            pbPreview.Image = world.Map.TileSet.Textures[0];

            lbTileSets.Items.Clear();
            foreach (string s in world.Map.TileSet.TextureNames)
                lbTileSets.Items.Add(s);

            lbTileSets.SelectedIndex = 0;
            pbPreview.SourceRectangle =
                world.Map.TileSet.SourceRectangles[0];
            pbTileset.SourceRectangle = new Rectangle(
                0,
                0,
                world.Map.TileSet.Textures[0].Width,
                world.Map.TileSet.Textures[0].Height);

            camera.Position = Vector2.Zero;
            camera.LockCamera(world.Map, viewPort);
        }

        private void LbTileSets_SelectionChanged(object sender, System.EventArgs e)
        {
            pbTileset.Image = world.Map.TileSet.Textures[lbTileSets.SelectedIndex];
            pbPreview.Image = world.Map.TileSet.Textures[lbTileSets.SelectedIndex];
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Point tile;

            if (!IsActive)
                return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            frameCount++;

            if (Xin.CheckKeyReleased(Keys.D1))
            {
                brushSize = 1;
            }

            if (Xin.CheckKeyReleased(Keys.D2))
            {
                brushSize = 2;
            }

            if (Xin.CheckKeyReleased(Keys.D4))
            {
                brushSize = 4;
            }

            if (Xin.CheckKeyReleased(Keys.D8))
            {
                brushSize = 8;
            }
            if (Xin.CheckKeyReleased(Keys.N) && frameCount > 5)
            {
                NewMapForm form = new NewMapForm(GraphicsDevice);

                form.ShowDialog();

                if (form.OkPressed)
                {
                    world.Maps.Add(form.TileMap.MapName, form.TileMap);
                    world.ChangeMap(form.TileMap.MapName);

                    lbWorld.Items.Add(world.Map.MapName);

                    camera.Position = Vector2.Zero;
                    camera.LockCamera(world.Map, viewPort);

                    pbTileset.Image = world.Map.TileSet.Textures[0];
                    pbPreview.Image = world.Map.TileSet.Textures[0];

                    lbTileSets.Items.Clear();
                    foreach (string s in world.Map.TileSet.TextureNames)
                        lbTileSets.Items.Add(s);

                    lbTileSets.SelectedIndex = 0;
                    pbPreview.SourceRectangle =
                        world.Map.TileSet.SourceRectangles[0];
                    pbTileset.SourceRectangle = new Rectangle(
                        0,
                        0,
                        world.Map.TileSet.Textures[0].Width,
                        world.Map.TileSet.Textures[0].Height);
                }

                frameCount = 0;
            }

            Vector2 position = new Vector2
            {
                X = Xin.MouseAsPoint.X + camera.Position.X,
                Y = Xin.MouseAsPoint.Y + camera.Position.Y
            };

            tile = Engine.VectorToCell(position);

            if (Xin.CheckKeyReleased(Keys.P))
            {
                Paint = !Paint;
            }

            if (world != null && world.Maps.Count > 0 && viewPort.Contains(Xin.MouseAsPoint))
            {
                if (Xin.MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (Paint)
                    {
                        switch (lbLayers.SelectedIndex)
                        {
                            case 0:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetGroundTile(tile.X + i, tile.Y + j, lbTileSets.SelectedIndex, selectedTile);
                                    }
                                }
                                break;
                            case 1:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetEdgeTile(tile.X + i, tile.Y + j, lbTileSets.SelectedIndex, selectedTile);
                                    }
                                }
                                break;
                            case 2:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetDecorationTile(tile.X + i, tile.Y + j, lbTileSets.SelectedIndex, selectedTile);
                                    }
                                }
                                break;
                            case 3:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetBuildingTile(tile.X + i, tile.Y + j, lbTileSets.SelectedIndex, selectedTile);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (!world.Map.CollisionLayer.Collisions.ContainsKey(
                            new Point(
                                tile.X,
                                tile.Y)))
                        {
                            world.Map.CollisionLayer.Collisions.Add(
                                new Point(
                                tile.X,
                                tile.Y),
                                ShadowMonsters.TileEngine.CollisionType.Impassable);
                        }
                    }
                }

                if (Xin.MouseState.RightButton == ButtonState.Pressed)
                {
                    if (Paint)
                    {
                        switch (lbLayers.SelectedIndex)
                        {
                            case 0:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetGroundTile(tile.X + i, tile.Y + j, -1, -1);
                                    }
                                }
                                break;
                            case 1:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetEdgeTile(tile.X + i, tile.Y + j, -1, -1);
                                    }
                                }
                                break;
                            case 2:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetDecorationTile(tile.X + i, tile.Y + j, -1, -1);
                                    }
                                }
                                break;
                            case 3:
                                for (int i = 0; i < brushSize; i++)
                                {
                                    for (int j = 0; j < brushSize; j++)
                                    {
                                        world.Map.SetBuildingTile(tile.X + i, tile.Y + j, -1, -1);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (world.Map.CollisionLayer.Collisions.ContainsKey(
                            new Point(
                                tile.X,
                                tile.Y)))
                        {
                            world.Map.CollisionLayer.Collisions.Remove(
                            new Point(
                                tile.X,
                                tile.Y));
                        }
                    }
                }
            }

            if (pbTileset != null &&
                pbTileset.DestinationRectangle.Contains(
                Xin.MouseAsPoint) &&
                Xin.CheckMouseReleased(MouseButtons.Left))
            {
                Point previewPoint = new Point(
                    Xin.MouseAsPoint.X - pbTileset.DestinationRectangle.X,
                    Xin.MouseAsPoint.Y - pbTileset.DestinationRectangle.Y);
                float xScale = (float)world.Map.TileSet.Textures[lbTileSets.SelectedIndex].Width /
                    pbTileset.DestinationRectangle.Width;

                float yScale = (float)world.Map.TileSet.Textures[lbTileSets.SelectedIndex].Height /
                    pbTileset.DestinationRectangle.Height;

                Point tilesetPoint = new Point(
                    (int)(previewPoint.X * xScale), 
                    (int)(previewPoint.Y * yScale));

                Point clickedTile = new Point(
                    tilesetPoint.X / world.Map.TileSet.TileWidth,
                    tilesetPoint.Y / world.Map.TileSet.TileHeight);

                selectedTile = clickedTile.Y * world.Map.TileSet.TilesWide + clickedTile.X;
                pbPreview.SourceRectangle =
                    world.Map.TileSet.SourceRectangles[selectedTile];
            }

            if (Xin.CheckKeyReleased(Keys.C) && frameCount > 5 && world.Maps.Count > 0)
            {
                CharacterListForm frm = new CharacterListForm(world.Map, this);
                frm.ShowDialog();
            }

            if (Xin.CheckKeyReleased(Keys.M) && frameCount > 5 && world.Maps.Count > 0)
            {
                MerchantListForm frm = new MerchantListForm(world.Map, this);
                frm.ShowDialog();
            }

            if (Xin.CheckKeyReleased(Keys.D) && frameCount > 5 && world.Maps.Count > 0)
            {
                PortalListForm frm = new PortalListForm(world.Map, this);
                frm.ShowDialog();
            }

            if (Xin.CheckKeyReleased(Keys.S) && frameCount > 5 && world.Maps.Count > 0)
            {
                ShadowMonsterListForm frm = new ShadowMonsterListForm(world.Map, this);
                frm.ShowDialog();
            }

            if (Xin.CheckKeyReleased(Keys.Q) && frameCount > 5)
            {
                DefinitionListForm frm = new DefinitionListForm();
                frm.ShowDialog();
            }
            if (Xin.CheckKeyReleased(Keys.W) && frameCount > 5)
            { 
                if (world != null)
                {
                    WF.DialogResult result = WF.MessageBox.Show("Proceeding will delete existing world. Are you sure you want to continue?", "Continue?", WF.MessageBoxButtons.YesNo);
                    if (result == WF.DialogResult.No)
                    {
                        return;
                    }
                }

                WorldForm form = new WorldForm();
                form.ShowDialog();

                if (!form.OKPressed)
                {
                    return;
                }

                world = form.World;
            }
            if (Xin.CheckKeyReleased(Keys.F1) && frameCount > 5)
            {
                WF.SaveFileDialog sfd = new WF.SaveFileDialog();
                sfd.Filter = "World (*.wrld)|*.wrld";
                WF.DialogResult result = sfd.ShowDialog();

                if (result == WF.DialogResult.OK)
                {
                    SaveWorld(sfd.FileName);
                }
            }

            if (Xin.CheckKeyReleased(Keys.F2) && frameCount > 5)
            {
                WF.OpenFileDialog ofd = new WF.OpenFileDialog();
                ofd.Filter = "World (*.wrld)|*.wrld";
                WF.DialogResult result = ofd.ShowDialog();

                if (result == WF.DialogResult.OK)
                {
                    LoadWorld(ofd.FileName);

                    pbTileset.Image = world.Map.TileSet.Textures[0];
                    pbPreview.Image = world.Map.TileSet.Textures[0];

                    lbTileSets.Items.Clear();
                    foreach (string s in world.Map.TileSet.TextureNames)
                        lbTileSets.Items.Add(s);

                    lbTileSets.SelectedIndex = 0;
                    pbPreview.SourceRectangle =
                        world.Map.TileSet.SourceRectangles[0];
                    pbTileset.SourceRectangle = new Rectangle(
                        0,
                        0,
                        world.Map.TileSet.Textures[0].Width,
                        world.Map.TileSet.Textures[0].Height);

                    camera.Position = Vector2.Zero;
                    camera.LockCamera(world.Map, viewPort);
                }
            }

            HandleScrollMap();
            controls.Update(gameTime);

            if (world != null  && world.Maps.Count > 0)
            {
                world.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void HandleScrollMap()
        {
            if (world == null || world.Maps.Count == 0)
            {
                return;
            }

            if (Xin.MouseAsPoint.X > 64 * 17 && Xin.MouseAsPoint.X < 64 * 18)
            {
                camera.Position = new Vector2(camera.Position.X + 8, camera.Position.Y);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Right))
            {
                camera.Position = new Vector2(camera.Position.X + 8, camera.Position.Y);
            }

            if (Xin.MouseAsPoint.Y > 1080 - 64 && Xin.MouseAsPoint.Y < 1080)
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 8);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 8);
            }

            if (Xin.MouseAsPoint.X < 64)
            {
                camera.Position = new Vector2(camera.Position.X - 8, camera.Position.Y);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Left))
            {
                camera.Position = new Vector2(camera.Position.X - 8, camera.Position.Y);
            }

            if (Xin.MouseAsPoint.Y < 64)
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 8);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Up))
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 8);
            }

            if (world != null && world.Maps.Count > 0)
            {
                camera.LockCamera(world.Map, viewPort);
            }
        }

        private void SaveWorld(string fileName)
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                try
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                    FileStream stream = new FileStream(
                        fileName,
                        FileMode.Create,
                        FileAccess.Write);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        encryptor,
                        CryptoStreamMode.Write))
                    {
                        BinaryWriter writer = new BinaryWriter(cryptoStream);
                        world.Save(writer);
                        writer.Close();
                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception exc)
                {
                    WF.MessageBox.Show("There was an error saving the world.Map." + exc.ToString());
                }
            }
        }

        private void LoadWorld(string fileName)
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                try
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                    FileStream stream = new FileStream(
                        fileName,
                        FileMode.Open,
                        FileAccess.Read);

                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        decryptor,
                        CryptoStreamMode.Read))
                    {
                        BinaryReader reader = new BinaryReader(cryptoStream);

                        world = World.Load(Content, reader);
                        lbWorld.Items.Clear();

                        foreach (TileMap m in world.Maps.Values)
                        {
                            lbWorld.Items.Add(m.MapName);
                        }

                        reader.Close();
                    }

                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception exc)
                {
                    WF.MessageBox.Show("There was a problem loading the world.Map. " + exc.ToString());
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            if (world != null && world.Maps.Count > 0)
            {
                world.Draw(gameTime, spriteBatch, camera, true);
            }

            spriteBatch.Begin();
            controls.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
