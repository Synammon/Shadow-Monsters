using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowMonsters.GameStates
{
    public class LoadingState : BaseGameState
    {
        private Texture2D background;
        private Color tint;
        private TimeSpan timer = TimeSpan.Zero;

        public LoadingState(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            background = content.Load<Texture2D>(@"Backgrounds\ShadowMonsters");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            tint = Color.Lerp(Color.White, Color.Blue, (float)Math.Abs(Math.Cos(timer.TotalSeconds)));
            timer += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720).Scale(Settings.Scale), Color.White);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Loading",
                new Vector2(
                    (Settings.Resolution.X - FontManager.GetFont("testfont").MeasureString("Loading").X) / 2,
                    (Settings.Resolution.Y - 100)),
                tint);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Show()
        {
            timer = TimeSpan.Zero;
            base.Show();
        }
    }
}
