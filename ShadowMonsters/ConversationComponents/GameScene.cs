using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ConversationComponents
{
    public class GameScene
    {
        #region Field Region

        protected Game game;
        protected string text;
        private List<SceneOption> options;
        private int selectedIndex;
        private Color highLight;
        private Color normal;
        private Vector2 textPosition;
        private bool isMouseOver;

        private Vector2 menuPosition = new Vector2(50f, Settings.Resolution.Y * .75f);

        #endregion

        #region Property Region

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public List<SceneOption> Options
        {
            get { return options; }
            set { options = value; }
        }

        public SceneAction OptionAction
        {
            get { return options[SelectedIndex].OptionAction; }
        }

        public string OptionScene
        {
            get { return options[SelectedIndex].OptionScene; }
        }

        public string OptionText
        {
            get { return options[SelectedIndex].OptionText; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = MathHelper.Clamp(value, 0, options.Count - 1); }
        }

        public bool IsMouseOver
        {
            get { return isMouseOver; }
        }

        public Color NormalColor
        {
            get { return normal; }
            set { normal = value; }
        }

        public Color HighLightColor
        {
            get { return highLight; }
            set { highLight = value; }
        }

        public Vector2 MenuPosition
        {
            get { return menuPosition; }
        }

        #endregion

        #region Constructor Region

        private GameScene()
        {
            NormalColor = Color.Blue;
            HighLightColor = Color.Red;
            options = new List<SceneOption>();
            menuPosition = new Vector2(50f, Settings.Resolution.Y * .75f);
        }

        public GameScene(string text, List<SceneOption> options)
            : this()
        {
            this.text = text;
            this.options = options;
            textPosition = Vector2.Zero;
        }

        public GameScene(Game game, string text, List<SceneOption> options)
            : this(text, options)
        {
            this.game = game;
        }

        #endregion

        #region Method Region

        public void SetText(string text)
        {
            textPosition = new Vector2(Settings.Resolution.X / 3, 50);

            StringBuilder sb = new StringBuilder();
            float currentLength = 0f;

            string[] parts = text.Split(' ');

            foreach (string s in parts)
            {
                Vector2 size = FontManager.GetFont("testfont").MeasureString(s);

                if (currentLength + size.X < Settings.Resolution.X *  (2 / 3f) - 50f)
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
                    currentLength = 0;
                }
            }

            this.text = sb.ToString();
        }

        public void Initialize()
        {
        }

        public void Update()
        {
            if (Xin.CheckKeyReleased(Keys.Up) || Xin.CheckKeyReleased(Keys.W))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = options.Count - 1;
                }
            }
            else if (Xin.CheckKeyReleased(Keys.Down) || Xin.CheckKeyReleased(Keys.S))
            {
                selectedIndex++;
                if (selectedIndex > options.Count - 1)
                {
                    selectedIndex = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D background)
        {
            Color myColor;

            if (textPosition == Vector2.Zero)
            {
                SetText(text);
            }

            if (background != null)
            {
                spriteBatch.Draw(background, Vector2.Zero, Color.White);
            }

            spriteBatch.DrawString(FontManager.GetFont("testfont"),
                text,
                textPosition,
                Color.White);

            Vector2 position = new Vector2(50f, Settings.Resolution.Y * .75f);

            Rectangle optionRect = new Rectangle(
                0, 
                (int)position.Y, 
                Settings.Resolution.X, 
                FontManager.GetFont("testfont").LineSpacing);

            isMouseOver = false;

            for (int i = 0; i < options.Count; i++)
            {
                if (optionRect.Contains(Xin.MouseState.Position))
                {
                    selectedIndex = i;
                    isMouseOver = true;
                }

                if (i == SelectedIndex)
                {
                    myColor = HighLightColor;
                }
                else
                {
                    myColor = NormalColor;
                }

                spriteBatch.DrawString(FontManager.GetFont("testfont"),
                    options[i].OptionText,
                    position,
                    myColor);

                position.Y += FontManager.GetFont("testfont").LineSpacing + 5;
                optionRect.Y += FontManager.GetFont("testfont").LineSpacing + 5;
            }
        }

        #endregion
    }
}
