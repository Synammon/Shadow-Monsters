using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class MessageState : BaseGameState
    {
        private Texture2D background;
        public string Message { get; set; }

        public MessageState(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            background = new Texture2D(GraphicsDevice, 1280, (int)(720 * .25f));
            Color[] data = new Color[background.Width * background.Height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }

            background.SetData(data);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.CheckAnyKeyPressed() || Xin.CheckMouseReleased(MouseButtons.Left))
            {
                manager.PopState();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            StringBuilder sb = new StringBuilder();
            float currentLength = 0f;
            int lines = 1;

            string[] parts = Message.Split(' ');

            foreach (string s in parts)
            {
                Vector2 size = FontManager.GetFont("testfont").MeasureString(s);

                if (currentLength + size.X < Settings.Resolution.X * (0.95f))
                {
                    sb.Append(s);
                    sb.Append(" ");
                    currentLength += size.X;
                }
                else
                {
                    sb.Append("\n\r");
                    sb.Append(s);
                    sb.Append(" ");
                    currentLength = size.X;
                    lines++;
                }
            }

            string text = sb.ToString();

            GameRef.SpriteBatch.Draw(
                background, 
                new Rectangle(
                    0, 
                    (int)(720 * .75f), 
                    Settings.Resolution.X, 
                    (int)(Settings.Resolution.Y *.25f)), Color.White);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                text,
                new Vector2(20, 720 * .75f).Scale(Settings.Scale),
                Color.Black);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Continue",
                new Vector2(20, 720 * .75f + lines * FontManager.GetFont("testfont").LineSpacing).Scale(Settings.Scale),
                Color.Red);
            GameRef.SpriteBatch.End();
        }
    }
}
