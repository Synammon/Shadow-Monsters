using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Controls;
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
            Settings.Resolution = Game1.Resolutions[resolutions.SelectedItem];
        }

        public override void Update(GameTime gameTime)
        {
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

        #endregion
    }
}
