using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShadowMonsters.GameStates
{
    public class TitleState : BaseGameState
    {
        private float alpha = 0f;
        private Color tint = new Color(1f, 1f, 1f, 0f);
        private TimeSpan timer = TimeSpan.Zero;
        private Texture2D _background;

        public TitleState(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _background = content.Load<Texture2D>(@"Backgrounds\ShadowMonsters");
            
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            alpha = (float)timer.TotalSeconds / 2;
            tint = new Color(alpha, alpha, alpha, alpha);
            
            if (timer.TotalSeconds >= 2 && (Xin.CheckAnyKeyReleased() || Xin.CheckMouseReleased(MouseButtons.Left)))
            {
                manager.PopState();
                manager.PushState(GameRef.MainMenuState);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            base.Draw(gameTime);

            GameRef.SpriteBatch.Draw(_background, new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y), tint);
            
            if (timer.TotalSeconds >= 2)
            {
                GameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    "Press any key to begin.",
                    (new Vector2((1280 - FontManager.GetFont("testfont").MeasureString("Press any key to begin.").X) / 2, 680)).Scale(Settings.Scale),
                    Color.White);
            }

            GameRef.SpriteBatch.End();
        }
    }
}
