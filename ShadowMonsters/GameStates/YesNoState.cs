using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Controls;

namespace ShadowMonsters.GameStates
{
    public class YesNoState : BaseGameState
    {
        #region Field Region

        private Rectangle destination;
        private Texture2D background;
        private Button yesButton;
        private Button noButton;

        #endregion

        #region Property Region

        public Button YesButton
        {
            get { return yesButton; }
        }

        public Button NoButton
        {
            get { return noButton; }
        }

        public string Message
        {
            get; set;
        }
        #endregion

        #region Constructor Region

        public YesNoState(Game game) : base(game)
        {
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            background = new Texture2D(GraphicsDevice, 500, 300);

            Color[] data = new Color[500 * 300];
                
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.DarkBlue;
            }

            background.SetData(data);

            destination = new Rectangle(
                (int)(1280 - background.Width) / 2,
                (int)(720 - background.Height) / 2,
                background.Width,
                background.Height);

            yesButton = new Button(
                content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Yes"
            };
            yesButton.Position = new Vector2(
                destination.X + (background.Width - yesButton.Width * 2 - 50) / 2,
                destination.Y + (background.Height - yesButton.Height - 20));

            noButton = new Button(
                content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "No",
                Position = new Vector2(
                    (yesButton.Position.X + yesButton.Width + 25),
                    (yesButton.Position.Y))
            };
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            yesButton.Update(gameTime);
            noButton.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            StringBuilder sb = new StringBuilder();
            float currentLength = 0f;
            int lines = 1;

            string[] parts = Message.Split(' ');

            foreach (string s in parts)
            {
                Vector2 size = FontManager.GetFont("testfont").MeasureString(s);

                if (currentLength + size.X < background.Width * Settings.Scale.X - 75 * Settings.Scale.X)
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

            GameRef.SpriteBatch.Begin();
            GameRef.SpriteBatch.Draw(background, destination.Scale(Settings.Scale), Color.White);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                text,
                new Vector2(destination.X + 20, destination.Y + 20).Scale(Settings.Scale),
                Color.White);
            yesButton.Draw(GameRef.SpriteBatch);
            noButton.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        public override void Show()
        {
            Visible = true;
            base.Show();
        }
        #endregion
    }
}
