using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Components
{
    public class MenuComponent : DrawableGameComponent
    {
        #region Fields

        Game1 gameRef;
        readonly List<string> menuItems = new List<string>();
        int selectedIndex = -1;
        bool mouseOver;

        int width;
        int height;

        Color normalColor = Color.White;
        Color hiliteColor = Color.Red;
        readonly Texture2D texture;

        Vector2 position;

        #endregion Fields

        #region Properties

        public Vector2 Postion
        {
            get { return position; }
            set { position = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(
                        value,
                        0,
                        menuItems.Count - 1);
            }
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }

        public Color HiliteColor
        {
            get { return hiliteColor; }
            set { hiliteColor = value; }
        }

        public bool MouseOver
        {
            get { return mouseOver; }
        }

        #endregion Properties

        #region Constructors

        public MenuComponent(Game game, Texture2D texture) : base(game)
        {
            this.gameRef = (Game1)game;
            this.mouseOver = false;
            this.texture = texture;
        }

        public MenuComponent(Game game, Texture2D texture, string[] menuItems)
            : this(game, texture)
        {
            selectedIndex = 0;

            foreach (string s in menuItems)
            {
                this.menuItems.Add(s);
            }

            MeassureMenu();
        }

        #endregion Constructors

        #region Methods

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            MeassureMenu();

            selectedIndex = 0;
        }

        private void MeassureMenu()
        {
            width = texture.Width;
            height = 0;

            foreach (string s in menuItems)
            {
                Vector2 size = FontManager.GetFont("testfont").MeasureString(s);

                if (size.X > width)
                    width = (int)size.X;

                height += texture.Height + 50;
            }

            height -= 50;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 menuPosition = position;
            Point p = Xin.MouseState.Position;

            Rectangle buttonRect;
            mouseOver = false;

            for (int i = 0; i < menuItems.Count; i++)
            {
                buttonRect = new Rectangle((int)menuPosition.X, (int)menuPosition.Y, texture.Width, texture.Height);

                if (buttonRect.Contains(p))
                {
                    selectedIndex = i;
                    mouseOver = true;
                }

                menuPosition.Y += texture.Height + 50;
            }

            if (!mouseOver && (Xin.CheckKeyReleased(Keys.Up)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Count - 1;
                }
            }
            else if (!mouseOver && (Xin.CheckKeyReleased(Keys.Down)))
            {
                selectedIndex++;
                if (selectedIndex > menuItems.Count - 1)
                {
                    selectedIndex = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 menuPosition = position;
            Color myColor;
            float myScale;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                {
                    myColor = HiliteColor;
                    myScale = (2f + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2)) / 2.5f;
                }
                else
                {
                    myColor = NormalColor;
                    myScale = 1f;
                }

                gameRef.SpriteBatch.Draw(texture, menuPosition, Color.White);

                Vector2 textSize = FontManager.GetFont("testfont").MeasureString(menuItems[i]);

                Vector2 textPosition = menuPosition + new Vector2((int)(texture.Width - textSize.X) / 2, (int)(texture.Height - textSize.Y) / 2);
                gameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    menuItems[i],
                    textPosition,
                    myColor,
                    0f,
                    Vector2.Zero,
                    myScale,
                    SpriteEffects.None,
                    1f);

                menuPosition.Y += texture.Height + 50;
            }
        }

        #endregion Methods

        #region Virtual Methods
        #endregion Virtual Methods

    }
}
