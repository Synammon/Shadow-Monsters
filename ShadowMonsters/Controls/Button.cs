using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Controls
{
    public class Button : Control
    {
        #region

        public event EventHandler Click;

        #endregion
        #region Field Region

        Texture2D _background;
       
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region  

        public Button(Texture2D background)
        {
            _background = background;
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

            Rectangle destination = new Rectangle(
                (int)Position.X, 
                (int)Position.Y, 
                _background.Width, 
                _background.Height).Scale(scale);
            spriteBatch.Draw(_background, destination, Color.White);
            _spriteFont = FontManager.GetFont("testfont");
            Vector2 size = _spriteFont.MeasureString(Text);
            Vector2 offset = new Vector2((_background.Width * scale.X - size.X) / 2, (_background.Height * scale.Y - size.Y) / 2);
            spriteBatch.DrawString(_spriteFont, Text, (Position.Scale(scale) + offset), Color);
        }

        public override void HandleInput()
        {
            Point position = Xin.MouseAsPoint;
            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

            Rectangle destination = new Rectangle(
                (int)_position.X, 
                (int)_position.Y, 
                _background.Width, 
                _background.Height).Scale(scale);

            if (destination.Contains(position) && Xin.CheckMouseReleased(MouseButtons.Left))
            {
                OnClick();
            }
        }

        private void OnClick()
        {
            Muse.PlaySoundEffect("menu_click");
            Click?.Invoke(this, null);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        #endregion
    }
}
