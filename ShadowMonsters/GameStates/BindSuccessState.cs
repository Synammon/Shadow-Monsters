using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class BindSuccessState : BaseGameState
    {
        private Texture2D background;
        private Texture2D bindTexture;

        private TimeSpan timer;

        public ShadowMonster Monster { get; internal set; }

        public Texture2D Background => background;

        public TimeSpan Timer { get => timer; set => timer = value; }

        public BindSuccessState(Game game) : base(game)
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

            bindTexture = content.Load<Texture2D>(@"Backgrounds\binding");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Timer += gameTime.ElapsedGameTime;

            if (Timer.TotalSeconds > 2f)
            {
                Timer = TimeSpan.FromSeconds(2f);
            }

            if (Timer.TotalSeconds >= 2f && (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckMouseReleased(MouseButtons.Left)))
            {
                manager.PopState();
                manager.PopState();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            GameRef.SpriteBatch.Draw(
                background,
                new Rectangle(
                    0,
                    (int)(720 * .75f),
                    Settings.Resolution.X,
                    (int)(Settings.Resolution.Y * .25f)), Color.White);

            StringBuilder sb = new StringBuilder();
            float currentLength = 0f;
            int lines = 1;

            string[] parts = (Monster.Name + " was successfully bound.").Split(' ');

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

            //GameRef.SpriteBatch.Draw(background, new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y), Color.White);

            if (timer.TotalSeconds < 2f)
            {
                GameRef.SpriteBatch.Draw(
                    bindTexture,
                    new Vector2(900, 200).Scale(Settings.Scale),
                    null,
                    Color.White,
                    (float)timer.TotalSeconds,
                    new Vector2(bindTexture.Width / 2, bindTexture.Height / 2),
                    2.5f,
                    SpriteEffects.None,
                    1f);
            }

            if (timer.TotalSeconds >= 2f)
            {
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
            }

            GameRef.SpriteBatch.End();
        }

        public override void Show()
        {
            timer = TimeSpan.Zero;
            base.Show();
        }
    }
}
