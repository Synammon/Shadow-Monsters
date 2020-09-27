using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Controls;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class OptionState : BaseGameState
    {
        #region Field Region

        LeftRightSelector resolutions;
        Button apply;
        Button back;
        Button save;
        TimeSpan timer;
        Point previousResolution;
        private bool countDown;
        private bool subscribed;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public OptionState(Game game) : base(game)
        {
        }

        #endregion
        
        #region Method Region

        protected override void LoadContent()
        {
            base.LoadContent();

            resolutions = new LeftRightSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"),
                null);

            resolutions.Position = new Vector2(100, 100);

            List<string> items = new List<string>();

            foreach (string s in Game1.Resolutions.Keys)
                items.Add(s);

            resolutions.SetItems(items.ToArray(), 400);

            apply = new Button(content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Apply",
                Position = new Vector2(700, 100)
            };

            apply.Click += Apply_Click;

            back = new Button(content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Back",
                Position = new Vector2(100, 300)
            };

            back.Click += Back_Click;

            save = new Button(content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Save",
                Position = new Vector2(100, 200)
            };

            save.Click += Save_Click;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Settings.Save();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            manager.PopState();
            GameRef.GamePlayState.ResetEngine();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            GameRef.GraphicsDeviceManager.PreferredBackBufferWidth = Game1.Resolutions[resolutions.SelectedItem].X;
            GameRef.GraphicsDeviceManager.PreferredBackBufferHeight = Game1.Resolutions[resolutions.SelectedItem].Y;
            GameRef.GraphicsDeviceManager.ApplyChanges();

            previousResolution = Settings.Resolution;
            Settings.Resolution = Game1.Resolutions[resolutions.SelectedItem];
            GameRef.GamePlayState.ResetEngine();

            if (Game1.Player != null)
            {
                Game1.Player.Sprite.Position = new Vector2(
                    Game1.Player.Tile.X * Engine.TileWidth,
                    Game1.Player.Tile.Y * Engine.TileHeight);
            }

            manager.PushState(GameRef.YesNoState);

            GameRef.YesNoState.Message = "Keep changes?";

            if (!subscribed)
            {
                GameRef.YesNoState.YesButton.Click += YesButton_Click;
                GameRef.YesNoState.NoButton.Click += NoButton_Click;
                subscribed = true;
            }

            Visible = true;
            Enabled = true;

            timer = TimeSpan.FromSeconds(15);
            countDown = true;
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            Settings.Resolution = previousResolution;

            GameRef.GraphicsDeviceManager.PreferredBackBufferWidth = Settings.Resolution.X;
            GameRef.GraphicsDeviceManager.PreferredBackBufferHeight = Settings.Resolution.Y;
            GameRef.GraphicsDeviceManager.ApplyChanges();
            manager.PopState();
            countDown = false;

            if (Game1.Player != null)
            {
                Game1.Player.Sprite.Position = new Vector2(
                    Game1.Player.Tile.X * Engine.TileWidth,
                    Game1.Player.Tile.Y * Engine.TileHeight);
            }
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            manager.PopState();
            countDown = false;
        }

        public override void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime;

            if (timer <= TimeSpan.Zero && countDown)
            {
                countDown = false;
                manager.PopState();
                NoButton_Click(this, null);
            }

            resolutions.Update(gameTime);
            apply.Update(gameTime);
            back.Update(gameTime);
            save.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();
            resolutions.Draw(GameRef.SpriteBatch);
            apply.Draw(GameRef.SpriteBatch);
            back.Draw(GameRef.SpriteBatch);
            save.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        public override void Show()
        {
            base.Show();
            int count = 0;

            foreach (var v in Game1.Resolutions.Values)
            {
                if (v == Settings.Resolution)
                {
                    resolutions.SelectedIndex = count;
                    break;
                }

                count++;
            }
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

        #endregion
    }
}
